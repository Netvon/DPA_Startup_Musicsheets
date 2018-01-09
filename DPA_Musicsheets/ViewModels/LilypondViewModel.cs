using Core.Editor;
using Core.IO;
using Core.Memento;
using Core.Models;
using DPA_Musicsheets.Editor;
using DPA_Musicsheets.Managers;
using DPA_Musicsheets.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;

namespace DPA_Musicsheets.ViewModels
{
    public class LilypondViewModel : ViewModelBase
    {
        private FileHandler _fileHandler;
        private EditorCareTaker careTaker;
        private SheetWriterFactory writerFactory;
        private Core.Editor.Commands commands = Editor.Commands.Factory;

        private string _text;
        private string _previousText;
        private string _nextText;

        private bool _textChangedByLoad;
        private DateTime _lastChange;
        private static int MILLISECONDS_BEFORE_CHANGE_HANDLED = 1500;
        private bool _waitingForRender;
        private int _cursorLocation;

        Task inputWaitTask;
        CancellationTokenSource tokenSource = new CancellationTokenSource();
        private readonly IFileService fileService;
        private readonly IMessageService messageService;

        public string LilypondText
        {
            get
            {
                return _text;
            }
            set
            {
                if (!_waitingForRender && !_textChangedByLoad)
                {
                    _previousText = _text;
                }
                _text = value;
                RaisePropertyChanged(() => LilypondText);
            }
        }

        public LilypondViewModel(FileHandler fileHandler, IFileService fileService, IMessageService messageService)
        {
            _fileHandler = fileHandler;
            writerFactory = new SheetWriterFactory();
            careTaker = new EditorCareTaker();

            _fileHandler.LilypondTextChanged += (src, e) =>
            {
                _textChangedByLoad = true;
                LilypondText = _previousText = e.LilypondText;
                _textChangedByLoad = false;

                var temp = new EditorMemento();
                temp.SetText(LilypondText);

                careTaker.Save(temp);
            };

            _text = "Your lilypond text will appear here.";

            var initial = new EditorMemento();
            initial.SetText(LilypondText);
            careTaker.Save(initial);
            careTaker.MementoChanged += CareTaker_MementoChanged;

            this.fileService = fileService;
            this.messageService = messageService;
        }

        void CareTaker_MementoChanged(object sender, EditorMemento e)
        {
            LilypondText = e.Text;
        }

        public ICommand TextChangedCommand => new RelayCommand<TextChangedEventArgs>((args) =>
        {
            if (!_textChangedByLoad)
            {
                if (args.Source is TextBox tb)
                {
                    CursorLocation = tb.CaretIndex;

                    if(careTaker.Current != null)
                        careTaker.Current.CursorIndex = CursorLocation;

                    var clone = (careTaker.Current?.Clone() ?? new EditorMemento()) as EditorMemento;

                    clone.SetText(tb.Text);
                    careTaker.Save(clone);
                }

                _waitingForRender = true;
                _lastChange = DateTime.Now;
                MessengerInstance.Send(new CurrentStateMessage() { State = "Rendering..." });

                Task.Delay(MILLISECONDS_BEFORE_CHANGE_HANDLED).ContinueWith((task) =>
                {
                    if ((DateTime.Now - _lastChange).TotalMilliseconds >= MILLISECONDS_BEFORE_CHANGE_HANDLED)
                    {
                        _waitingForRender = false;
                        UndoCommand.RaiseCanExecuteChanged();

                        _fileHandler.LoadLilypond(LilypondText);
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext()); // Request from main thread.
            }
        });

        public RelayCommand UndoCommand => new RelayCommand(() =>
        {
            var editorMemento = careTaker.Undo();
            CursorLocation = editorMemento.CursorIndex;
            LilypondText = editorMemento.Text;

            base.RaisePropertyChanged(nameof(RedoCommand));
            base.RaisePropertyChanged(nameof(UndoCommand));
        }, () => careTaker.CanUndo);

        public RelayCommand RedoCommand => new RelayCommand(() =>
        {
            var editorMemento = careTaker.Redo();
            CursorLocation = editorMemento.CursorIndex;

            LilypondText = editorMemento.Text;

            base.RaisePropertyChanged(nameof(RedoCommand));
            base.RaisePropertyChanged(nameof(UndoCommand));
        }, () => careTaker.CanRedo);

        public ICommand SaveAsCommand => new RelayCommand(() =>
        {
            var fact = new SheetWriterFactory();
            var extensions = fact.GetAllSupportedExtension().Select(x => $"{x.name}|*{x.ext}");

            var path = fileService.RequestWritePath(string.Join("", extensions.ToArray()));

            if(!string.IsNullOrWhiteSpace(path))
            {
                var writer = writerFactory.GetWriter(path);
                writer.WriteToFile(careTaker.Current.MusicSheet);
            }
        });

        public ICommand KeyDownCommand => new RelayCommand<KeyEventArgs>((e) =>
        {
            tokenSource.Cancel();
            tokenSource = new CancellationTokenSource();

            var key = e.Key;
            if (key == Key.System)
                key = e.SystemKey;

            var keyname = Enum.GetName(typeof(Key), key);

            commands.Handle(keyname, careTaker);

            inputWaitTask = Task.Delay(200).ContinueWith(
                (task, obj) =>
                {
                    commands.InvokeLast(careTaker);
                }, tokenSource.Token);
        });

        public int CursorLocation
        {
            get => _cursorLocation;
            set
            {
                _cursorLocation = value;
                RaisePropertyChanged();
            }
        }

        public override void Cleanup()
        {
            base.Cleanup();

            var result = messageService.Ask("You have changes in you're lilypond, you want to save it?", "Unsaved Changes");

            if(result == AskQuestionResult.Yes)
            {
                SaveAsCommand.Execute(null);
            }
        }
    }
}
