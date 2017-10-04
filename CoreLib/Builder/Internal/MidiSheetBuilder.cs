using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Sanford.Multimedia.Midi;

namespace Core.Builder.Internal
{
    class MidiSheetBuilder : SheetBuilder
    {
        readonly IEnumerable<IMessageHandler> messageHandlers;

        public MidiSheetBuilder()
        {
            messageHandlers = Util.Reflection.GetInstancesOf<IMessageHandler>();
        }

        public void AddMidiMessage(IMidiMessage message)
        {
            var handler = messageHandlers.FirstOrDefault(x => x.Accepts(message));
            handler?.Handle(message, this);
        }

        override public Sheet Build()
        {
            throw new NotImplementedException();
        }
    }
}
