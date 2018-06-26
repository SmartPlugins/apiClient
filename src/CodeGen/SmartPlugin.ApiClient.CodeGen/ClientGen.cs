using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;

namespace SmartPlugin.ApiClient.CodeGen
{

    /*
     *Example : http://petstore.swagger.io/
     */
    public sealed class ClientGen
    {
        private Settings _settings;
        private OpenApiDocument _apiSpecDoc;

        internal ClientGen(Settings settings)
        {
            _settings = settings;
        }

        public void Generate()
        {

            using (Stream specStream = (!string.IsNullOrEmpty(_settings.ApiSpecPath) ?
                                            (new FileStream(_settings.ApiSpecPath, FileMode.Open, FileAccess.Read)) :
                                            (WebRequest.Create(_settings.ApiSpecUri).GetResponse().GetResponseStream())))
            {
                _apiSpecDoc = new OpenApiStreamReader().Read(specStream, out var diagnostics);

                if (diagnostics != null && (diagnostics.Errors?.Count ?? 0) > 0)
                {
                    Console.WriteLine("Following are the error(s) reading ApiSecp:");
                    diagnostics.Errors.ToList().ForEach(e => Console.WriteLine(e));
                    return;
                }
                Console.WriteLine("*****************************************************************************************************");
                Console.WriteLine("Generating client files");
                Console.WriteLine("*****************************************************************************************************");
                Console.WriteLine($"Parsing Api operations for : '{_apiSpecDoc.Info.Title} (version:{_apiSpecDoc.Info.Version})'");
                Console.WriteLine($"- {_apiSpecDoc.Info.Description}");
                Console.WriteLine("*****************************************************************************************************");

                // var clients= _apiSpecDoc.Paths.Keys.Select(p => p.Substring(1,p.IndexOf('/',1) )).Distinct();
                Dictionary<Client, List<Operation>> clients= new Dictionary<Client, List<Operation>> ();

                _apiSpecDoc.Paths.ToList().ForEach(p =>
                {
                    var clientName = p.Key.Substring(1, (p.Key.IndexOf('/', 1)==-1?p.Key.Length: p.Key.IndexOf('/', 1))-1);
                    Client client=clients?.Keys?.FirstOrDefault(c => c.Name == clientName);
                      client = clients?.Keys?.FirstOrDefault(c => c.Name == clientName) ?? new Client(_apiSpecDoc.Tags.FirstOrDefault(c => c.Name == clientName))
                      {
                          //RouteTemplate = 
                      };

                });

                _apiSpecDoc.Tags.ToList().ForEach(t =>
                {
                    Console.WriteLine($"Generating operations for the client '{t.Name}': {t.Description}");

                });
            }
        }
    }

    public class Client:OpenApiTag
    {
        public Client()
        {
        }

        public Client(OpenApiTag tag)
        {
            Name = tag.Name;
            Description = tag.Description;
            Extensions = tag.Extensions;
            ExternalDocs = tag.ExternalDocs;
            Reference = tag.Reference;
            UnresolvedReference = tag.UnresolvedReference;
        }

        public string RouteTemplate { get; set; }
        public string Comments { get; set; }
    }

    public class Operation : OpenApiOperation
    {
        public string ActionTemplate { get; set; }
        public string Comments { get; set; }
    }
}
