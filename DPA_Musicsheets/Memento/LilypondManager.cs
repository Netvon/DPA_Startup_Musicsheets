using Core.Memento;
using Core.Models;
using DPA_Musicsheets.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Memento
{
    class LilypondManager : IMemento<Sheet>
    {
        private readonly LinkedList<Sheet> sheetList;
        private LinkedListNode<Sheet> currentSheet;

        public LilypondManager()
        {
            sheetList = new LinkedList<Sheet>();
        }

        public void Save(Sheet memento)
        {
            if (currentSheet == null)
            {
                sheetList.AddFirst(memento);
                currentSheet = sheetList.First;
            }
            else if (memento != currentSheet.Value)
            {
                currentSheet = currentSheet.ReplaceNext(memento);
            }
        }
        public Sheet Redo()
        {
            if (!CanUndo)
                throw new IndexOutOfRangeException();
            currentSheet = currentSheet.Previous;
            return currentSheet.Value;
        }
        public Sheet Undo()
        {
            if (!CanRedo)
                throw new IndexOutOfRangeException();
            currentSheet = currentSheet.Next;
            return currentSheet.Value;
        }

        public bool CanUndo => currentSheet?.Previous != null;

        public bool CanRedo => currentSheet?.Next != null;

    }
}
