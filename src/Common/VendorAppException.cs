using System;
using System.Net;

namespace MvpVendingMachineApp.Common
{
    public class VendorAppException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public ApiResultStatusCode ApiStatusCode { get; set; }
        public object AdditionalData { get; set; }

        public VendorAppException()
            : this(ApiResultStatusCode.ServerError)
        {
        }

        public VendorAppException(ApiResultStatusCode statusCode)
            : this(statusCode, null)
        {
        }

        public VendorAppException(string message)
            : this(ApiResultStatusCode.ServerError, message)
        {
        }

        public VendorAppException(ApiResultStatusCode statusCode, string message)
            : this(statusCode, message, HttpStatusCode.InternalServerError)
        {
        }

        public VendorAppException(string message, object additionalData)
            : this(ApiResultStatusCode.ServerError, message, additionalData)
        {
        }

        public VendorAppException(ApiResultStatusCode statusCode, object additionalData)
            : this(statusCode, null, additionalData)
        {
        }

        public VendorAppException(ApiResultStatusCode statusCode, string message, object additionalData)
            : this(statusCode, message, HttpStatusCode.InternalServerError, additionalData)
        {
        }

        public VendorAppException(ApiResultStatusCode statusCode, string message, HttpStatusCode httpStatusCode)
            : this(statusCode, message, httpStatusCode, null)
        {
        }

        public VendorAppException(ApiResultStatusCode statusCode, string message, HttpStatusCode httpStatusCode, object additionalData)
            : this(statusCode, message, httpStatusCode, null, additionalData)
        {
        }

        public VendorAppException(string message, Exception exception)
            : this(ApiResultStatusCode.ServerError, message, exception)
        {
        }

        public VendorAppException(string message, Exception exception, object additionalData)
            : this(ApiResultStatusCode.ServerError, message, exception, additionalData)
        {
        }

        public VendorAppException(ApiResultStatusCode statusCode, string message, Exception exception)
            : this(statusCode, message, HttpStatusCode.InternalServerError, exception)
        {
        }

        public VendorAppException(ApiResultStatusCode statusCode, string message, Exception exception, object additionalData)
            : this(statusCode, message, HttpStatusCode.InternalServerError, exception, additionalData)
        {
        }

        public VendorAppException(ApiResultStatusCode statusCode, string message, HttpStatusCode httpStatusCode, Exception exception)
            : this(statusCode, message, httpStatusCode, exception, null)
        {

        }

        public VendorAppException(ApiResultStatusCode statusCode, string message, HttpStatusCode httpStatusCode, Exception exception, object additionalData)
            : base(message, exception)
        {
            ApiStatusCode = statusCode;
            HttpStatusCode = httpStatusCode;
            AdditionalData = additionalData;
        }
    }
}
