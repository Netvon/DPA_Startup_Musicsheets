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

                    var modifier = xmlNotes.Current.SelectSingleNode("pitch/alter");

                    if (modifier != null)
                    {
                        switch (int.Parse(modifier.Value))
                        {
                            case 1:
                                noteBuilder.AddModifier(NoteModifier.Sharp);
                                break;
                            case -1:
                                noteBuilder.AddModifier(NoteModifier.Flat);
                                break;
                            default:
                                noteBuilder.AddModifier(NoteModifier.None);
                                break;
                        }
                    } else
                    {
                        noteBuilder.AddModifier(NoteModifier.None);
                    }
                }

                var type = xmlNotes.Current.SelectSingleNode("type");
                if (type != null && type.Name.Equals("type"))
                {
                    // Bah switch statement 
                    // Veranderen + even overleggen met Tom
                    switch (type.Value.ToLower())
                    {
                        case "whole":
                            noteBuilder.AddBaseLength(1);
                            break;
                        case "half":
                            noteBuilder.AddBaseLength(2);
                            break;
                        case "quarter":
                            noteBuilder.AddBaseLength(4);
                            break;
                        case "eighth":
                            noteBuilder.AddBaseLength(8);
                            break;
                        default:
                            break;
                    }
                }

                notes.Add(noteBuilder.Build());
                i++;
            }

            builder.AddBar(notes);
        }
    }
}
