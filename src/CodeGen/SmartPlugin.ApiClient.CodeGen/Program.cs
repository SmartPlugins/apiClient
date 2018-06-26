using CommandLine;
using System;

namespace SmartPlugin.ApiClient.CodeGen
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            Parser.Default.ParseArguments<Settings>(args)
                .WithParsed<Settings>(opts => InvokeCodeGen(opts));
        }

        /// <summary>
        /// Invokes the code gen.
        /// </summary>
        /// <param name="settings">The settings.</param>
        private static void InvokeCodeGen(Settings settings)
        {
            settings.ApiSpecPath = @"C:\Temp\EACH\Jasaon.json";
            settings.ApiSpecUri = @"https://petstore.swagger.io/v2/swagger.json";

            if (string.IsNullOrEmpty(settings.ApiSpecPath) && string.IsNullOrEmpty(settings.ApiSpecUri))
                Console.WriteLine("Neither of Api specification file/Uri has been specified for the client generation.");
            else
            {
                settings.DynamicInvoke = false;

                new ClientGen(settings).Generate();

                //coreGen.Generate();
            }
            Console.ReadLine();
        }
    }
}
