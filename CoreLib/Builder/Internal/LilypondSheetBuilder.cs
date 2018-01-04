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
            foreach (var token in tokens)
            {
                handlerInstances.Where(x => x.Accepts(token))
                    .ToList()
                    .ForEach(x => x.Handle(token, this));
            }

            throw new NotImplementedException();
        }
    }
}
