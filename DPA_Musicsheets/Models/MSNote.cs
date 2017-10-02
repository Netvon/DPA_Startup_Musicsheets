using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Models
{
    public abstract class MSNote
    {
        public bool Dotted { get; set; }
        public NoteType NoteType { get; set; }
    }
}
