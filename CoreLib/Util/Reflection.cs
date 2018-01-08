using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Util
{
    public static class Reflection
    {
        static Dictionary<string, List<object>> instances = new Dictionary<string, List<object>>();

        public static IEnumerable<T> GetInstancesOf<T>(Func<TypeInfo, bool> predicate = null, Assembly useAssembly = null)
        {
            // check if type paramenter T is an interface
            if (!typeof(T).IsInterface)
                throw new ArgumentException("T must be an interface");

            // get the name of the inferface
            var interfaceName = GetTypeName<T>();

            // get the stored list of intances for this interface
            var instList = GetInstancesList<T>();
            if (instList != null) // if a list is present return it
                return instList;  // we don't need to create a new one

            // if no list was present we can create a new one

            // we can instuct the system to use a specif assembly to search for implementations
            // of T. If none was given use the current Assembly
            var asm = useAssembly ?? typeof(Reflection).Assembly;

            // create a new List of Instances and return the result
            return CreateInstancesList<T>(asm, predicate);
        }

        static string GetTypeName<T>()
        {
            return typeof(T).Name;
        }

        static IEnumerable<T> GetInstancesList<T>()
        {
            var interfaceName = GetTypeName<T>();
            var instList = instances.FirstOrDefault(i => i.Key == interfaceName).Value;

            return instList?.Select(t => (T)t);
        }

        public static IEnumerable<TypeInfo> GetInfo<TImplements>(Assembly assembly)
        {
            bool isInterface = typeof(TImplements).IsInterface;

            Func<TypeInfo, bool> match = (TypeInfo t) => t.ImplementedInterfaces.Contains(typeof(TImplements));

            if(!isInterface)
                match = (TypeInfo t) => t.BaseType == typeof(TImplements);

            var typeName = GetTypeName<TImplements>();

            return assembly.DefinedTypes.Where(match).Where(x => !x.IsAbstract);
        }

        static IEnumerable<T> CreateInstancesList<T>(Assembly useAssembly, Func<TypeInfo, bool> predicate = null)
        {
            if (predicate == null)
                predicate = (t) => true;

            var interfaceName = GetTypeName<T>();

            // find all the implementations of T in the given Assembly
            var types = useAssembly.DefinedTypes.Where(t => t.ImplementedInterfaces.Contains(typeof(T)) && !t.IsAbstract).Where(predicate);

            // create a new collection of instances using the Activator.
            // we cast the new instance to T to make sure everything stays generic
            var newInstances = types.Select(t => (T)Activator.CreateInstance(t));

            // create the List we will return to the User
            var newInstancesListGeneric = newInstances.ToList();
            // create the List we will store in our static dictionary
            var newInstancesList = newInstancesListGeneric.Select(t => (object)t).ToList();
            // add this new list to the dictionary
            instances.Add(interfaceName, newInstancesList);

            return newInstancesListGeneric;
        }

        public static IEnumerable<TInstance> Instanciate<TInstance>(this IEnumerable<TypeInfo> info)
        {
            return info.Select(t => (TInstance)Activator.CreateInstance(t));
        }
    }
}
