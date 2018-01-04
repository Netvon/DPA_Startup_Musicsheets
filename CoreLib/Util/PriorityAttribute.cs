using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Util
{
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class PriorityAttribute : Attribute
    {
        int priority;

        public PriorityAttribute(int priority)
        {
            this.priority = priority;
        }

        public int Priority => priority;
    }
}
