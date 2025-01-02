using System;

namespace ED.PrefsDataManagement.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class PrefsDataPropertyAttribute : System.Attribute
    {
        public string Name { get; }
        public PrefsDataPropertyAttribute() => Name = null;
        public PrefsDataPropertyAttribute(string name) => Name = name;
    }
}