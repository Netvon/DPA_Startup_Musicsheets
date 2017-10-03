using Core.Models;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Builder.Internal
{
    interface IMessageHandler
    {
        bool Accepts(IMidiMessage type);
        void Handle(IMidiMessage message, SheetBuilder builder);
    }
}
