using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using SmartPlugin.ApiClient.Enums;

namespace SmartPlugin.ApiClient.Model
{
    public sealed class Parameters : Dictionary<BindingSource, List<Parameter>>
    {
        public Parameters() { }

        public Parameters(params Parameter[] parameters)
        {
            parameters?.ToList().ForEach(p =>
            {
                AddParameter(p);
            });
        }

        private bool IsPrimitiveType<T>() =>
               IsPrimitiveType(typeof(T));

        private bool IsPrimitiveType(Type t) =>
            t.IsPrimitive || t.IsValueType || t == typeof(Decimal) || t == typeof(string);

        public void AddParameter<T>(string name, T value, BindingSource binding)
        {
            if (!base.ContainsKey(binding))
                base[binding] = new List<Parameter>();

            if ((binding == BindingSource.Path || binding == BindingSource.Query) && !IsPrimitiveType<T>())
                return;

            if (!EqualityComparer<T>.Default.Equals(default(T), value))
                base[binding].Add(new Parameter(name, binding, value, typeof(T)));
        }

        public void AddParameter(Parameter parameter)
        {
            if (parameter != default)
            {
                if (!base.ContainsKey(parameter.Binding))
                    base[parameter.Binding] = new List<Parameter>();

                if ((parameter.Binding == BindingSource.Path || parameter.Binding == BindingSource.Query) &&
                    !IsPrimitiveType(parameter.ParameterType))
                    return;
                if (parameter.Value != default)
                    base[parameter.Binding].Add(parameter);
            }
        }

        public HttpContent RequestContent { get; set; }
    }
}
