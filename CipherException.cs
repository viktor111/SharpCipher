namespace SharpCipher;

public class CipherException : Exception
{
    public CipherException() { }

    public CipherException(string message) : base(message) { }

    public CipherException(string message, Exception innerException)
        : base(message, innerException) { }
}