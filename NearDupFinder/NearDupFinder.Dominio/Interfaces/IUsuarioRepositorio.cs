using NearDupFinder.Dominio.Entidades;

namespace NearDupFinder.Dominio.Interfaces;

public interface IRepositorioUsuarios: IRepositorio<Usuario>
{
    Task<Usuario?> ObtenerPorEmail(string email);          
    Task<IEnumerable<Usuario>> ObtenerPorRol(string rol);       
    Task<bool> ExisteEmail(string email);
    Task<bool> ExisteEmailExcluyendoId(string email, int idExcluir);       
}