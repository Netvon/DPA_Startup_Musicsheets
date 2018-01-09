using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.IO
{
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class SheetWriterAttribute : Attribute
    {
        public SheetWriterAttribute(string handleExtension, string name)
        {
            HandleExtension = handleExtension;
            Name = name;
        }

        public string HandleExtension { get; }
        public string Name { get; }

        public bool MatchesExtension(string extension)
        {
            return Regex.IsMatch(extension, HandleExtension, RegexOptions.IgnoreCase);
        }
    }
}
