using NearDupFinder.Dominio.Entidades;
using NearDupFinder.Dominio.Interfaces;

namespace NearDupFinder.Infraestructura.Repositorios;

 public class AuditoriaEnMemoriaRepository : IAuditoriaRepository
{
    private readonly List<AuditoriaEntry> _logs = new List<AuditoriaEntry>();

    public Task Log(AuditoriaEntry entry)
    {
         _logs.Add(entry);
        return Task.CompletedTask;
    }

    public Task<List<AuditoriaEntry>> GetLogs()
    {
         return Task.FromResult(new List<AuditoriaEntry>(_logs));
    }
}