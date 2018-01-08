using Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core.Util;
using System.Text.RegularExpressions;

namespace Core.Editor
{
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class CommandBindingAttribute : Attribute
    {
        public string Name { get; set; }
    }

    public class KeyBind
    {
        string[] keys;
        int matches;
        int matchesRequired;
        string pattern;

        public KeyBind(string pattern)
        {
            this.pattern = pattern;

            keys = pattern.Split(' ');
            matchesRequired = keys.Length;
        }

        public bool Match(string keyInput)
        {
            var key = keys[matches];

            if(Regex.Match(keyInput, key).Success)
            {
                matches++;

                if(matches == matchesRequired)
                {
                    Reset();
                    return true;
                }
            }

            return false;
        }

        public void Reset()
        {
            matches = 0;
        }
    }

    public class Commands
    {
        Dictionary<string, ICommand> nameBindings;
        Dictionary<string, KeyBind> keyBindings;

        public Commands(Assembly assembly)
        {
            nameBindings = All(assembly);
            keyBindings = new Dictionary<string, KeyBind>();
        }

        public void AddBinding(string name, string pattern)
        {
            keyBindings.Add(name, new KeyBind(pattern));
        }

        public bool Handle(string key)
        {
            bool didCommand = false;

            foreach (var keyBinding in keyBindings)
            {
                if(keyBinding.Value.Match(key))
                {
                    nameBindings[keyBinding.Key].Invoke();
                    didCommand = true;
                }
            }

            if (didCommand)
                foreach (var keyBinding in keyBindings) { keyBinding.Value.Reset(); }

            return didCommand;
        }

        public IEnumerable<string> CommandNames => nameBindings.Keys;

        public IDictionary<string, ICommand> NameBindings => nameBindings;

        static public Dictionary<string, ICommand> All(Assembly assembly)
        {
            var types = Reflection.GetInfo<ICommand>(assembly);

            var instances = types.Instanciate<ICommand>();

            return types
                .Select(t => (name: t.GetCustomAttribute<CommandBindingAttribute>()?.Name, typename: t.Name))
                .Select(x => (name: x.name, command: instances.FirstOrDefault(i => i.GetType().Name == x.typename)))
                .ToDictionary(kv => kv.name, kv => kv.command);
        }
    }
}
