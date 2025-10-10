using NearDupFinder.Dominio.Entidades;

namespace NearDupFinder.Dominio.Interfaces;

public interface IAuditoriaRepository
{
    Task Log(AuditoriaEntry entry);
    Task<List<AuditoriaEntry>> GetLogs();
}