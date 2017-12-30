using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Core.Builder.Internal.Xml
{
    interface IXmlElementHandler
    {
        string XPathSelector { get; }
        void Handle(XPathNodeIterator it, SheetBuilder builder);
    }
}
