namespace IpGeolocation.Exceptions;

public class QuotaExceededException(string message) : Exception(message);