using NearDupFinder.Dominio.Exceptions;
using NearDupFinder.Dominio.Utiles;

namespace NearDupFinder.Dominio.Entidades;

public class Usuario
{ 
    public int Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Password { get; set; }
    public string ConfirmarPassword { get; set; }
    
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public List<string> Roles { get; set; } = new();
    public DateTime FechaNacimiento { get; set; }
    public bool TieneRol(string rol) => Roles.Contains(rol);
    public bool EsAdministrador => TieneRol(RolesConstantes.ADMINISTRADOR);
    public bool EsRevisor => TieneRol(RolesConstantes.REVISOR_CATALOGO);
    
    
    public Usuario()
    {
        
    }
    
    public Usuario(int id,string nombre, string apellido, string email, DateTime fechaNacimiento,string passwordHash="",List<string> roles=null )
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new UsuarioException("El nombre no puede ser vacío.");
        }
      
        if (string.IsNullOrWhiteSpace(apellido))
        {
            throw new UsuarioException("El apellido no puede ser vacío.");
        }
       
        if (!Validador.IsEmailValido(email))
        {
            throw new UsuarioException("El formato del email no es válido.");
        }
        
        if (fechaNacimiento > DateTime.Today)
        {
            throw new UsuarioException("La fecha de nacimiento no puede ser mayor a la actual.");
        }
        this.Id = id;   
        this.Nombre = nombre;
        this.Apellido = apellido;
        this.Email = email;
        this.FechaNacimiento = fechaNacimiento;
        this.PasswordHash = passwordHash;
        this.Roles = roles;
    }
}