using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Builder.Internal.Lilypond
{
    class NoteHandler : ILilypondTokenHandler
    {
        readonly string[] names = Enum.GetNames(typeof(NotePitch));

        public bool Accepts(string token)
        {
            return names.Any(x => x == token[0].ToString().ToUpper()) && token.Length > 1;
        }

        public void Handle(string token, SheetBuilder builder)
        {
            throw new NotImplementedException();
        }
    }
}
