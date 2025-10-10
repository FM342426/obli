namespace NearDupFinder.Dominio.Exceptions;

public class InvalidItemException : Exception
{
    public InvalidItemException(string mensajeTexto) : base(mensajeTexto) { }
}