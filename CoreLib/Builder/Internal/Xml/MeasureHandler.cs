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
            Dictionary<string, uint> noteLengths = new Dictionary<string, uint>()
            {
                { "whole", 1 },
                { "half", 2 },
                { "quarter", 4 },
                { "eighth", 8 }
            };

            Dictionary<int, NoteModifier> modifiers = new Dictionary<int, NoteModifier>()
            {
                { 1, NoteModifier.Sharp },
                { -1, NoteModifier.Flat },
            };


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

                    var modifier = xmlNotes.Current.SelectSingleNode("pitch/alter");
                    if (modifier != null && modifier.Name.Equals("modifier"))
                    {
                        if (modifiers.ContainsKey(int.Parse(modifier.Value)))
                        {
                            noteBuilder.AddModifier(modifiers[int.Parse(modifier.Value)]);
                        }
                        else
                        {
                            noteBuilder.AddModifier(NoteModifier.None);
                        }
                    }
                }

                var type = xmlNotes.Current.SelectSingleNode("type");
                if (type != null && type.Name.Equals("type"))
                {
                    if (noteLengths.ContainsKey(type.Value))
                    {
                        noteBuilder.AddBaseLength(noteLengths[type.Value]);
                    }
                }

                var dot = xmlNotes.Current.SelectSingleNode("dot");
                if (dot != null && dot.Name.Equals("dot")) { noteBuilder.AddDotts(1); }

                notes.Add(noteBuilder.Build());
                i++;
            }

            builder.AddBar(notes);
        }
    }
}
