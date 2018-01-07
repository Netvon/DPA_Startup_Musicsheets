using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Builder.Internal.Lilypond
{
    class TimeHandler : ILilypondTokenHandler
    {
        readonly static string match_token = "\\time";

        bool contextActive;

        public bool Accepts(string previous, string token, string next)
        {
            if (token == match_token || contextActive)
            {
                contextActive = true;
                return true;
            }

            return false;
        }

        public void Handle(string token, SheetBuilder builder)
        {
            if (contextActive && token != match_token && token.Contains("/"))
            {
                var values = token.Split('/');

                if (values.Length == 2)
                {
                    builder.AddTimeSignature(uint.Parse(values[0]), uint.Parse(values[1]));

                    contextActive = false;
                }
            }
        }
    }
}
