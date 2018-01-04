using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Builder.Internal.Lilypond
{
    class NoteHandler : ILilypondTokenHandler
    {
        readonly string[] names = Enum.GetNames(typeof(NotePitch));

        List<MSNote> currentBar = new List<MSNote>();

        public bool Accepts(string token)
        {
            return token.Contains("|") || (names.Any(x => x == token[0].ToString().ToUpper()) && token.Length > 1);
        }

        public void Handle(string token, SheetBuilder builder)
        {
            if (token.Contains("|"))
            {
                builder.AddBar(currentBar);
                currentBar.Clear();
            }
            else
            {
                var noteBuidler = builder.GetNoteBuilder();

                if (token[0] == 'r')
                {
                    noteBuidler.AddRest();
                }
                else
                {
                    var pitch = (NotePitch)Enum.Parse(typeof(NotePitch), token[0].ToString().ToUpper());
                    noteBuidler.AddPitch(pitch);
                }

                int up = token.Count(x => x == '\'');
                int down = token.Count(x => x == ',');
                int dots = token.Count(x => x == '.');

                var numberMatches = Regex.Match(token, @"(\d)");

                var length = int.Parse(numberMatches.Groups[0].Value);

                noteBuidler.AddDotts((uint)dots);
                noteBuidler.AddOctave(up + down);
                noteBuidler.AddBaseLength((uint)length);

                currentBar.Add(noteBuidler.Build());
            }

        }
    }
}
