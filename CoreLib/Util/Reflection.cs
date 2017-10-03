using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Util
{
    class Reflection
    {
        public static IEnumerable<T> GetInstancesOf<T>(Assembly useAssembly = null)
        {
            if (!typeof(T).IsInterface)
                throw new ArgumentException("T must be an interface");

            var asm = useAssembly ?? typeof(Reflection).Assembly;

            var types = asm.DefinedTypes.Where(t => t.ImplementedInterfaces.Contains(typeof(T)));

            return types.Select(t => (T)Activator.CreateInstance(t));
        }
    }
}
