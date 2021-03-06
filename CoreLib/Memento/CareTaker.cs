﻿using Core.Extensions;
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
        public event EventHandler<TMemento> MementoChanged;

        readonly LinkedList<TMemento> mementoList;
        LinkedListNode<TMemento> currentSheet;

        public CareTaker()
        {
            mementoList = new LinkedList<TMemento>();
        }

        public void Save(TMemento memento)
        {
            if (currentSheet == null)
            {
                mementoList.AddFirst(memento);
                currentSheet = mementoList.First;

                MementoChanged?.Invoke(this, currentSheet.Value);
            }
            else if (!memento.Equals(currentSheet.Value))
            {
                currentSheet = currentSheet.ReplaceNext(memento);

                MementoChanged?.Invoke(this, currentSheet.Value);
            }
        }

        public TMemento Redo()
        {
            if (!CanRedo)
                throw new IndexOutOfRangeException();
            currentSheet = currentSheet.Next;
            return currentSheet.Value;
        }

        public TMemento Undo()
        {
            if (!CanUndo)
                throw new IndexOutOfRangeException();
            currentSheet = currentSheet.Previous;
            return currentSheet.Value;
        }

        public bool CanUndo => currentSheet?.Previous != null;

        public bool CanRedo => currentSheet?.Next != null;

        public TMemento Current
        {
            get {
                if (currentSheet == null)
                    return default(TMemento);

                return currentSheet.Value;
            }
        }

        public bool HasChanges => mementoList.Count > 1;
    }
}
