using SmartPlugin.ApiClient.Extensions;
using System;

namespace SmartPlugin.ApiClient.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class RouteAttribute : Attribute, IAttribute
    {
        public string Path { get; private set; }
        public RouteAttribute(string path)
        {
            Path = path;
        }

        public string Tag => $"{this.AttributeName()}(\"{Path.Trim()}\")";
    }
}
