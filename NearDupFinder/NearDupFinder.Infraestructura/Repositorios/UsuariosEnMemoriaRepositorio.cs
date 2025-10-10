using NearDupFinder.Dominio.Entidades;
using NearDupFinder.Dominio.Interfaces;
using NearDupFinder.Dominio.Utiles;

namespace NearDupFinder.Infraestructura.Repositorios;

  public class UsuariosEnMemoriaRepositorio : IRepositorioUsuarios
{
    private List<Usuario> _usuarios = new();
    private int _siguienteId;

    public UsuariosEnMemoriaRepositorio()
    {
        _usuarios = new List<Usuario>();
        _siguienteId = 1;
        InicializarDatos();
    }

    private void InicializarDatos()
    {
        _usuarios.AddRange(new[]
        {
            new Usuario
    {
                Id=_siguienteId++,
        Email = "admin@gmail.com",
        PasswordHash = PasswordHasher.Hash("1234"),
        Nombre = "Admin",
        Apellido="Mendez",
        FechaNacimiento = new  DateTime(1970, 01, 01),
        Roles = new List<string> { RolesConstantes.ADMINISTRADOR }
    },
    new Usuario
    {
         Id=_siguienteId++,
        Email = "user@gmail.com",
        PasswordHash = PasswordHasher.Hash("1234"),
        Nombre = "Usuario Normal",
        FechaNacimiento = new  DateTime(1981, 01, 01),
        Roles = new List<string> { RolesConstantes.REVISOR_CATALOGO}
    }
        });
    }

    public Task<IEnumerable<Usuario>> ObtenerTodos()
    {
        return Task.FromResult<IEnumerable<Usuario>>(_usuarios.ToList());
    }

    public Task<Usuario?> ObtenerPorId(int id)
    {
        var usuario = _usuarios.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(usuario);
    }

    public Task<Usuario?> ObtenerPorEmail(string email)
    {
        var usuario = _usuarios.FirstOrDefault(u =>
            u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(usuario);
    }

    public Task<IEnumerable<Usuario>> ObtenerPorRol(string rol)
    {
        var usuarios = _usuarios.Where(u => u.Roles.Contains(rol)).ToList();
        return Task.FromResult<IEnumerable<Usuario>>(usuarios);
    }

    public Task<bool> ExisteEmail(string email)
    {
        var existe = _usuarios.Any(u =>
            u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(existe);
    }

    public Task<bool> ExisteEmailExcluyendoId(string email, int idExcluir)
    {
        var existe = _usuarios.Any(u =>
            u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
            u.Id != idExcluir);
        return Task.FromResult(existe);
    }

    public Task<Usuario> Agregar(Usuario entidad)
    {
        entidad.Id = _siguienteId++;
        _usuarios.Add(entidad);
        return Task.FromResult(entidad);
    }

    public Task<Usuario> Actualizar(Usuario entidad)
    {
        var index = _usuarios.FindIndex(u => u.Id == entidad.Id);
        if (index >= 0)
        {
            _usuarios[index] = entidad;
            return Task.FromResult(entidad);
        }
        throw new InvalidOperationException($"Usuario con ID {entidad.Id} no encontrado");
    }

 
    public Task<bool> Eliminar(int id)
    {
        var usuario = _usuarios.FirstOrDefault(u => u.Id == id);
        if (usuario != null)
        {
            _usuarios.Remove(usuario);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    } 
  
} 