namespace Domain.Exceptions;

public class NotFoundException : Exception
{
    public object Key { get; }

    public NotFoundException() : base() { }

    public NotFoundException(string message, object key) : base(message) { Key = key; }

    public NotFoundException(string? message, object key, Exception? innerException) : base(message, innerException) { Key = key; }
}