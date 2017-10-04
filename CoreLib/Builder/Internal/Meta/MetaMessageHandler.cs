using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Sanford.Multimedia.Midi;

namespace Core.Builder.Internal.Meta
{
    class MetaMessageHandler : IMessageHandler
    {
        readonly IEnumerable<IMetaTypeHandler> metaTypeHandlers;

        public bool Accepts(IMidiMessage type) => type.MessageType == MessageType.Meta;

        public MetaMessageHandler()
        {
            metaTypeHandlers = Util.Reflection.GetInstancesOf<IMetaTypeHandler>();
        }

        public void Handle(IMidiMessage message, SheetBuilder sheet)
        {
            var metaMessage = message as MetaMessage;
            var handler = metaTypeHandlers.FirstOrDefault(h => h.Accepts(metaMessage));

            handler?.Handle(metaMessage, sheet);
        }
    }
}
