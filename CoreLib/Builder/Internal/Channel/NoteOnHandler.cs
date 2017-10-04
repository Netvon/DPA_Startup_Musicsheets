using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanford.Multimedia.Midi;

namespace Core.Builder.Internal.Channel
{
    class NoteOnHandler : IChannelCommandHandler
    {
        // this shows that this instance is unique
        // Guid guid = Guid.NewGuid();

        int previousMidiKey = 60;

        public bool Accepts(ChannelMessage message) => message.Command == ChannelCommand.NoteOn;

        public void Handle(ChannelMessage message, SheetBuilder builder)
        {
            if(message.Data2 > 0) // loudness
            {
                var note = message.Data1 % 12;
                var test = message;
            }

            var outside = "oeps...";

            // throw new NotImplementedException();
        }

        string GetNoteName(int midiKey)
        {
            string name = "";
            switch (midiKey % 12)
            {
                case 0:
                    name = "c";
                    break;
                case 1:
                    name = "cis";
                    break;
                case 2:
                    name = "d";
                    break;
                case 3:
                    name = "dis";
                    break;
                case 4:
                    name = "e";
                    break;
                case 5:
                    name = "f";
                    break;
                case 6:
                    name = "fis";
                    break;
                case 7:
                    name = "g";
                    break;
                case 8:
                    name = "gis";
                    break;
                case 9:
                    name = "a";
                    break;
                case 10:
                    name = "ais";
                    break;
                case 11:
                    name = "b";
                    break;
            }

            int distance = midiKey - previousMidiKey;
            while (distance < -6)
            {
                name += ",";
                distance += 8;
            }

            while (distance > 6)
            {
                name += "'";
                distance -= 8;
            }

            return name;
        }
    }
}
