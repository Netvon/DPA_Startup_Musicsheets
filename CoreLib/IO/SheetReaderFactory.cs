using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Core.IO
{
    public class SheetReaderFactory
    {
        /// <summary>
        /// Source Assemblies. These assemblies will be used to reslove all implementations of
        /// the ISheetReader interface
        /// </summary>
        readonly List<Assembly> lookupAssemblies;

        public SheetReaderFactory() : this(null) { }

        public SheetReaderFactory(Assembly assembly)
        {
            lookupAssemblies = new List<Assembly>()
            {
                assembly ?? typeof(SheetReaderFactory).GetTypeInfo().Assembly
            };
        }

        /// <summary>
        /// Adds a new Assembly to the lookup list. This list will be used to reslove all implementations of
        /// the ISheetReader interface.
        /// </summary>
        /// <param name="lookup"></param>
        public void AddAssembly(Assembly lookup)
        {
            lookupAssemblies.Add(lookup);
        }

        /// <summary>
        /// Created a new instance of the ISheetReader interface.
        /// Call ReadFromFileAsync on this instance to load the Sheet.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public ISheetReader GetReader(string filePath)
        {
            // get the extension of the file
            var ext = Path.GetExtension(filePath).ToLowerInvariant();

            // get the correct reader instance
            // the different implementations for this project can be found in the "/IO/Internal" folder
            return GetSheetReaderInstance(filePath, ext);
        }

        private ISheetReader GetSheetReaderInstance(string filePath, string ext)
        {
            // Take all the Assemblies that are in the lookupAssemblies list
            // for each Assembly, find all defined types within
            var types = lookupAssemblies.SelectMany(x => x.DefinedTypes);
            // of those types, select all that implement the ISheetReader interface
            var readersTypes = types.Where(t => t.ImplementedInterfaces.Contains(typeof(ISheetReader)));

            // loop over all the Types that we found in the previous step
            foreach (var readerType in readersTypes)
            {
                // get the SheetReaderAttribute from this type
                var attr = readerType.GetCustomAttribute<SheetReaderAttribute>();

                // check if this reader type could handle the file we are trying to load
                // if the SheetReaderAttribute was not found this will also equal false
                if (attr?.MatchesExtension(ext) == true)
                {
                    // if this reader type can read the file create a new instance of it
                    // afterwards we give the reader the path to read
                    var instance = Activator.CreateInstance(readerType.AsType()) as ISheetReader;
                    instance.SetFilePath(filePath);

                    return instance;
                }
            }

            // if we were not able to find any reader types that are able to read this file, return null
            return null;
        }

        public IEnumerable<(string name, string ext)> GetAllSupportedExtension()
        {
            // Take all the Assemblies that are in the lookupAssemblies list
            // for each Assembly, find all defined types within
            var types = lookupAssemblies.SelectMany(x => x.DefinedTypes);
            // of those types, select all that implement the ISheetReader interface
            var readersTypes = types.Where(t => t.ImplementedInterfaces.Contains(typeof(ISheetReader)));

            // loop over all the Types that we found in the previous step
            foreach (var readerType in readersTypes)
            {
                // get the SheetReaderAttribute from this type
                var attr = readerType.GetCustomAttribute<SheetReaderAttribute>();

                yield return (attr.Name, attr.HandleExtension);
            }
        }
    }
}
