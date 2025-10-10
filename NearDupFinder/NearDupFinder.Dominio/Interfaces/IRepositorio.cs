namespace NearDupFinder.Dominio.Interfaces;

public interface IRepositorio<T>
{
    Task<IEnumerable<T>> ObtenerTodos();
    Task<T?> ObtenerPorId(int id);
    Task<T> Agregar(T entidad);
    Task<T> Actualizar(T entidad);
    Task<bool> Eliminar(int id);
}