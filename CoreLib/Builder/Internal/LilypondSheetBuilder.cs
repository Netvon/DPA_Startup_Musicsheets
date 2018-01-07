using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Builder.Internal.Lilypond;
using Core.Models;

namespace Core.Builder.Internal
{
    public class LilypondSheetBuilder : SheetBuilder
    {
        List<ILilypondTokenHandler> handlerInstances;
        List<string> tokens;

        public LilypondSheetBuilder(IEnumerable<string> tokens)
        {
            this.tokens = new List<string>(tokens);
        }

        public override Sheet Build()
        {
            sheet = new Sheet();
            handlerInstances = new List<ILilypondTokenHandler>(Util.Reflection.GetInstancesOf<ILilypondTokenHandler>());

            var allTokens = tokens.ToList();
            foreach (var token in tokens)
            {
                var index = allTokens.IndexOf(token);
                string previous = null;
                string next = null;

                if( index > 1 )
                    previous = allTokens[index - 1];

                if (index + 1 < allTokens.Count)
                {
                    next = allTokens[index + 1];
                }

                handlerInstances.Where(x => x.Accepts(previous, token, next))
                    .ToList()
                    .ForEach(x => x.Handle(token, this));
            }

            tokens.Clear();
            handlerInstances.Clear();

            return sheet;
        }
    }
}
