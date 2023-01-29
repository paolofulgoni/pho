using System.Net;
using System.Runtime.Serialization;

namespace Pho.Core.Exceptions;

[Serializable]
public class ThirdPartyServiceException : Exception
{
    public ThirdPartyServiceException()
    {
    }

    protected ThirdPartyServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ThirdPartyServiceException(string message) : base(message)
    {
    }

    public ThirdPartyServiceException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public ThirdPartyServiceException(string message, HttpStatusCode statusCode) : base(message)
    {
        StatusCode = statusCode;
    }

    public ThirdPartyServiceException(string message, HttpStatusCode statusCode, Exception innerException) :
        base(message, innerException)
    {
        StatusCode = statusCode;
    }

    public HttpStatusCode? StatusCode { get; }
}
