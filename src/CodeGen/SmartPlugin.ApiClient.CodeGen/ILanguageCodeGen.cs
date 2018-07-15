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

        string FileExtension { get; }
        List<NSwag.CodeGeneration.Models.IOperationModel> Operations{ get; }
        List<BaseClientInfo> Clients { get; }
        void IdentifyClients();
        List<(string ClientName, string Code)> GetClientCode();
        List<(string ClientName, string Code)> GetClientCode(ClientGeneratorOutputType outputType);
       string GenerateClientClass(BaseClientInfo csci, ClientGeneratorOutputType outputType);

        string GenerateClientCode(string controllerName, string controllerClassName, IList<IOperationModel> operations,
            ClientGeneratorOutputType outputType);

        List<IOperationModel> GetOperations();

        string GetConfiguration();

        Language Language { get; }
    }

    public enum Language
    {
        CSharp = 0,
        //TypeScript=1
    }
}
