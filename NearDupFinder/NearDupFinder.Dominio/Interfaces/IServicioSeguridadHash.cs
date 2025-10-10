namespace NearDupFinder.Dominio.Interfaces;

public interface IServicioSeguridadHash
{
    bool Verificar(string password, string passwordHash);
}