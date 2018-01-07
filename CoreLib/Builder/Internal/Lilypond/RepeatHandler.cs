using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Builder.Internal.Lilypond
{
    class RepeatHandler : ILilypondTokenHandler
    {
        readonly static string match_token = "\\repeat";
        readonly static string match_keyword = "volta";

        bool contextActive;

        bool bracketStart;
        bool bracketEnd;

        public bool Accepts(string previous, string token, string next)
        {
            if(bracketStart && token.Contains("}"))
            {
                bracketStart = false;
                bracketEnd = true;

                return true;
            }

            if (previous == match_token && token == match_keyword) {
                contextActive = true;
                return true;
            }

            if(previous == match_keyword && contextActive && next.Contains("{"))
            {
                var amount = Regex.Match(token, @"\d");
                contextActive = false;
                bracketStart = true;

                return amount.Success;
            }

            return false;
        }

        public void Handle(string token, SheetBuilder builder)
        {
            if(bracketEnd)
            {
                builder.AddStopRepeat();

                bracketEnd = false;
                return;
            }

            if (contextActive)
                return;

            var amount = uint.Parse(token);
            builder.AddRepeat(amount);
        }
    }
}
