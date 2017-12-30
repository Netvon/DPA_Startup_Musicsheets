using System.Collections.Generic;

namespace Core.Models
{
    public class Bar
    {
        public uint UpperSignature { get; set; }
        public uint LowerSignature { get; set; }

        public IEnumerable<MSNote> Notes { get; set; } = new List<MSNote>();
    }
}