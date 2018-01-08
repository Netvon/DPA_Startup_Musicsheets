using Core.IO;
using Core.Memento;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Editor
{
    public class EditorMemento
    {
        Sheet _musicSheet;
        string _text;

        public Sheet MusicSheet => _musicSheet;

        public string Text => Text;

        public void SetMusicSheet(Sheet sheet)
        {
            _musicSheet = sheet;

            var factory = new SheetWriterFactory();
            var writer = factory.GetWriter(".ly");
            writer.WriteToString(_musicSheet).ContinueWith(x =>
            {
                _text = x.Result;
            });
        }

        public void InsertText(string add)
        {
            var temp = _text.Insert(CursorIndex, add);

            SetText(temp);
        }

        public void SetText(string text)
        {
            _text = text;

            var factory = new SheetReaderFactory();
            var writer = factory.GetReader(".ly");

            writer.ReadFromStringAsync(_text).ContinueWith(x =>
            {
                _musicSheet = x.Result;
            });
        }

        public int CursorIndex { get; set; }
    }

    class EditorCareTaker : CareTaker<EditorMemento>
    {

    }
}
