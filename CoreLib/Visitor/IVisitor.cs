using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Visitor
{
    public interface IVisitor
    {
        void Accept(PitchNote pitchNote);
        void Accept(RestNote restNote);
    }
}
