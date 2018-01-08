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
    [CommandBinding(Name = Editor.Commands.InsertTempoCommandName)]
    class InsertTempoCommand : ICommand
    {
        public bool CanInvoke<T>(CareTaker<T> careTaker) => true;

        public void Invoke<T>(CareTaker<T> careTaker)
        {
            if (careTaker is EditorCareTaker ect)
            {
                var clone = ect.Current?.Clone() as EditorMemento ?? new EditorMemento();

                clone.InsertText("\\tempo 4=120");
                ect.Save(clone);
            }
        }
    }
}
