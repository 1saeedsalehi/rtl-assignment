using System.Runtime.Serialization;

namespace Rtl.MazeScrapper.Domain.Exceptions;

public class DomainLogicException : Exception
{
    public DomainLogicException()
    {
    }

    public DomainLogicException(string? message) : base(message)
    {
    }

    public DomainLogicException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected DomainLogicException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
