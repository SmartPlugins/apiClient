using SmartPlugin.ApiClient.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SmartPlugin.ApiClient.Model
{
    /// <summary>
    /// Reflection based API operation information
    /// </summary>
    public sealed class OperationInfo
    {
        public OperationInfo() { }

        public OperationInfo(MethodBase metaInfo, RestActionAttribute actionInfo)
        {
            MetaInfo = metaInfo;
            ActionInfo = actionInfo;
        }

        /// <summary>
        /// Method base reflection object
        /// </summary>
        public MethodBase MetaInfo { get; internal set; }

        /// <summary>
        /// RestAction attribute defined on the Api operation
        /// </summary>
        public RestActionAttribute ActionInfo { get; internal set; }

        //public ParamBindings ParamBindingInfo { get; internal set; }
    }
}
