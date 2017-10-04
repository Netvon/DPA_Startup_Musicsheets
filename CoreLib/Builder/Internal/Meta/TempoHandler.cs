using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanford.Multimedia.Midi;

namespace Core.Builder.Internal.Meta
{
    class TempoHandler : IMetaTypeHandler
    {
        public bool Accepts(MetaMessage message) => message.MetaType == MetaType.Tempo;

        public void Handle(MetaMessage message, SheetBuilder builder)
        {
            var tempoBytes = message.GetBytes();
            int tempo = (tempoBytes[0] & 0xff) << 16 | (tempoBytes[1] & 0xff) << 8 | (tempoBytes[2] & 0xff);
            var bpm = 60000000 / tempo;

            builder.AddTempto((uint)bpm);
        }
    }
}
