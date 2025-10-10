namespace NearDupFinder.Dominio.Exceptions;

public class ProductoException: Exception
{
    public ProductoException() { }

    public ProductoException(string message) : base(message) { }

    public ProductoException(string message, Exception inner) : base(message, inner) { }
}