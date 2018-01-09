using Core.Commands;
using Core.Editor;
using Core.Memento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Commands
{
    [CommandBinding(Name = Editor.Commands.OpenFileCommandName)]
    class SaveToPdfCommand : ICommand
    {
        public bool CanInvoke<T>(CareTaker<T> careTaker)
        {
            throw new NotImplementedException();
        }

        public void Invoke<T>(CareTaker<T> careTaker)
        {
            throw new NotImplementedException();
        }
    }
}
