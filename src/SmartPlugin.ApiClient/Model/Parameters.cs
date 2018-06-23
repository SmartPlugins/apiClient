using System.Collections.Generic;
using System.Linq;
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
                if (p != null)
                    AddParameter(p, p.Binding);
            });
        }

        public void AddParameter<T>(string name, T value, BindingSource binding)
        {
            if (!base.ContainsKey(binding))
                base[binding] = new List<Parameter>();

            if (!EqualityComparer<T>.Default.Equals(default(T), value))
                base[binding].Add(new Parameter(name, binding, value, typeof(T)));
        }

        public void AddParameter(Parameter parameter, BindingSource binding = BindingSource.Path)
        {
            if (!base.ContainsKey(binding))
                base[binding] = new List<Parameter>();

            if (parameter.Value != default)
                base[binding].Add(parameter);
        }
    }
}
