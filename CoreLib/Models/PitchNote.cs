﻿using System;
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

        public int Octave { get; set; } = 4;

        public override string ToString()
        {
            return $"{Pitch}{Modifier} Octave: {Octave}, Dotts: {Dotts}, Length: {Length}";
        }
    }
}
