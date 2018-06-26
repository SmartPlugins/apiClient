using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SmartPlugin.ApiClient
{
    public sealed class Configuration
    {
        [JsonProperty("ApiClient")]
        public ApiClient ClientSettings { get; set; }

        private static Lazy<ApiClient> _settings;

        public static ApiClient Settings
        {
            get
            {
                if (_settings == null)
                    _settings = new Lazy<ApiClient>(() => Load());
                return _settings.Value;
            }
        }

        private static ApiClient Load(string fileName = "EACH.ApiClient.json")
        {
            using (StreamReader r = new StreamReader(fileName))
            {
                var json = r.ReadToEnd();
                var config = JsonConvert.DeserializeObject<Configuration>(json);
                return config.ClientSettings;
            }
        }
    }
    public sealed class ApiClient
    {
        internal ApiClient()
        {
            DefaultRoute = "api/[client]";
        }
        [JsonProperty("BaseUrl")]
        public string BaseUrl { get; internal set; }

        [JsonProperty("WindowsAuthentication")]
        public bool WindowsAuthentication { get; internal set; } = true;

        [JsonProperty("ClientAppCode")]
        public string ClientAppCode { get; internal set; }
        [JsonProperty("DefaultRoute")]
        public string DefaultRoute { get; internal set; }
        [JsonProperty("Routes")]
        public Route[] Routes { get; internal  set; }
    }

    public sealed class Route
    {
        [JsonProperty("Client")]
        internal string Client { get; set; }
        [JsonProperty("Path")]
        internal string Path { get; set; }
    }
}
