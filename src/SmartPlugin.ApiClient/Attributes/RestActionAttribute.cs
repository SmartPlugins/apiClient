using System;
using System.Collections.Generic;
using System.Text;
using SmartPlugin.ApiClient.Enums;
using SmartPlugin.ApiClient.Extensions;

namespace SmartPlugin.ApiClient.Attributes
{
    /// <summary>
    /// Method level attribute to gather meta data for a ApiClient to process the request
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class RestActionAttribute:Attribute, IAttribute
    {
        public HttpVerb Action { get; private set; }
        public string Template { get; private set; }

        public RestActionAttribute(HttpVerb action)
        {
            Action = action;
        }
        public RestActionAttribute(HttpVerb action, string template)
        {
            Action = action;
            Template = template;
        }
        //Arrays in Attrib https://msdn.microsoft.com/en-us/library/system.reflection.customattributedata.namedarguments(v=vs.110).aspx
        public string Tag => $"{this.AttributeName()}({nameof(HttpVerb)}.{Enum.GetName(typeof(HttpVerb), Action)}{(!string.IsNullOrEmpty(Template) ? $", \"{Template.Trim()}\"" : string.Empty)})";
    }
}
