using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Builder.Internal.Channel
{
    class ChannelMessageHandler : IMessageHandler
    {
        readonly IEnumerable<IChannelCommandHandler> metaTypeHandlers;

        public bool Accepts(IMidiMessage type) => type.MessageType == MessageType.Channel;

        public ChannelMessageHandler()
        {
            metaTypeHandlers = Util.Reflection.GetInstancesOf<IChannelCommandHandler>();
        }

        public void Handle(IMidiMessage message, SheetBuilder sheet)
        {
            var channelMessage = message as ChannelMessage;
            var handler = metaTypeHandlers.FirstOrDefault(h => h.Accepts(channelMessage));

            handler?.Handle(channelMessage, sheet);
        }
    }
}
