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
        public string XPathSelector => throw new NotImplementedException();

        public void Handle(XPathNodeIterator it, SheetBuilder builder)
        {
            throw new NotImplementedException();
        }
    }
}
