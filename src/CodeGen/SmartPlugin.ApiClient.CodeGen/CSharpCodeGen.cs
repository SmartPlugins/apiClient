using System;
using System.Collections.Generic;
using System.Linq;
using CSharpx;
using NJsonSchema.CodeGeneration.CSharp;
using NSwag;
using NSwag.CodeGeneration;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.CSharp.Models;
using NSwag.CodeGeneration.Models;
using SmartPlugin.ApiClient.CodeGen.Model;

namespace SmartPlugin.ApiClient.CodeGen
{
    public class CSharpCodeGen : SwaggerToCSharpClientGenerator, ILanguageCodeGen
    {

        public Language Language => Language.CSharp;

        public  SwaggerDocument Document { get; private set; }
        public List<IOperationModel> Operations { get; private set; }

        public List<BaseClientInfo> Clients { get; private set; }

        public string FileExtension => ".cs";
        /// <summary>Initializes a new instance of the <see cref="SwaggerToCSharpClientGenerator" /> class.</summary>
        /// <param name="document">The Swagger document.</param>
        /// <param name="settings">The settings.</param>
        /// <exception cref="ArgumentNullException"><paramref name="document" /> is <see langword="null" />.</exception>
        public CSharpCodeGen(SwaggerDocument document, SwaggerToCSharpClientGeneratorSettings settings)
            : base(document, settings)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));
            Document = document;
        }

        /// <summary>Initializes a new instance of the <see cref="SwaggerToCSharpClientGenerator" /> class.</summary>
        /// <param name="document">The Swagger document.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="resolver">The resolver.</param>
        /// <exception cref="ArgumentNullException"><paramref name="document" /> is <see langword="null" />.</exception>
        public CSharpCodeGen(SwaggerDocument document, SwaggerToCSharpClientGeneratorSettings settings, CSharpTypeResolver resolver)
            : base(document, settings, resolver)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));
            Document = document;
        }

        /// <summary>
        /// Identifies the clients.
        /// </summary>
        public void IdentifyClients()
        {
            Console.WriteLine($"Parsing swagger spec for '{Enum.GetName(typeof(Language), Language)}' code generation.");
            var operations = GetOperations();
            Clients = new List<BaseClientInfo>();

            Console.WriteLine($"--- Code generation mode : '{nameof(BaseSettings.OperationNameGenerator.SupportsMultipleClients)}:{(BaseSettings.OperationNameGenerator.SupportsMultipleClients? "TRUE":"FALSE")}' ---");

            if (BaseSettings.OperationNameGenerator.SupportsMultipleClients)
            {
                foreach (var controllerOperations in operations.Cast<CSharpOperationModel>().GroupBy(o => o.ControllerName))
                {
                    Console.WriteLine($"Involking code generation for '{controllerOperations.Key}'.");
                    Clients.Add(new CSharpClientInfo(clientClassName: controllerOperations.Key,
                            clientControllerName: BaseSettings.GenerateControllerName(controllerOperations.Key),
                            operations: controllerOperations.ToList())
                    );
                }
            }
            else
            {
                Console.WriteLine($"Involking code generation.");
                Clients.Add(new CSharpClientInfo(clientClassName: string.Empty,
                    clientControllerName: BaseSettings.GenerateControllerName(string.Empty),
                    operations: operations.Cast<CSharpOperationModel>().ToList()));
            }

            //return GenerateFile(clientCode, clientClasses, type)
            //    .Replace("\r", string.Empty)
            //    .Replace("\n\n\n\n", "\n\n")
            //    .Replace("\n\n\n", "\n\n");
        }

        /// <summary>
        /// Gets the client code.
        /// </summary>
        /// <returns></returns>
        public List<(string ClientName, string Code)> GetClientCode()
            => GetClientCode(ClientGeneratorOutputType.Full);

        /// <summary>
        /// Gets the client code.
        /// </summary>
        /// <param name="outputType">Type of the output.</param>
        /// <returns></returns>
        public List<(string ClientName, string Code)> GetClientCode(ClientGeneratorOutputType outputType)
        {
            if (Clients == default)
                IdentifyClients();

            Console.WriteLine("Creating response object from the generated code.");
            Clients?.ForEach(c => { c.Code = GenerateClientClass(c, outputType); });

            return Clients?.Select(k => (k.ClientClassName, k.Code)).ToList();
            //  return (Clients?.Select(c=>c.Code).Aggregate((c, n) => $"{c}\n\n\n{n}"))??string.Empty;
        }

        /// <summary>
        /// Generates the client class.
        /// </summary>
        /// <param name="csci">The csci.</param>
        /// <param name="outputType">Type of the output.</param>
        /// <returns></returns>
        public string GenerateClientClass(BaseClientInfo csci, ClientGeneratorOutputType outputType)
                    => GenerateClientCode(csci.ClientClassName, csci.ClientControllerName, csci.Operations, outputType);

        /// <summary>
        /// Generates the client code.
        /// </summary>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="controllerClassName">Name of the controller class.</param>
        /// <param name="operations">The operations.</param>
        /// <param name="outputType">Type of the output.</param>
        /// <returns></returns>
        public string GenerateClientCode(string controllerName, string controllerClassName, IList<IOperationModel> operations, ClientGeneratorOutputType outputType)
        {
            var exceptionSchema = (Resolver as CSharpTypeResolver)?.ExceptionSchema;
            var model = new CSharpClientTemplateModel(controllerName, controllerClassName, operations.Cast<CSharpOperationModel>(), exceptionSchema, Document, Settings)
            {
                GenerateContracts = outputType == ClientGeneratorOutputType.Full || outputType == ClientGeneratorOutputType.Contracts,
                GenerateImplementation = outputType == ClientGeneratorOutputType.Full || outputType == ClientGeneratorOutputType.Implementation,
            };

           var template = Settings.CSharpGeneratorSettings.TemplateFactory.CreateTemplate("CSharp", "Client", model);
            return template.Render();
        }

        /// <summary>
        /// Gets the operations.
        /// </summary>
        /// <returns></returns>
        public List<IOperationModel> GetOperations()
        {
            Document.GenerateOperationIds();
            Console.WriteLine($"Enumerating operations from the swagger sepc.");
            return Document.Paths
                .SelectMany(pair => pair.Value.Select(p => new { Path = pair.Key.TrimStart('/'), HttpMethod = p.Key, Operation = p.Value }))
                .Select(tuple =>
                {
                    var operationModel = CreateOperationModel(tuple.Operation, BaseSettings);
                    operationModel.ControllerName = BaseSettings.OperationNameGenerator.GetClientName(Document, tuple.Path, tuple.HttpMethod, tuple.Operation);
                    operationModel.Path = tuple.Path;
                    operationModel.HttpMethod = tuple.HttpMethod;
                    operationModel.OperationName = BaseSettings.OperationNameGenerator.GetOperationName(Document, tuple.Path, tuple.HttpMethod, tuple.Operation);
                    return operationModel;
                }).Cast<IOperationModel>()
                .ToList();
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <returns></returns>
        public string GetConfiguration()
        {
            //TODO: Scaffold config
            throw new NotImplementedException();
        }
    }
}
