using NearDupFinder.Dominio.Entidades;

namespace NearDupFinder.Dominio.Interfaces;

public interface IUsuarioService
{
    Task<IEnumerable<Usuario>> ObtenerTodos(); 
    Task<Usuario?> ObtenerPorEmail(string email);
    Task<Usuario?> ObtenerPorId(int id);
    Task<Usuario> CrearUsuario(Usuario usuario);
    Task<Usuario> ActualizarUsuario(Usuario usuario);
    Task<bool> EliminarUsuario(int id);
    Task<bool> ValidarEmail(string email);
    Task<bool> ValidarEmailEdicion(string email, int idUsuario);      
    Task<IEnumerable<Usuario>> ObtenerPorRol(string rol);
    Task<IEnumerable<Usuario>> ObtenerAdministradores();
    Task<IEnumerable<Usuario>> ObtenerRevisores();    
}