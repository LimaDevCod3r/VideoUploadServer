
namespace VideoUploadServer.Exceptions;


public class InsufficientStorageException : Exception
{
    public InsufficientStorageException(string message) : base(message) { }

}
