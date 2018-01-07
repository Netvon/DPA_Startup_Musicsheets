using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Sheet
    {
        public string Name { get; set; }
        public SheetKey Key { get; set; }
        public uint Tempo { get; set; }

        public List<Bar> Bars { get; set; } = new List<Bar>();
        public int GlobalOctave { get; set; }

        public uint UpperSignature { get; set; }
        public uint LowerSignature { get; set; }
    }
}
