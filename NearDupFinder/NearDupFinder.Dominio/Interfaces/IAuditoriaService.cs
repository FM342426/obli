using NearDupFinder.Dominio.Entidades;

namespace NearDupFinder.Dominio.Interfaces;

public interface  IAuditoriaService
{
    Task LogAccion(string accion, string detalles = "");
    Task<List<AuditoriaEntry>> GetLogs();
}