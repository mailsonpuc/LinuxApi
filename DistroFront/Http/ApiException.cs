using System.Net;

namespace DistroFront.Http;

public sealed class ApiException : Exception
{
    public ApiException(HttpStatusCode statusCode, string message)
        : base(message)
    {
        StatusCode = statusCode;
    }

    public HttpStatusCode StatusCode { get; }
}
