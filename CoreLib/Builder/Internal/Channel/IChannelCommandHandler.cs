using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Builder.Internal.Channel
{
    interface IChannelCommandHandler
    {
        bool Accepts(ChannelMessage message);
        void Handle(ChannelMessage message, SheetBuilder builder);
    }
}
