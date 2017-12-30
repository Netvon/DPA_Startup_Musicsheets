using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using Core.Builder.Internal.Xml;
using Core.Models;

namespace Core.Builder.Internal
{
    public class XMLSheetBuilder : SheetBuilder
    {
        XPathNavigator navigator;
        readonly IEnumerable<IXmlElementHandler> handlers;

        public XMLSheetBuilder()
        {
            handlers = Util.Reflection.GetInstancesOf<IXmlElementHandler>();
        }

        public void AddXPathNavigator(XPathNavigator nav)
        {
            navigator = nav;
        }

        public override Sheet Build()
        {
            foreach (var handler in handlers)
            {
                if (string.IsNullOrWhiteSpace(handler.XPathSelector))
                    continue;

                var nav = navigator.Select(handler.XPathSelector);

                if (nav.Count >= 1) {
                    nav.MoveNext();
                    handler.Handle(nav, this);
                }
            }

            return sheet;
        }
    }
}
