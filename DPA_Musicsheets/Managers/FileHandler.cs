
using Core.IO.Internal;
using DPA_Musicsheets.Models;
using PSAMControlLibrary;
using PSAMWPFControlLibrary;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Text.RegularExpressions;
using DPA_Musicsheets.Convertors;
using Core.IO;

namespace DPA_Musicsheets.Managers
{
    public class FileHandler
    {
        private string _lilypondText;
        public string LilypondText
        {
            get { return _lilypondText; }
            set
            {
                _lilypondText = value;
                LilypondTextChanged?.Invoke(this, new LilypondEventArgs() { LilypondText = value });
            }
        }
        public List<MusicalSymbol> WPFStaffs { get; set; } = new List<MusicalSymbol>();
        private static List<Char> notesorder = new List<Char> { 'c', 'd', 'e', 'f', 'g', 'a', 'b' };

        public Sequence MidiSequence { get; set; }

        public event EventHandler<LilypondEventArgs> LilypondTextChanged;
        public event EventHandler<WPFStaffsEventArgs> WPFStaffsChanged;
        public event EventHandler<MidiSequenceEventArgs> MidiSequenceChanged;

        private int _beatNote = 4;  // De waarde van een beatnote.
        private int _bpm = 120;     // Aantal beatnotes per minute.
        private int _beatsPerBar;   // Aantal beatnotes per maat.

        async public Task OpenFile(string fileName)
        {
            var sheetReaderFactory = new SheetReaderFactory();
            var sheetWriterFactory = new SheetWriterFactory();
            var sheetReader = sheetReaderFactory.GetReader(fileName);
            var converter = new SheetToWPFConverter();
            var writer = sheetWriterFactory.GetWriter(".ly");

            var sheet = await sheetReader.ReadFromFileAsync();
            
            WPFStaffs.Clear();
            WPFStaffs.AddRange(converter.ConvertSheet(sheet));
            WPFStaffsChanged?.Invoke(this, new WPFStaffsEventArgs() { Symbols = WPFStaffs });

            LilypondTextChanged?.Invoke(this, new LilypondEventArgs { LilypondText = await writer.WriteToString(sheet) });

            MidiSequence = GetSequenceFromWPFStaffs();
            MidiSequenceChanged?.Invoke(this, new MidiSequenceEventArgs() { MidiSequence = MidiSequence });
        }

        async public Task textChanged(string lilypondText)
        {
            var sheetReaderFactory = new SheetReaderFactory();
            var sheetReader = sheetReaderFactory.GetReader(".ly");
            var sheet = await sheetReader.ReadFromStringAsync(lilypondText);
            var converter = new SheetToWPFConverter();

            WPFStaffs.Clear();
            WPFStaffs.AddRange(converter.ConvertSheet(sheet));
            WPFStaffsChanged?.Invoke(this, new WPFStaffsEventArgs() { Symbols = WPFStaffs });

            MidiSequence = GetSequenceFromWPFStaffs();
            MidiSequenceChanged?.Invoke(this, new MidiSequenceEventArgs() { MidiSequence = MidiSequence });
        }

        private Sequence GetSequenceFromWPFStaffs()
        {
            List<string> notesOrderWithCrosses = new List<string>() { "c", "cis", "d", "dis", "e", "f", "fis", "g", "gis", "a", "ais", "b" };
            int absoluteTicks = 0;

            Sequence sequence = new Sequence();

            Track metaTrack = new Track();
            sequence.Add(metaTrack);

            // Calculate tempo
            int speed = (60000000 / _bpm);
            byte[] tempo = new byte[3];
            tempo[0] = (byte)((speed >> 16) & 0xff);
            tempo[1] = (byte)((speed >> 8) & 0xff);
            tempo[2] = (byte)(speed & 0xff);
            metaTrack.Insert(0 /* Insert at 0 ticks*/, new MetaMessage(MetaType.Tempo, tempo));

            Track notesTrack = new Track();
            sequence.Add(notesTrack);

            foreach (MusicalSymbol musicalSymbol in WPFStaffs)
            {
                switch (musicalSymbol.Type)
                {
                    case MusicalSymbolType.Note:
                        Note note = musicalSymbol as Note;

                        // Calculate duration
                        double absoluteLength = 1.0 / (double)note.Duration;
                        absoluteLength += (absoluteLength / 2.0) * note.NumberOfDots;

                        double relationToQuartNote = _beatNote / 4.0;
                        double percentageOfBeatNote = (1.0 / _beatNote) / absoluteLength;
                        double deltaTicks = (sequence.Division / relationToQuartNote) / percentageOfBeatNote;

                        // Calculate height
                        int noteHeight = notesOrderWithCrosses.IndexOf(note.Step.ToLower()) + ((note.Octave + 1) * 12);
                        noteHeight += note.Alter;
                        notesTrack.Insert(absoluteTicks, new ChannelMessage(ChannelCommand.NoteOn, 1, noteHeight, 90)); // Data2 = volume

                        absoluteTicks += (int)deltaTicks;
                        notesTrack.Insert(absoluteTicks, new ChannelMessage(ChannelCommand.NoteOn, 1, noteHeight, 0)); // Data2 = volume

                        break;
                    case MusicalSymbolType.TimeSignature:
                        byte[] timeSignature = new byte[4];
                        timeSignature[0] = (byte)_beatsPerBar;
                        timeSignature[1] = (byte)(Math.Log(_beatNote) / Math.Log(2));
                        metaTrack.Insert(absoluteTicks, new MetaMessage(MetaType.TimeSignature, timeSignature));
                        break;
                    default:
                        break;
                }
            }

            notesTrack.Insert(absoluteTicks, MetaMessage.EndOfTrackMessage);
            metaTrack.Insert(absoluteTicks, MetaMessage.EndOfTrackMessage);
            return sequence;
        }
    }
}
