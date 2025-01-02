using System;

namespace ED.DataManagement.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class DataPropertyNameAttribute : System.Attribute
    {
        public string Name { get; }
        public DataPropertyNameAttribute(string name) => Name = name;
    }
}