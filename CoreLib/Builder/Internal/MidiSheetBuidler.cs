using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Sanford.Multimedia.Midi;

namespace Core.Builder.Internal
{
    class MidiSheetBuidler : SheetBuilder
    {
        readonly IEnumerable<IMessageHandler> messageHandlers;

        public MidiSheetBuidler()
        {
            messageHandlers = Util.Reflection.GetInstancesOf<IMessageHandler>();
        }

        override public void AddKey(SheetKey key)
        {
            throw new NotImplementedException();
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
