using System.Collections.Generic;

namespace Core.Models
{
    public enum Repeat
    {
        None, Start, End
    }

    public class Bar
    {
        public uint UpperSignature { get; set; }
        public uint LowerSignature { get; set; }

        public Repeat Repeat { get; set; }

        public uint RepeatAmount { get; set; }

        public List<MSNote> Notes { get; set; } = new List<MSNote>();

        public List<List<Bar>> Alternatives { get; set; } = new List<List<Bar>>();

        public bool HasAlternative => Alternatives.Count > 0;
    }
}