using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using CommandLine;
using NSwag.CodeGeneration.OperationNameGenerators;

namespace SmartPlugin.ApiClient.CodeGen
{
    public enum FileGenerationMode
    {
        SingleClientFromPathSegments=0,
        SingleClientFromOperationId = 1,
        MultipleClientsFromPathSegment = 2,
        MultipleClientsFromOperationId = 3,
    }

    internal class Settings
    {
        /// <summary>
        /// Gets or sets the API spec path.
        /// </summary>
        /// <value>
        /// The API spec path.
        /// </value>
        [Option('f', "specFilePath", Required = false,HelpText = "File path to the Api specification json file.")]
        internal string ApiSpecPath { get; set; }

        /// <summary>
        /// Gets or sets the API spec URI.
        /// </summary>
        /// <value>
        /// The API spec URI.
        /// </value>
        [Option('u', "specFileUri", Required = false, HelpText = "File path to the Api specification json file.")]
        internal string ApiSpecUri { get; set; }

        /// <summary>
        /// Namespace to be used on the generated code
        /// </summary>
        [Option('n', HelpText = "Namespace to be used on the generated code. Default = 'Auto.Generated.Code'")]
        internal string ClientNamespace { get; set; } = "Auto.Generated.Code";


        /// <summary>
        /// Specify the code pattern to use reflection based invocation vs. static
        /// </summary>
        [Option('d', HelpText = "Specify the code pattern to use reflection based invocation vs. static. Default= true", Hidden = true)]
        internal bool DynamicInvoke { get; set; } = true;

        /// <summary>
        /// Generate config
        /// </summary>
        [Option('c', HelpText = "Generate config file.", Default =true)]
        internal bool ScaffoldConfig { get; set; }

        /// <summary>
        /// Path for the generated code
        /// </summary>
        [Option('o', HelpText = "Path for the generated code")]
        internal string OutputPath { get; set; } = System.IO.Path.Combine(Environment.CurrentDirectory, "GeneratedCode");

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        [Option('l', HelpText = "Code generation language", Default = "CSharp", Required = true)]
        internal string Language { get; set; }

        [Value(0,  HelpText = "Code generation language", Default = CodeGen.Language.CSharp, Required = true, Hidden = true )]
        internal Language LanguageCode {
            get
            {
                if (string.IsNullOrEmpty(Language))
                    return CodeGen.Language.CSharp;
                else
                {
                    //Enum.TryParse(typeof(Language), Language,  out  Language lang);
                    Enum.TryParse(Language, out SmartPlugin.ApiClient.CodeGen.Language lang);
                    return lang;
                }
            }
        }


        /// <summary>
        /// Gets or sets the additional namespaces.
        /// </summary>
        /// <value>
        /// The additional namespaces.
        /// </value>
        [Option('a', HelpText = "Semicolon delimited string consisting namespaces to be added to the files")]
        internal string AdditionalNamespaces { get; set; }

        /// <summary>
        /// Gets or sets the generate synchronize methods.
        /// </summary>
        /// <value>
        /// The generate synchronize methods.
        /// </value>
        [Option('s', HelpText ="Specify synchronous method generation.",  Default = true)]
        internal bool GenerateSyncMethods { get; set; }

        /// <summary>
        /// Gets or sets the generate synchronize methods.
        /// </summary>
        /// <value>
        /// The generate synchronize methods.
        /// </value>
        [Option('i', HelpText = "Specify generator to generateclient interfaces.", Default = true)]
        internal bool GenerateClientInterfaces { get; set; }

        /// <summary>
        /// Gets or sets the generate synchronize methods.
        /// </summary>
        /// <value>
        /// The generate synchronize methods.
        /// </value>
        [Option('t', nameof(TemplatesAssemblyName), HelpText = "Specify the name of the assembly containing the templates for the generation.", Required = true)]
        internal string TemplatesAssemblyName { get; set; }

        internal string[] GetNamespaces()
        {
            var namespaces= DefaultNamespaces;
                    namespaces.AddRange(AdditionalNamespaces?.Split(';').ToList() ?? new List<string>());
                return namespaces?.Distinct().OrderBy(n => n.Length).ThenBy(n => n).ToArray();
        }

        private List<string> DefaultNamespaces=>new List<string>(){ "System.Threading", "System.Collections.Generic", "System.Linq", "SmartPlugin.ApiClient.Model", "SmartPlugin.ApiClient.Enums", "SmartPlugin.ApiClient.Attributes" } ;

        public (bool IsValid, List<string> Validations) ValidateInputs()
        {
            List<string> validations = new List<string>();

            if (string.IsNullOrEmpty(ApiSpecPath) && string.IsNullOrEmpty(ApiSpecUri))
                validations.Add("Neither of Api specification file/Uri has been specified for the client generation.");

            return (validations.Count == 0, validations);
        }
    }

    public enum Language
    {
        CSharp=0,
        //TypeScript=1
    }
}
