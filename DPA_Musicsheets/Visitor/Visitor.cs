using Core.Models;
using Core.Visitor;
using PSAMControlLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Visitor
{
    public class Visitor : IVisitor
    {
        public MusicalSymbol Symbol { get; private set; }

        public void Accept(PitchNote pitchNote)
        {
            var p = new Note(pitchNote.Pitch.ToString(), (int)pitchNote.Modifier, pitchNote.Octave, (MusicalSymbolDuration)pitchNote.BaseLength, NoteStemDirection.Up, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Single });
            p.NumberOfDots = (int)pitchNote.Dotts;
            Symbol = p;
        }

        public void Accept(RestNote restNote)
        {
            var r = new Rest((MusicalSymbolDuration)restNote.BaseLength);
            r.NumberOfDots = (int)restNote.Dotts;
            Symbol = r;
        }
    }
}
