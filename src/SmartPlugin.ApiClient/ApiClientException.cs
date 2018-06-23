using System;
using System.Collections.Generic;

namespace SmartPlugin.ApiClient
{
    [Serializable]
    public class ApiClientException<TResult> : ApiClientException
    {
        public TResult Result { get; private set; }

        public ApiClientException(string message, int statusCode, string response, System.Collections.Generic.Dictionary<string, System.Collections.Generic.IEnumerable<string>> headers, TResult result, System.Exception innerException)
            : base(message, statusCode, response, headers, innerException)
        {
            Result = result;
        }

        protected ApiClientException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
            throw new NotImplementedException();
        }
    }

    [Serializable]
    public class ApiClientException : Exception
    {
        public int StatusCode { get; private set; }

        public string Response { get; private set; }

        public Dictionary<string, IEnumerable<string>> Headers { get; private set; }

        public ApiClientException()
        {
        }

        public ApiClientException(string message) : base(message)
        {
        }

        public ApiClientException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ApiClientException(string message, int statusCode, string responseData, Dictionary<string, IEnumerable<string>> headers, Exception exception) : base(message, exception)
        {
            StatusCode = statusCode;
            Response = responseData;
            Headers = headers;
        }

        public override string ToString()
        {
            return string.Format("HTTP Response: \n\n{0}\n\n{1}", Response, base.ToString());
        }

        protected ApiClientException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
            throw new NotImplementedException();
        }
    }
}
