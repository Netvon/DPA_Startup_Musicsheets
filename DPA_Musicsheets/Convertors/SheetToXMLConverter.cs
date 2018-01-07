using Core.Models;
using PSAMControlLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Convertors
{
    class SheetToXMLConverter : SheetConverter
    {
        public SheetToXMLConverter(string pType) : base(pType)
        {
        }

        public IEnumerable<MusicalSymbol> ConvertSheet(Sheet sheet)
        {
            List<MusicalSymbol> symbols = new List<MusicalSymbol>();

            var clef = new Clef((ClefType)sheet.Key, 2);
            symbols.Add(clef);
            var timeSignature = new TimeSignature(TimeSignatureType.Numbers, sheet.Bars[0].UpperSignature, sheet.Bars[0].LowerSignature);
            symbols.Add(timeSignature);

            foreach (var bar in sheet.Bars)
            {
                foreach (var note in bar.Notes) 
                {
                    if (note is RestNote restNote)
                    {
                        var r = new Rest((MusicalSymbolDuration)restNote.BaseLength);
                        r.NumberOfDots = (int)restNote.Dotts;
                        symbols.Add(r);
                    }

                    if (note is PitchNote pitchNote)
                    {
                        var n = new Note(pitchNote.Pitch.ToString(), (int)pitchNote.Modifier, pitchNote.Octave, (MusicalSymbolDuration)pitchNote.BaseLength, NoteStemDirection.Up, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Single });
                        n.NumberOfDots = (int)pitchNote.Dotts;
                        symbols.Add(n);
                    }
                }
                symbols.Add(new Barline());
            }

            return symbols;
        }

    }
}
