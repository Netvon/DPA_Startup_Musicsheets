﻿using Core.IO;
using Core.Memento;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Editor
{
    public class EditorMemento : ICloneable
    {
        Sheet _musicSheet = new Sheet();
        string _text = "";

        public Sheet MusicSheet => _musicSheet;

        public string Text => _text;

        public void SetMusicSheet(Sheet sheet)
        {
            _musicSheet = sheet;

            var factory = new SheetWriterFactory();
            var writer = factory.GetWriter(".ly");
            _text = writer.WriteToString(_musicSheet).Result;
        }

        public void InsertText(string add)
        {
            var temp = _text.Insert(CursorIndex, add);

            SetText(temp);

            CursorIndex += add.Length;
        }

        public void SetText(string text)
        {
            _text = text;

            var factory = new SheetReaderFactory();
            var writer = factory.GetReader(".ly");

            _musicSheet = writer.ReadFromStringAsync(_text).Result;
        }

        public object Clone()
        {
            var temp = new EditorMemento();
            temp._text = _text;
            temp._musicSheet = _musicSheet;
            temp.CursorIndex = CursorIndex;

            return temp;
        }

        public int CursorIndex { get; set; }
    }

    class EditorCareTaker : CareTaker<EditorMemento>
    {

    }
}
