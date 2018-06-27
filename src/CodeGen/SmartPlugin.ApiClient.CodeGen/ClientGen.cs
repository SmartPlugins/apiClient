using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;
using NSwag;
using NSwag.CodeGeneration;
using NSwag.CodeGeneration.CSharp;
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
            if (_settings.Language == Language.CSharp)
            {
                _langCodeGenSettings = new SwaggerToCSharpClientGeneratorSettings()
                {
                    CSharpGeneratorSettings =
                    {
                        Namespace = _settings.ClientNamespace,
                        TemplateFactory = new DefaultTemplateFactory(new CSharpGeneratorSettings (){
                                                                    Namespace = _settings.ClientNamespace,
                                                                    SchemaType = SchemaType.Swagger2},  new[]
                        {
                            typeof(CSharpGeneratorSettings).GetTypeInfo().Assembly,
                            typeof(SwaggerToCSharpGeneratorSettings).GetTypeInfo().Assembly,
                        } )
                        //TemplateDirectory =
                    },
                    AdditionalNamespaceUsages =_settings.GetNamespaces(),
                    GenerateSyncMethods = _settings.GenerateSyncMethods,
                    ClientClassAccessModifier = "public",
                    ClientBaseClass = "ApiBaseClient"
                };
            }
            _langCodeGenSettings.OperationNameGenerator=new MultipleClientsFromPathSegmentsOperationNameGenerator();
            _langCodeGenSettings.ClassName = "{controller}Client";
        }

        private void DefineCodeGenerator()
        {
            if(_langCodeGenSettings==default)
                DefineGeneratorSettings();

            if (_settings.Language == Language.CSharp)
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

            //TODO: Write files
        }
    }
}
