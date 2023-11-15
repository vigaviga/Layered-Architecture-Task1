namespace Carting.Carting.Services.Exceptions
{
    public class EntityWithGivenIdExistsException : Exception
    {
        public EntityWithGivenIdExistsException() { }

        public EntityWithGivenIdExistsException(string message) : base(message) { }

        public EntityWithGivenIdExistsException(string message, Exception innerException) : base(message, innerException) { }
    }
}
