using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class PitchNote : MSNote
    {
        public NotePitch Pitch { get; set; }
        public NoteModifier Modifier { get; set; }

        public int Octave { get; set; }
    }
}
