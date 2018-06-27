using NSwag.CodeGeneration;
using SmartPlugin.ApiClient.CodeGen.Model;
using System;
using System.Collections.Generic;
using System.Text;
using NSwag.CodeGeneration.Models;

namespace SmartPlugin.ApiClient.CodeGen
{
    public interface ILanguageCodeGen
    {
        NSwag.SwaggerDocument Document { get; }
        List<NSwag.CodeGeneration.Models.IOperationModel> Operations{ get; }
        List<BaseClientInfo> Clients { get; }
        void IdentifyClients();
        Dictionary<string, string> GetClientCode();
        Dictionary<string, string> GetClientCode(ClientGeneratorOutputType outputType);
       string GenerateClientClass(BaseClientInfo csci, ClientGeneratorOutputType outputType);

        string GenerateClientCode(string controllerName, string controllerClassName, IList<IOperationModel> operations,
            ClientGeneratorOutputType outputType);

        List<IOperationModel> GetOperations();

        string GetConfiguration();
    }
}
