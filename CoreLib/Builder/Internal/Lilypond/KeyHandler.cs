using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Builder.Internal.Lilypond
{
    class KeyHandler : ILilypondTokenHandler
    {
        readonly static string match_token = "\\relative";

        bool contextActive;
        bool lookForStartBracket;

        public bool Accepts(string previous, string token, string next)
        {
            if(token == match_token || contextActive)
            {
                contextActive = true;
                return true;
            }

            if(lookForStartBracket && token.Contains("{"))
            {
                return true;
            }

            return false;
        }

        public void Handle(string token, SheetBuilder builder)
        {
            if(contextActive && token != match_token)
            {
                var key = token[0].ToString().ToUpper();
                builder.AddKey((SheetKey)Enum.Parse(typeof(SheetKey), key));

                int up = token.Count(x => x == '\'');
                int down = token.Count(x => x == ',');

                //builder.AddGlobalNoteOctave(up + down);

                contextActive = false;
                lookForStartBracket = true;
            }
        }
    }
}
