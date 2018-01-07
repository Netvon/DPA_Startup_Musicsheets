using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Builder.Internal.Lilypond
{
    interface ILilypondTokenHandler
    {
        bool Accepts(string previous, string token, string next);
        void Handle(string token, SheetBuilder builder);
    }
}
