using Core.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class RestNote : MSNote
    {
        public override string ToString()
        {
            return "Rest";
        }
        public override void Visit(IVisitor visitor)
        {
            visitor.Accept(this);
        }
    }
}
