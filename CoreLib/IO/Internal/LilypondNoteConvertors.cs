using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IO.Internal
{
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class ConvertTypeAttribute : Attribute
    {
        public ConvertTypeAttribute(Type convertType)
        {
            ConvertType = convertType;
        }

        public Type ConvertType { get; set; }
    }

    interface INoteConvertor
    {
        string Convert(MSNote note, Sheet sheet);
    }

    [ConvertType(typeof(PitchNote))]
    class LilypondPitchNoteConvertors : INoteConvertor
    {
        public string Convert(MSNote note, Sheet sheet)
        {
            var notestr = "";
            if(note is PitchNote p)
            {
                var pitch = Enum.GetName(typeof(NotePitch), p.Pitch).ToLower();
                notestr += pitch;

                if (p.Modifier == NoteModifier.Flat)
                    notestr += "es";
                else if (p.Modifier == NoteModifier.Sharp)
                    notestr += "is";

                var octave = p.Octave - sheet.GlobalOctave;

                if (octave < 0)
                    notestr += "".PadLeft(Math.Abs(octave), ',');
                else
                    notestr += "".PadLeft(Math.Abs(octave), '\'');
            }

            return notestr;
        }
    }
}
