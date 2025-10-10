using NearDupFinder.Dominio.Entidades;
using NearDupFinder.Dominio.Exceptions;

namespace NearDupFinder.Test.Dominio;

[TestClass]
public class UsuarioTest
{
    [TestMethod]
    public void CrearUsuario_Constructor_Default_DeberiaCrearInstanciaValida()
    {
        // Arrange & Act
        var usuario = new Usuario();
            
        // Assert
        Assert.IsNotNull(usuario);
        Assert.IsNull(usuario.Nombre);
        Assert.IsNull(usuario.Apellido);
        Assert.IsNull(usuario.Email);
        Assert.AreEqual(default(DateTime), usuario.FechaNacimiento);
        Assert.IsNull(usuario.PasswordHash);
       
    }
    
    [TestMethod]
    public void CrearUsuario_ConDatosValidos_DebeAsignarPropiedadesCorrectamente()
    {
        // Arrange
        int id = 0;
        string nombre = "Sebastian";
        string apellido = "Mendez";
        string email = "test@test.com";
        DateTime fechaNacimiento = new DateTime(1981, 5, 23);

        // Act
        var usuario = new Usuario(id,nombre, apellido, email, fechaNacimiento);

        // Assert
        Assert.AreEqual(id, usuario.Id);
        Assert.AreEqual(nombre, usuario.Nombre);
        Assert.AreEqual(apellido, usuario.Apellido);
        Assert.AreEqual(email, usuario.Email);
        Assert.AreEqual(fechaNacimiento, usuario.FechaNacimiento);
    }
    
    [TestMethod]
    public void Constructor_DeberiaLanzarArgumentException_CuandoNombreEsVacio()
    {
        // Arrange
        int id = 0;
        string nombre = "";
        string apellido = "Mendez";
        string email = "test@test.com";
        DateTime fecha = new DateTime(1981, 3, 23);

        // Act & Assert
        Assert.ThrowsException<UsuarioException>(() => 
        {
            new Usuario(id,nombre, apellido, email, fecha);
        });
    }
    
    
    [TestMethod]
    public void Constructor_DeberiaLanzarArgumentException_CuandoApellidoEsVacio()
    {
        // Arrange
        int id = 0;
        string nombre = "Sebastian";
        string apellido = "";
        string email = "test@test.com";
        DateTime fecha = new DateTime(1981, 3, 23);
         
        // Act & Assert
          Assert.ThrowsException<UsuarioException>(() => 
        {
            new Usuario(id,nombre, apellido, email, fecha);
        });
    }
    
    
    [TestMethod]
    public void Constructor_DeberiaLanzarArgumentException_CuandoEmailNoEsValido()
    {
        // Arrange
        int id = 0;
        string nombre = "Sebastian";
        string apellido = "Mendez";
        string email = "noesunemail@";
        DateTime fecha = new DateTime(1981, 3, 23);

        // Act & Assert
         Assert.ThrowsException<UsuarioException>(() => 
        {
            new Usuario(id,nombre, apellido, email, fecha);
        });
    }
    
    [TestMethod]
    public void Constructor_DeberiaGuardarFechaNacimientoCorrectamente()
    {
        // Arrange
        DateTime fecha = new DateTime(1981, 5, 23);

        // Act
        var usuario = new Usuario(0,"Sebastian", "Mendez", "test@test.com", fecha);

        // Assert
        Assert.AreEqual(fecha, usuario.FechaNacimiento);
    }
    
    
    [TestMethod]
    public void Constructor_DeberiaLanzarArgumentException_CuandoFechaNacimientoEsFutura()
    {
        // Arrange
        string nombre = "Sebastian";
        string apellido = "Mendez";
        string email = "test@test.com";
        DateTime fecha = DateTime.Today.AddDays(1);  
  
        // Act & Assert
         Assert.ThrowsException<UsuarioException>(() => 
        {
            new Usuario(0,nombre, apellido, email, fecha);
        });
    }
    
   
}