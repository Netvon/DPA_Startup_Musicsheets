using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.IO
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class SheetReaderAttribute : Attribute
    {
        // This is a positional argument
        public SheetReaderAttribute(string handleExtension, string name)
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
