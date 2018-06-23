using SmartPlugin.ApiClient.Attributes;
using SmartPlugin.ApiClient.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SmartPlugin.ApiClient.Extensions
{
    public static class Extensions
    {
        public static string AttributeName(this Attribute attr) => attr.GetType().Name.Replace("Attribute", string.Empty);
        public static string TagEnclosed(this IAttribute attr) => $"{attr.Tag}";

        public static string GetName(this Enum value) => Enum.GetName(value.GetType(), value);
    }

    public static class CodeGenerator
    {
        public static OperationInfo GetOperationInfo(this StackTrace callStack)
        {
            var mI = callStack.GetOperationMetaData();
            return new OperationInfo(mI, mI?.GetOperationActionInfo());
        }

        /// <summary>
        /// Returns the <see cref="MethodBase"/> of the calling functions from the call stack with action attibute <see cref="RestActionAttribute"/>  that is defined in the Client class inherited from <see cref="ApiBaseClient"/>
        /// </summary>
        /// <param name="callStack"></param>
        /// <returns></returns>
        public static MethodBase GetOperationMetaData(this StackTrace callStack) =>
            callStack?.GetFrames()
                .Where(sf => sf.GetMethod().GetCustomAttribute<RestActionAttribute>() != null && (typeof(AbstractApiBaseClient)).IsAssignableFrom(sf.GetMethod().DeclaringType))
                .FirstOrDefault()
                ?.GetMethod();

        /// <summary>
        ///
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static RestActionAttribute GetOperationActionInfo(this MethodBase method) =>
            method?.GetCustomAttributes<RestActionAttribute>()?.FirstOrDefault();
    }

}
