using System;
using SmartPlugin.ApiClient.Enums;

namespace SmartPlugin.ApiClient.Model
{
    public sealed class Parameter
    {
        private Parameter() { }

        public Parameter(string name, BindingSource binding, object value, Type type)
        {
            Name = name;
            Binding = binding;
            Value = value;
            ParameterType = type;
        }

        public string Name { get; set; }
        public BindingSource Binding { get; set; }
        public Object Value { get; set; }
        public Type ParameterType { get; set; }
    }
}
