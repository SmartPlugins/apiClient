using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using SmartPlugin.ApiClient.Extensions;

namespace SmartPlugin.ApiClient.Attributes
{
   public sealed class ParamBindings:List<ParamBindingAttribute>, IAttribute
    {
        public ParamBindings() { }

        public ParamBindings(List<ParamBindingAttribute> bindings)
        {
            base.AddRange(bindings);
        }

        public string Tag => this?.Select(pb => pb.Tag).Aggregate((c, n) => $"{c}, {n}");
        public string Tags => this?.Select(pb => pb.TagEnclosed()).Aggregate((c, n) => $"{c}\n{n}");

        public static ParamBindings LoadFrom(ParamBindingAttribute[] bindings) => new ParamBindings(bindings?.ToList());

        public static ParamBindings LoadFrom(IEnumerable<ParamBindingAttribute> bindings) => LoadFrom(bindings?.ToArray());

        public static ParamBindings LoadFrom(AttributeCollection collection) => LoadFrom(collection?.OfType<ParamBindingAttribute>());

        public void AddParamBinding(ParamBindingAttribute paramBinding)
        {
            if (!this.Any(p => p.Name.Equals(paramBinding)))
                this.Add(paramBinding);
        }
    }
}
