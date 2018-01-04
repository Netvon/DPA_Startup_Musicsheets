using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Core.Builder.Internal.Xml
{
    class MeasureHandler : IXmlElementHandler
    {
        public string XPathSelector => "/score-partwise/part[1]/measure";

        public void Handle(XPathNodeIterator it, SheetBuilder builder)
        {
            var xmlNotes = it.Current.Select("note");
            List<MSNote> notes = new List<MSNote>();

            int i = 0;
            while (i < xmlNotes.Count)
            {
                // creating new Note
                var noteBuilder = builder.GetNoteBuilder();

                xmlNotes.MoveNext();

                var rest = xmlNotes.Current.SelectSingleNode("rest");
                if (rest != null && rest.Name.Equals("rest")) { noteBuilder.AddRest(); }
                var pitchOveral = xmlNotes.Current.SelectSingleNode("pitch");
                if (pitchOveral != null && pitchOveral.Name.Equals("pitch"))
                {
                    var pitch = xmlNotes.Current.SelectSingleNode("pitch/step").Value;
                    var octave = xmlNotes.Current.SelectSingleNode("pitch/octave").Value;

                    noteBuilder.AddPitch((NotePitch)Enum.Parse(typeof(NotePitch), pitch));
                    noteBuilder.AddOctave(int.Parse(octave));
                }
                var duration = xmlNotes.Current.SelectSingleNode("duration");
                //if (duration != null && duration.Name.Equals("duration")) { noteBuilder.AddBaseLength(); }




                i++;
            }

            builder.AddBar(notes);
        }
    }
}
