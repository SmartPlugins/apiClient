using NSwag.CodeGeneration.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartPlugin.ApiClient.CodeGen.Model
{
    public class BaseClientInfo
    {

        /// <summary>
        /// Gets the name of the client class.
        /// </summary>
        /// <value>
        /// The name of the client class.
        /// </value>
        public string ClientClassName { get; private set; }

        /// <summary>
        /// Gets the name of the client controller.
        /// </summary>
        /// <value>
        /// The name of the client controller.
        /// </value>
        public string ClientControllerName { get; private set; }

        /// <summary>
        /// Gets the operations.
        /// </summary>
        /// <value>
        /// The operations.
        /// </value>
        public List<IOperationModel> Operations { get; private set; }

        /// <summary>
        /// Gets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseClientInfo"/> class.
        /// </summary>
        /// <param name="clientClassName">Name of the client class.</param>
        /// <param name="clientControllerName">Name of the client controller.</param>
        /// <param name="operations">The operations.</param>
        public BaseClientInfo(string clientClassName, string clientControllerName, List<IOperationModel> operations)
        {
            ClientClassName = clientClassName;
            ClientControllerName = clientControllerName;
            Operations = operations;
        }
    }
}
