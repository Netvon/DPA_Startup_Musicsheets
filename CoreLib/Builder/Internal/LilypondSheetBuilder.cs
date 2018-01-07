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
        readonly IEnumerable<ILilypondTokenHandler> handlerInstances;
        readonly IEnumerable<string> tokens;

        public LilypondSheetBuilder(IEnumerable<string> tokens)
        {
            this.tokens = tokens;
            handlerInstances = Util.Reflection.GetInstancesOf<ILilypondTokenHandler>();
        }

        public override Sheet Build()
        {
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

            return sheet;
        }
    }
}
