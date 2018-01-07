using Core.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Commands
{
    public interface ICommand
    {
        void Invoke(ref State state);
        void Undo(ref State state);
    }
}
