using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Core.Builder.Internal.Xml
{
    class NameHandler : IXmlElementHandler
    {
        public string XPathSelector => "/score-partwise/work/work-title";

        public void Handle(XPathNodeIterator it, SheetBuilder builder)
        {
            if (!string.IsNullOrWhiteSpace(it.Current.Value))
            {
                builder.AddName(it.Current.Value);
            }
        }
    }
}
