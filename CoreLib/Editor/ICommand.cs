using Core.Editor;
using Core.Memento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Commands
{
    public interface ICommand
    {
        void Invoke<T>(IMemento<T> memento);
        bool CanInvoke();
    }
}
