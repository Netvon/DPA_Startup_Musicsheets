using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Memento
{
    class LilypondMemento
    {
        public LilypondMemento(string text)
        {
            LilypondText = text;
        }

        public string LilypondText { get; set; }
    }
}
