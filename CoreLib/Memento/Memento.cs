using Core.Extensions;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Memento
{
    public class Memento<T>
    {
        private readonly LinkedList<T> sheetList;
        private LinkedListNode<T> currentSheet;

        public Memento()
        {
            sheetList = new LinkedList<T>();
        }

        public void Save(T memento)
        {
            if (currentSheet == null)
            {
                sheetList.AddFirst(memento);
                currentSheet = sheetList.First;
            }
            else if (!memento.Equals(currentSheet.Value))
            {
                currentSheet = currentSheet.ReplaceNext(memento);
            }
        }

        public T Redo()
        {
            if (!CanUndo)
                throw new IndexOutOfRangeException();
            currentSheet = currentSheet.Previous;
            return currentSheet.Value;
        }

        public T Undo()
        {
            if (!CanRedo)
                throw new IndexOutOfRangeException();
            currentSheet = currentSheet.Next;
            return currentSheet.Value;
        }

        public bool CanUndo => currentSheet?.Previous != null;

        public bool CanRedo => currentSheet?.Next != null;

        public T Current => currentSheet.Value;
    }
}
