using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Core.IO
{
    public class MusicReaderFactory
    {
        readonly List<Assembly> lookupAssemblies;

        public MusicReaderFactory() : this(null) { }

        public MusicReaderFactory(Assembly assembly)
        {
            lookupAssemblies = new List<Assembly>()
            {
                assembly ?? typeof(MusicReaderFactory).GetTypeInfo().Assembly
            };
        }

        public void AddAssembly(Assembly lookup)
        {
            lookupAssemblies.Add(lookup);
        }

        public IMusicReader GetReader(string filePath)
        {
            var ext = Path.GetExtension(filePath).ToLowerInvariant();

            var types = lookupAssemblies.SelectMany(x => x.DefinedTypes);
            var readers = types.Where(t => t.ImplementedInterfaces.Contains(typeof(IMusicReader)));

            foreach (var reader in readers)
            {
                var attr = reader.GetCustomAttribute<MusicReaderAttribute>();

                if (attr?.MatchesExtension(ext) == true)
                    return Activator.CreateInstance(reader.AsType()) as IMusicReader;

            }

            return null;
        }
    }
}
