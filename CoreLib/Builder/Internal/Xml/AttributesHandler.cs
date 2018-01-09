using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Core.Builder.Internal.Xml
{
    class AttributesHandler : IXmlElementHandler
    {
        public string XPathSelector => "/score-partwise/part[1]/measure/attributes";

        public void Handle(XPathNodeIterator it, SheetBuilder builder)
        {
            var beats = it.Current.SelectSingleNode("time/beats");
            var beat_type = it.Current.SelectSingleNode("time/beat-type");
            var key = it.Current.SelectSingleNode("clef/sign");

            var parsedKey = (SheetKey)Enum.Parse(typeof(SheetKey), key.Value);

            builder.AddKey(parsedKey);
            builder.AddTimeSignature((uint)beats.ValueAsInt, (uint)beat_type.ValueAsInt);
            builder.AddTempto(120);
        }
    }
}
