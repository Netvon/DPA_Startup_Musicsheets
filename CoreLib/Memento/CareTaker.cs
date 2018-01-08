using Core.Extensions;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Memento
{
    public abstract class CareTaker<TMemento>
    {
        readonly LinkedList<TMemento> mementoList;
        LinkedListNode<TMemento> currentMemento;

        public CareTaker()
        {
            mementoList = new LinkedList<TMemento>();
        }

        public void Save(TMemento memento)
        {
            if (currentMemento == null)
            {
                mementoList.AddFirst(memento);
                currentMemento = mementoList.First;
            }
            else if (!memento.Equals(currentMemento.Value))
            {
                currentMemento = currentMemento.ReplaceNext(memento);
            }
        }

        public TMemento Redo()
        {
            if (!CanUndo)
                throw new IndexOutOfRangeException();
            currentMemento = currentMemento.Previous;
            return currentMemento.Value;
        }

        public TMemento Undo()
        {
            if (!CanRedo)
                throw new IndexOutOfRangeException();
            currentMemento = currentMemento.Next;
            return currentMemento.Value;
        }

        public bool CanUndo => currentMemento?.Previous != null;

        public bool CanRedo => currentMemento?.Next != null;

        public TMemento Current
        {
            get {
                if (currentMemento == null)
                    return default(TMemento);

                return currentMemento.Value;
            }
        }
    }
}
