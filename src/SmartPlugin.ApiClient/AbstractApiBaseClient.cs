using Newtonsoft.Json;
using SmartPlugin.ApiClient.Enums;
using SmartPlugin.ApiClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartPlugin.ApiClient
{
    public abstract class AbstractApiBaseClient
    {
        public string BaseUrl { get; private set; }
        protected Dictionary<string, string> ApiHeaders = new Dictionary<string, string>();
        protected string BaseRoute { get; private set; }
        protected ApiClient Config { get; set; }

        #region Initialization
       protected abstract  void InitializeClient();

        private void ReadConfig()
        {
            Config = Configuration.Settings;
            BaseUrl = Config.BaseUrl;

            var client = this.GetType().Name.Replace("Client", string.Empty);
            if (Config.Routes.Length > 0 && Config.Routes.Any(r => r.Client.Equals(client, StringComparison.OrdinalIgnoreCase)))
                BaseRoute = Config.Routes.First(r => r.Client.Equals(client, StringComparison.OrdinalIgnoreCase)).Path;
            else
                BaseRoute = Config.DefaultRoute.Replace("[client]", client);

            ApiHeaders.Add("appCode", Config.ClientAppCode);

            //var userName = $@"{Environment.UserDomainName}\{Environment.UserName}";
            //ApiHeaders.Add("clientUser", userName);
        }
        #endregion

        #region Constructors
        protected AbstractApiBaseClient()
        {
            ReadConfig();
            InitializeClient();
        }
        protected AbstractApiBaseClient(string route)
        {
            ReadConfig();
            BaseRoute = route;
            InitializeClient();
        }

        protected AbstractApiBaseClient(string baseUrl, string route)
        {
            BaseUrl = baseUrl;
            BaseRoute = route;
            InitializeClient();
        }
        #endregion

        protected StringBuilder GetRequestUrl(string actionTemplate) =>
            new StringBuilder().Append(BaseUrl?.TrimEnd('/') ?? string.Empty)
                .Append($"/{BaseRoute?.TrimStart('/').TrimEnd('/')}")
                .Append($"/{actionTemplate?.TrimStart('/')}");
        // $"{BaseUrl?.TrimEnd('/')??string.Empty}/{BaseRoute?.TrimStart('/').TrimEnd('/')}/{actionTemplate?.TrimStart('/')}"

        protected StringBuilder GetRequestUrl(string actionTemplate, Parameters parameters)
        {
            var url = GetRequestUrl(actionTemplate);
            if (parameters?.ContainsKey(BindingSource.Path) ?? false)
                parameters[BindingSource.Path].ForEach(p =>
                {
                    url.Replace($"{{{p.Name}}}", EscapeData(p.Value.ToString()));
                });
            if (parameters?.ContainsKey(BindingSource.Query) ?? false)
            {
                var qryString = parameters[BindingSource.Query].Select(p => $"{p.Name}={EscapeData(p.Value.ToString())}").Aggregate((c, n) => $"{c}&{n}");
                url.Append($"?{qryString}");
            }
            return url;
        }

        protected string EscapeData(string data)
            => System.Uri.EscapeDataString(ConvertToString(data, System.Globalization.CultureInfo.InvariantCulture));

        protected string ConvertToString(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value is System.Enum)
            {
                string name = System.Enum.GetName(value.GetType(), value);
                if (name != null)
                {
                    var field = System.Reflection.IntrospectionExtensions.GetTypeInfo(value.GetType()).GetDeclaredField(name);
                    if (field != null)
                    {
                        var attribute = System.Reflection.CustomAttributeExtensions.GetCustomAttribute(field, typeof(System.Runtime.Serialization.EnumMemberAttribute))
                            as System.Runtime.Serialization.EnumMemberAttribute;
                        if (attribute != null)
                        {
                            return attribute.Value;
                        }
                    }
                }
            }
            else if (value is byte[])
            {
                return System.Convert.ToBase64String((byte[])value);
            }
            else if (value.GetType().IsArray)
            {
                var array = System.Linq.Enumerable.OfType<object>((System.Array)value);
                return string.Join(",", System.Linq.Enumerable.Select(array, o => ConvertToString(o, cultureInfo)));
            }

            return System.Convert.ToString(value, cultureInfo);
        }

        protected abstract Task<T> ExecuteAsync<T>(Parameters parameters,
            CancellationToken cancellationToken = default(CancellationToken));

        protected abstract Task<T> ExecuteAsync<T>(HttpVerb action, string actionTemplate, Parameters parameters,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
