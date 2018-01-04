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

            foreach (var note in notes)
            {
                


            }

            builder.AddBar(notes);
        }
    }
}
