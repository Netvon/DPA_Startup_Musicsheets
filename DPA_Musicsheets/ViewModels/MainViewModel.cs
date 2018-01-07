﻿using DPA_Musicsheets.Managers;
using DPA_Musicsheets.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Windows.Input;

namespace DPA_Musicsheets.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _fileName;
        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
                RaisePropertyChanged(nameof(FileName));
            }
        }

        private string _currentState;
        public string CurrentState
        {
            get { return _currentState; }
            set
            {
                _currentState = value;
                RaisePropertyChanged(() => CurrentState);
            }
        }

        private FileHandler _fileHandler;

        public MainViewModel(FileHandler fileHandler)
        {
            _fileHandler = fileHandler;
            FileName = @"Files/Five_little_ducks.mxl";

            MessengerInstance.Register<CurrentStateMessage>(this, (message) => CurrentState = message.State);
        }

        public ICommand OpenFileCommand => new RelayCommand(() =>
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "MusicXML or LilyPond files (*.ly *.mxl)|*.ly;*.mxl" };
            if (openFileDialog.ShowDialog() == true)
            {
                FileName = openFileDialog.FileName;
            }
        });
        public ICommand LoadCommand => new RelayCommand(() =>
        {
            _fileHandler.OpenFile(FileName);
        });
        
        public ICommand OnLostFocusCommand => new RelayCommand(() =>
        {
            Console.WriteLine("Maingrid Lost focus");
        });

        public ICommand OnKeyDownCommand => new RelayCommand<KeyEventArgs>((e) =>
        {
            //Console.WriteLine($"Key down: {e.Key}");
        });

        public ICommand OnKeyUpCommand => new RelayCommand(() =>
        {
            //Console.WriteLine("Key Up");
        });

        public ICommand OnWindowClosingCommand => new RelayCommand(() =>
        {
            ViewModelLocator.Cleanup();
        });
    }
}
