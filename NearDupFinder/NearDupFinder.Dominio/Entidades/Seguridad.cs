using NearDupFinder.Dominio.Interfaces;
using NearDupFinder.Dominio.Utiles;

namespace NearDupFinder.Dominio.Entidades;

public class Seguridad
{
    
    private readonly IRepositorioUsuarios _repositorio; 
    
    public Seguridad(IRepositorioUsuarios repositorio)
    {
        _repositorio = repositorio; 
    }
    
    public async Task<Usuario> Autenticar(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || !Validador.IsEmailValido(email))
        {
            return null;
        }

        try
        {
            var usuario = await _repositorio.ObtenerPorEmail(email);
            
            if (usuario == null)
            {
                return null;
            }
          
            if (!PasswordHasher.Verificar(password, usuario.PasswordHash))
            {
                return null;
            }
          
            return usuario;
        }
        catch (Exception ex)
        {
            return null; 
        }
    }
}