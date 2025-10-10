using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

using Microsoft.AspNetCore.Http;
using NearDupFinder.Dominio.Entidades;
using NearDupFinder.Dominio.Interfaces;

namespace NearDupFinder.Aplicacion;

 
public class AuditoriaService: IAuditoriaService
{
    private readonly IAuditoriaRepository _auditoriaRepository;
    private readonly AuthenticationStateProvider _authStateProvider;
   
    public AuditoriaService(IAuditoriaRepository auditoriaRepository, AuthenticationStateProvider authStateProvider)
    {
        _auditoriaRepository = auditoriaRepository;
        _authStateProvider = authStateProvider;
    }

    public async Task LogAccion(string accion, string detalles = "")
    {
        var authState = await _authStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        
        var emailUsuarioLogueado = user?.FindFirst(ClaimTypes.Email)?.Value 
                                   ?? user?.FindFirst("email")?.Value
                                   ?? user?.Identity?.Name
                                   ?? "Sistema";

        var entry = new AuditoriaEntry(emailUsuarioLogueado, accion, detalles);
        await _auditoriaRepository.Log(entry);
    }

    public async Task<List<AuditoriaEntry>> GetLogs()
    {
        return await _auditoriaRepository.GetLogs();
    }
}
 