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

        private bool _textChangedByLoad = false;
        private DateTime _lastChange;
        private static int MILLISECONDS_BEFORE_CHANGE_HANDLED = 1500;
        private bool _waitingForRender = false;
        private int _cursorLocation;

        public LilypondViewModel(FileHandler fileHandler)
        {
            _fileHandler = fileHandler;
            writerFactory = new SheetWriterFactory();
            careTaker = new EditorCareTaker();
            careTaker.MementoChanged += CareTaker_MementoChanged;

            _fileHandler.LilypondTextChanged += (src, e) =>
            {
                _textChangedByLoad = true;
                LilypondText = _previousText = e.LilypondText;
                _textChangedByLoad = false;
            };

            _text = "Your lilypond text will appear here.";
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
            base.RaisePropertyChanged(nameof(ViewModels.LilypondViewModel.UndoCommand));
        }, (Func<bool>)(() => (bool)careTaker.CanUndo));

        public RelayCommand RedoCommand => new RelayCommand(() =>
        {
            var editorMemento = careTaker.Redo();
            CursorLocation = editorMemento.CursorIndex;

            LilypondText = editorMemento.Text;

            base.RaisePropertyChanged(nameof(RedoCommand));
            base.RaisePropertyChanged(nameof(ViewModels.LilypondViewModel.UndoCommand));
        }, (Func<bool>)(() => (bool)careTaker.CanRedo));

        public ICommand SaveAsCommand => new RelayCommand(() =>
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Lilypond|*.ly|PDF|*.pdf" };
            if (saveFileDialog.ShowDialog() == true)
            {
                var writer = writerFactory.GetWriter(saveFileDialog.FileName);
                writer.WriteToFile(careTaker.Current.MusicSheet);
            }
        });

        Task inputWaitTask;
        CancellationTokenSource tokenSource = new CancellationTokenSource();

        public ICommand KeyDownCommand => new RelayCommand<KeyEventArgs>((e) =>
        {
            tokenSource.Cancel();
            tokenSource = new CancellationTokenSource();

            var key = e.Key;
            if (key == Key.System)
                key = e.SystemKey;

            var keyname = Enum.GetName(typeof(Key), key);

            if (commands.Handle(keyname, careTaker))
            {
                inputWaitTask = Task.Delay(200).ContinueWith(
                    (task, obj) =>
                    {
                        commands.InvokeLast(careTaker);
                    }, tokenSource.Token);
            }
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

            if (MessageBox.Show("You have changes in you're lilypond, you want to save it?", "Unsaved changes!",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                SaveAsCommand.Execute(null);
            }       
        }

    }
}
