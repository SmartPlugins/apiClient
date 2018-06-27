using NSwag.CodeGeneration.CSharp.Models;
using NSwag.CodeGeneration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartPlugin.ApiClient.CodeGen.Model
{
    public class CSharpClientInfo: BaseClientInfo
    {
        //public string ClientClassName { get; private set; }
        //public string ClientControllerName { get; private set; }
        //public List<CSharpOperationModel> Operations { get; private set; }
        //public string Code { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpClientInfo"/> class.
        /// </summary>
        /// <param name="clientClassName">Name of the client class.</param>
        /// <param name="clientControllerName">Name of the client controller.</param>
        /// <param name="operations">The operations.</param>
        public CSharpClientInfo(string clientClassName, string clientControllerName, List<CSharpOperationModel> operations):base(clientClassName, clientControllerName, operations.Cast<IOperationModel>().ToList())
        {
        }

    }
}
