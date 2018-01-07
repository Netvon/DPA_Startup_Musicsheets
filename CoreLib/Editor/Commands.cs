using Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Editor
{
    public static class Commands
    {
        static public IEnumerable<ICommand> All()
        {
            return Util.Reflection.GetInstancesOf<ICommand>();
        }
    }
}
