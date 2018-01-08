using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Memento
{
    public interface IMemento<T>
    {

        void Save(T memento);
        Sheet Redo();
        Sheet Undo();

        bool CanUndo { get; }
        bool CanRedo { get; }
    }
}
