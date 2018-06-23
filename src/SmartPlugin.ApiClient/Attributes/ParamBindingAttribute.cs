using System;
using System.Collections.Generic;
using System.Text;
using SmartPlugin.ApiClient.Enums;
using SmartPlugin.ApiClient.Extensions;

namespace SmartPlugin.ApiClient.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class ParamBindingAttribute: Attribute, IAttribute
    {
        public string Name { get; set; }
        public BindingSource Binding { get; private set; }

        public ParamBindingAttribute(string name, BindingSource binding)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException($"The paramater '{nameof(name)}' cannot be null.");
            Name = name;
            Binding= binding;
        }

        public string Tag => $"{this.AttributeName()}(\"{Name.Trim()}\", {nameof(BindingSource)}.{Binding.GetName()})"; //Enum.GetName(typeof(BindingSource), Binding)
    }
}
