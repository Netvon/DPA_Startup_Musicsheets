using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Builder.Internal.Lilypond
{
    class AlternativeHandler : ILilypondTokenHandler
    {
        readonly static string match_token = "\\alternative";

        bool bracketStart;
        bool bracketEnd;

        bool altBracketStart;
        bool altBracketEnd;

        public bool Accepts(string previous, string token, string next)
        {
            if (token == match_token && next.Contains("{") )
            {
                bracketStart = true;
                return false;
            }

            if ( bracketStart && !altBracketStart && token.Contains("{"))
            {
                altBracketStart = true;
                return true;
            }

            if ( bracketStart && altBracketStart && token.Contains("}"))
            {
                altBracketEnd = true;
                return true;
            }

            if(bracketStart && !altBracketEnd && !altBracketStart && token.Contains("}"))
            {
                bracketEnd = true;
                return true;
            }

            return false;
        }

        public void Handle(string token, SheetBuilder builder)
        {
            if (altBracketStart && token.Contains("{"))
            {
                builder.AlternativeCount++;
                return;
            }

            if (altBracketEnd && token.Contains("}"))
            {
                altBracketEnd = false;
                altBracketStart = false;
                return;
            }

            if(bracketEnd)
            {
                builder.AlternativeCount = 0;
            }
        }
    }
}
