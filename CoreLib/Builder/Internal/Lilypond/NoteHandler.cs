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

        string nextToken;

        public bool Accepts(string previous, string token, string next)
        {
            nextToken = next;

            return CanParse(token);
        }

        bool CanParse(string token)
        {
            if (token.Contains("|"))
                return true;

            if (names.Any(x => x == token[0].ToString().ToUpper()) || token[0] == 'r')
            {
                var numberMatches = Regex.Match(token, @"\d");

                return numberMatches.Length > 0 && token.Length > 0;
            }

            return false;
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

                    if (token.Contains("is"))
                    {
                        noteBuidler.AddModifier(NoteModifier.Sharp);
                    }
                    else if (token.Contains("es") || token.Contains("as"))
                    {
                        noteBuidler.AddModifier(NoteModifier.Flat);
                    }
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

            if(!CanParse(nextToken))
            {
                builder.AddBar(currentBar);
                currentBar.Clear();
            }
        }
    }
}
