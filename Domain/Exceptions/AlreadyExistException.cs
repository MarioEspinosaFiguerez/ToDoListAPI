namespace Domain.Exceptions;

    public class AlreadyExistException : Exception
    {
        // We can put the parameter that already exists
        public object Key { get; }

        public AlreadyExistException() : base() { }

    public AlreadyExistException(string? message, object key) : base(message) { Key = key; }

        public AlreadyExistException(string? message, object key, Exception? innerException) : base(message, innerException) { Key = key; }
    }