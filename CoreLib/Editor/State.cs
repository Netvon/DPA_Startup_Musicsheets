using Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Editor
{
    public class State
    {
        public string Text { get; set; }

        readonly IEnumerable<ICommand> commands;
        public IEnumerable<ICommand> Commands => commands;

        public State()
        {
            commands = Util.Reflection.GetInstancesOf<ICommand>();
        }

        public void SetBack(State old)
        {
            Text = old.Text;
        }
    }
}
