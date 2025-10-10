using System.Security.Claims;
 
using NearDupFinder.Dominio.Entidades;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;


namespace NearDupFinder.Aplicacion;

public class LoginService
{
    private readonly Seguridad _seguridad;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public LoginService(Seguridad seguridad)
    {
        _seguridad = seguridad;
    }

    public Task<Usuario?> Autenticar(string email, string password)
    {
        return _seguridad.Autenticar(email, password);
    }

 

}