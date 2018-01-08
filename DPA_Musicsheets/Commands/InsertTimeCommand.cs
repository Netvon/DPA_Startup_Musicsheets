using Core.Commands;
using Core.Editor;
using Core.Memento;
using DPA_Musicsheets.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Commands
{
    abstract class InsertTimeCommand : ICommand
    {
        string time;
        public InsertTimeCommand(int uppertime, int lowertime)
        {
            time = $"{uppertime}/{lowertime}";
        }

        public bool CanInvoke<T>(CareTaker<T> careTaker) => true;

        public void Invoke<T>(CareTaker<T> careTaker)
        {
            if (careTaker is EditorCareTaker ect)
            {
                var clone = ect.Current.Clone() as EditorMemento;

                clone.InsertText($"\\time {time}");

                ect.Save(clone);
            }
        }
    }

    [CommandBinding(Name = Editor.Commands.InsertTime44CommandName)]
    class InsertTime44Command : InsertTimeCommand
    {
        public InsertTime44Command() : base(4, 4)
        {
        }
    }

    [CommandBinding(Name = Editor.Commands.InsertTime34CommandName)]
    class InsertTime34Command : InsertTimeCommand
    {
        public InsertTime34Command() : base(3, 4)
        {
        }
    }

    [CommandBinding(Name = Editor.Commands.InsertTime68CommandName)]
    class InsertTime68Command : InsertTimeCommand
    {
        public InsertTime68Command() : base(6, 8)
        {
        }
    }
}
