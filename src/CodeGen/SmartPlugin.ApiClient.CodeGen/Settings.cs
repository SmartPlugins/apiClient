using System;
using System.Collections.Generic;
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
        [Option('n', nameof(ClientNamespace), HelpText = "Namespace to be used on the generated code. Default = 'Auto.Generated.Code'")]
        internal string ClientNamespace { get; set; } = "Auto.Generated.Code";


        /// <summary>
        /// Specify the code pattern to use reflection based invocation vs. static
        /// </summary>
        [Option('i', nameof(DynamicInvoke), HelpText = "Specify the code pattern to use reflection based invocation vs. static. Default= true")]
        internal bool DynamicInvoke { get; set; } = true;

        /// <summary>
        /// Generate config
        /// </summary>
        [Option('c', nameof(ScaffoldConfig), HelpText = "Generate config file. Default =true")]
        internal bool ScaffoldConfig { get; set; } = true;

        /// <summary>
        /// Path for the generated code
        /// </summary>
        [Option('o', nameof(OutputPath), HelpText = "Path for the generated code")]
        internal string OutputPath { get; set; } = System.IO.Path.Combine(Environment.CurrentDirectory, "GeneratedCode");

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        [Option('l', nameof(Language), HelpText = "Code generation language")]
        internal Language Language { get; set; } = Language.CSharp;

        /// <summary>
        /// Gets or sets the additional namespaces.
        /// </summary>
        /// <value>
        /// The additional namespaces.
        /// </value>
        [Option('a', nameof(AdditionalNamespaces), HelpText = "Semicolon delimited string consisting namespaces to be added to the files")]
        internal string AdditionalNamespaces { get; set; }

        /// <summary>
        /// Gets or sets the generate synchronize methods.
        /// </summary>
        /// <value>
        /// The generate synchronize methods.
        /// </value>
        [Option('s', nameof(GenerateSyncMethods), HelpText ="Specify synchronous method generation. Default is 'true'.")]
        internal bool GenerateSyncMethods { get; set; } = true;

        internal string[] GetNamespaces()=>
            AdditionalNamespaces?.Split(';')?.ToArray();
    }

    public enum Language
    {
        CSharp=0,
        //TypeScript=1
    }
}
