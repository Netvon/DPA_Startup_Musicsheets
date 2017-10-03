using Core.Models;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Builder.Internal
{
    interface IMetaTypeHandler
    {
        bool Accepts(MetaMessage message);
        void Handle(MetaMessage message, SheetBuilder builder);
    }
}
