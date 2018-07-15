using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using NSwag;

namespace SmartPlugin.ApiClient.CodeGen
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            Parser.Default.ParseArguments<Settings>(args)
                .WithParsed<Settings>(opts => InvokeCodeGen(opts))
                .WithNotParsed<Settings>((errors)=> HandleArgumentErrors(errors.ToList()));
        }

        private static void HandleArgumentErrors(List<Error> errors)
        {
            
        }

        /// <summary>
        /// Invokes the code gen.
        /// </summary>
        /// <param name="settings">The settings.</param>
        private static void InvokeCodeGen(Settings settings)
        {
            settings.ApiSpecPath = @"C:\Temp\EACH.json";
           // settings.ApiSpecUri = @"https://petstore.swagger.io/v2/swagger.json";

            if (string.IsNullOrEmpty(settings.ApiSpecPath) && string.IsNullOrEmpty(settings.ApiSpecUri))
                Console.WriteLine("Neither of Api specification file/Uri has been specified for the client generation.");
            else
            {
                settings.DynamicInvoke = false;

                new ClientGen(settings).GenerateFiles();

                //coreGen.Generate();
            }
            Console.ReadLine();
        }
    }


    //public class Test
    //{

    //    //MultipleClientsFromPathSegmentsOperationNameGenerator.cs
    //    //MultipleClientsFromOperationIdOperationNameGenerator.cs

    //    private List<TOperationModel> GetOperations(SwaggerDocument document)
    //    {
    //        document.GenerateOperationIds();

    //        return document.Paths
    //            .SelectMany(pair => pair.Value.Select(p => new { Path = pair.Key.TrimStart('/'), HttpMethod = p.Key, Operation = p.Value }))
    //            .Select(tuple =>
    //            {
    //                var operationModel = CreateOperationModel(tuple.Operation, BaseSettings);
    //                operationModel.ControllerName = BaseSettings.OperationNameGenerator.GetClientName(document, tuple.Path, tuple.HttpMethod, tuple.Operation);
    //                operationModel.Path = tuple.Path;
    //                operationModel.HttpMethod = tuple.HttpMethod;
    //                operationModel.OperationName = BaseSettings.OperationNameGenerator.GetOperationName(document, tuple.Path, tuple.HttpMethod, tuple.Operation);
    //                return operationModel;
    //            })
    //            .ToList();
    //    }
    //}
}
