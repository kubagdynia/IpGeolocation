namespace IpGeolocation.Exceptions;

public class InvalidKeyException : Exception
{
    public InvalidKeyException(string message) : base(message)
    {
    }
}