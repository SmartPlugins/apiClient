using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;
using NSwag;
using NSwag.CodeGeneration;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.CSharp.Models;
using NSwag.CodeGeneration.Models;
using NSwag.CodeGeneration.OperationNameGenerators;

namespace SmartPlugin.ApiClient.CodeGen
{

    /*
     *Example : http://petstore.swagger.io/
     */
    public sealed class ClientGen
    {
        /// <summary>
        /// Gets the document.
        /// </summary>
        /// <value>
        /// The document.
        /// </value>
        public SwaggerDocument Document { get; private set; }

        private readonly Settings _settings;
        private ILanguageCodeGen _langCodeGen;

        private ClientGeneratorBaseSettings _langCodeGenSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientGen"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        internal ClientGen(Settings settings)
        {
            _settings = settings;
            FetchSwaggerDocument();
            DefineGeneratorSettings();
            DefineCodeGenerator();
        }

        /// <summary>
        /// Defines the generator settings.
        /// </summary>
        private void DefineGeneratorSettings()
        {
            if (_settings.LanguageCode == Language.CSharp)
            {
                _langCodeGenSettings = new SwaggerToCSharpClientGeneratorSettings()
                {
                    CSharpGeneratorSettings =
                    {
                        Namespace = _settings.ClientNamespace,
                        TemplateFactory = new Templates.DefaultTemplateFactory(new CSharpGeneratorSettings (){
                                                                    Namespace = _settings.ClientNamespace,
                                                                    SchemaType = SchemaType.Swagger2}, _settings.TemplatesAssemblyName ,  new[]
                        {
                            typeof(CSharpGeneratorSettings).GetTypeInfo().Assembly,
                            typeof(SwaggerToCSharpGeneratorSettings).GetTypeInfo().Assembly,
                        } )
                        //TemplateDirectory =
                    },
                    AdditionalNamespaceUsages =_settings.GetNamespaces(),
                    GenerateSyncMethods = _settings.GenerateSyncMethods,
                    ClientClassAccessModifier = "public",
                    ClientBaseClass = "ApiBaseClient",
                    ParameterArrayType = "List",        //Default: System.Collections.Generic.IEnumerable
                    ParameterDictionaryType = "Dictonary", //Default: System.Collections.Generic.IDictionary
                    ResponseArrayType = "List", // Default: System.Collections.ObjectModel.ObservableCollection
                    ResponseDictionaryType = "Dictonary", //Default: System.Collections.Generic.Dictionary
                    GenerateDtoTypes = false,
                    GenerateClientInterfaces = true,
                };
            }
            _langCodeGenSettings.OperationNameGenerator=new MultipleClientsFromPathSegmentsOperationNameGenerator();
            _langCodeGenSettings.ClassName = "{controller}Client";
        }

        private void DefineCodeGenerator()
        {
            if(_langCodeGenSettings==default)
                DefineGeneratorSettings();

            if (_settings.LanguageCode == Language.CSharp)
            {
               _langCodeGen = new CSharpCodeGen(Document,(SwaggerToCSharpClientGeneratorSettings) _langCodeGenSettings);
            }
        }

        /// <summary>
        /// Fetch Swagger document from swagger file path or Uri.
        /// </summary>
        /// <remarks>
        /// FilePath takes precidence over Uri
        /// </remarks>
        private void FetchSwaggerDocument()
        {
            Document = Task.Run(() => !string.IsNullOrEmpty(_settings.ApiSpecPath)
                ? SwaggerDocument.FromFileAsync(_settings.ApiSpecPath)
                : SwaggerDocument.FromUrlAsync(_settings.ApiSpecUri)).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Generates the files.
        /// </summary>
        public void GenerateFiles()
        {
            try
            {
                if (!Directory.Exists(_settings.OutputPath))
                    Directory.CreateDirectory(_settings.OutputPath);

                _langCodeGen.GetClientCode()
                            .ToList()
                            .ForEach(c => {                                
                                var fileName = Path.Combine(_settings.OutputPath, $"{c.ClientName}.{_langCodeGen.FileExtension}");

                                Console.WriteLine($"Writing file '{c.ClientName}.{_langCodeGen.FileExtension}' to '{_settings.OutputPath}'");

                                using (TextWriter tw = new StreamWriter(fileName))
                                {
                                    tw.WriteLine(c.Code);
                                    tw.Flush();
                                }
                            });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
