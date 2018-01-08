﻿using Core.IO;
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

        public LilypondViewModel(FileHandler fileHandler)
        {
            _fileHandler = fileHandler;
            writerFactory = new SheetWriterFactory();
            careTaker = new EditorCareTaker();

            _fileHandler.LilypondTextChanged += (src, e) =>
            {
                _textChangedByLoad = true;
                LilypondText = _previousText = e.LilypondText;
                _textChangedByLoad = false;
            };

            _text = "Your lilypond text will appear here.";
        }
        
        public ICommand TextChangedCommand => new RelayCommand<TextChangedEventArgs>((args) =>
        {
            if (!_textChangedByLoad)
            {
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
            LilypondText = careTaker.Undo().Text;

            RaisePropertyChanged(nameof(RedoCommand));
            RaisePropertyChanged(nameof(UndoCommand));
        }, () => careTaker.CanUndo);

        public RelayCommand RedoCommand => new RelayCommand(() =>
        {
            LilypondText = careTaker.Redo().Text;

            RaisePropertyChanged(nameof(RedoCommand));
            RaisePropertyChanged(nameof(UndoCommand));
        }, () => careTaker.CanRedo);

        public ICommand SaveAsCommand => new RelayCommand(() =>
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Lilypond|*.ly|PDF|*.pdf" };
            if (saveFileDialog.ShowDialog() == true)
            {
                var writer = writerFactory.GetWriter(saveFileDialog.FileName);
                writer.WriteToFile(careTaker.Current.MusicSheet);
            }
        });

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
