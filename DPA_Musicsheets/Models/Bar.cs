using System.Collections.Generic;

namespace DPA_Musicsheets.Models
{
    public class Bar
    {
        public uint UpperSignature { get; set; }
        public uint LowerSignature { get; set; }

        public List<MSNote> Notes { get; set; }

    }
}