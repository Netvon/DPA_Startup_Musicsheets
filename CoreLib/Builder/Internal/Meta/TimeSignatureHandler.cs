using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Sanford.Multimedia.Midi;

namespace Core.Builder.Internal.Meta
{
    class TimeSignatureHandler : IMetaTypeHandler
    {
        public bool Accepts(MetaMessage message) => message.MetaType == MetaType.TimeSignature;

        public void Handle(MetaMessage message, SheetBuilder builder)
        {
            var timeSignatureBytes = message.GetBytes();
            var upper = (uint)timeSignatureBytes[0];
            var lower = (uint)(1 / Math.Pow(timeSignatureBytes[1], -2));

            builder.AddTimeSignature(upper, lower);
        }
    }
}
