using System;

namespace Core.Editor
{
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class CommandBindingAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
