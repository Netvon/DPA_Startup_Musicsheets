using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Core.IO.Internal
{
    [SheetWriter(".ly", nameof(LilypondSheetWriter))]
    public class LilypondSheetWriter : ISheetWriter
    {
        string filePath;

        public void SetFilePath(string filePath)
        {
            this.filePath = filePath;
        }

        public async Task WriteToFile(Sheet sheet)
        {
            var text = await WriteToString(sheet);

            using (var writer = File.CreateText(filePath))
            {
                await writer.WriteAsync(text);
            }
        }

        public Task<string> WriteToString(Sheet sheet)
        {
            var header = "";

            var key = Enum.GetName(typeof(SheetKey), sheet.Key);

            var globalOctave = "";

            if (sheet.GlobalOctave < 0)
                globalOctave += "".PadLeft(Math.Abs(sheet.GlobalOctave), ',');
            else
                globalOctave += "".PadLeft(Math.Abs(sheet.GlobalOctave), '\'');

            header += $"\\relative {key.ToLower()}{globalOctave} {{\n  \\clef treble\n";

            string time = "";

            var tempo = $"  \\tempo 4={sheet.Tempo}\n\n";
            string notes = "  ";

            foreach (var bar in sheet.Bars)
            {
                if(string.IsNullOrWhiteSpace(time))
                    time = $"  \\time {bar.UpperSignature}/{bar.LowerSignature}\n";

                foreach (var note in bar.Notes)
                {
                    var notestr = "";

                    if(note is PitchNote p)
                    {
                        var pitch = Enum.GetName(typeof(NotePitch), p.Pitch).ToLower();
                        notestr += pitch;

                        if (p.Modifier == NoteModifier.Flat)
                            notestr += "es";
                        else if(p.Modifier == NoteModifier.Sharp)
                            notestr += "is";

                        var octave = p.Octave - sheet.GlobalOctave;

                        if (octave < 0)
                            notestr += "".PadLeft(Math.Abs(octave), ',');
                        else
                            notestr += "".PadLeft(Math.Abs(octave), '\'');
                    }
                    else if(note is RestNote r)
                    {
                        notestr += "r";
                    }

                    notestr += note.BaseLength.ToString();
                    notestr += "".PadLeft((int)note.Dotts, '.');

                    notes += notestr + " ";
                }

                notes += "|\n  ";
            }

            return Task.FromResult(header + time + tempo + notes + "\n}");
        }
    }
}
