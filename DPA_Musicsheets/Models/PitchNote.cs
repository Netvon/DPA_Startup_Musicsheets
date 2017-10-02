using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    class PitchNote : MSNote
    {
        public NotePitch Pitch { get; set; }
        public NoteModifier Modifier { get; set; }
    }
}
