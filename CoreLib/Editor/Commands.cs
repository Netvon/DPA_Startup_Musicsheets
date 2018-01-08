using Core.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core.Util;
using Core.Memento;

namespace Core.Editor
{
    public class Commands
    {
        Dictionary<string, ICommand> nameBindings;
        List<KeyValuePair<string, KeyBind>> keyBindings;

        ICommand last;

        public Commands(Assembly assembly)
        {
            nameBindings = All(assembly);
            keyBindings = new List<KeyValuePair<string, KeyBind>>();
        }

        public void AddBinding(string name, string pattern)
        {
            keyBindings.Add(new KeyValuePair<string, KeyBind>(name, new KeyBind(pattern)));
        }

        public bool Handle<T>(string key, CareTaker<T> careTaker)
        {
            bool didCommand = false;

            foreach (var keyBinding in keyBindings)
            {
                if(keyBinding.Value.Match(key))
                {
                    last = nameBindings[keyBinding.Key];

                    if (!keyBindings.Any(x => x.Value.HasPartialMatch) && last?.CanInvoke(careTaker) == true)
                    {
                        last?.Invoke(careTaker);
                        Reset();
                    }
                    didCommand = true;
                }
            }

            return didCommand;
        }

        public bool HasLastCommand => last != null;

        public void InvokeLast<T>(CareTaker<T> careTaker)
        {
            if (last?.CanInvoke(careTaker) == true)
            {
                last.Invoke(careTaker);
                Reset();
            }
        }

        public void Reset()
        {
            foreach (var keyBinding in keyBindings) { keyBinding.Value.Reset(); }
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
