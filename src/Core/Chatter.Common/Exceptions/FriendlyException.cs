using System.Net;
using Chatter.Common.Models.ErrorModels;

namespace Chatter.Common.Exceptions;

public class FriendlyException : Exception
{
    public HttpStatusCode HttpStatusCode { get; set; }
    public ErrorMessage ErrorMessage { get; set; }

    public FriendlyException(string errorMessage, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest) :
        base(errorMessage)
    {
        HttpStatusCode = httpStatusCode;
        ErrorMessage = new ErrorMessage
        {
            Message = errorMessage
        };
    }
        
    public FriendlyException(ErrorMessage errorMessage, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest) :
        base(errorMessage.Message)
    {
        HttpStatusCode = httpStatusCode;
        ErrorMessage = errorMessage;
    }

}