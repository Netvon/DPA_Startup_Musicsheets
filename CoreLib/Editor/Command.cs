using Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Editor
{
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class CommandAttribute : Attribute
    {
        // This is a positional argument
        public CommandAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }

    public abstract class Command : ICommand
    {
        State previousState;

        virtual public void Invoke(ref State state)
        {
            previousState = state;
        }

        public void Undo(ref State state)
        {
            state.SetBack(previousState);
        }
    }
}
