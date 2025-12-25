using System;

namespace Px6Api;

public class Px6ApiException : Exception
{
    public int ErrorId { get; }

    public Px6ApiException(int errorId, string message) : base(message)
    {
        ErrorId = errorId;
    }

    public Px6ApiException(int errorId, string message, Exception innerException)
        : base(message, innerException)
    {
        ErrorId = errorId;
    }
}