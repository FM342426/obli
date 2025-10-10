using NearDupFinder.Dominio.Entidades;

namespace NearDupFinder.Test.Dominio;

[TestClass]
public class UsuarioRolesTests
{

    [TestMethod]
    public void TieneRol_DebeDevolverTrue_SiUsuarioTieneElRol()
    {
        // Arr
        var rolesDelUsuario = new List<string> { RolesConstantes.ADMINISTRADOR, RolesConstantes.REVISOR_CATALOGO };
        var usuario = new Usuario(0,"Juan", "Perez", "juan@test.com", DateTime.Now.AddYears(-44), roles: rolesDelUsuario);

        // Act
        bool resultado = usuario.TieneRol(RolesConstantes.ADMINISTRADOR);

        // Ass
        Assert.IsTrue(resultado);
    }

    [TestMethod]
    public void TieneRol_DebeDevolverFalse_SiUsuarioNoTieneElRol()
    {
        // Arr
        var rolesDelUsuario = new List<string> { RolesConstantes.REVISOR_CATALOGO };
        var usuario = new Usuario(0,"Juan", "Perez", "juan@test.com", DateTime.Now.AddYears(-44), roles: rolesDelUsuario);

        // Act
        bool resultado = usuario.TieneRol(RolesConstantes.ADMINISTRADOR);

        // Assert
        Assert.IsFalse(resultado);
    }

    [TestMethod]
    public void TieneRol_DebeDevolverFalse_SiListaDeRolesEstaVacia()
    {
        // Arrange
        var usuario = new Usuario(0,"Juan", "Perez", "juan@test.com", DateTime.Now.AddYears(-44), roles: new List<string>());

        // Act
        bool resultado = usuario.TieneRol(RolesConstantes.ADMINISTRADOR);

        // Assert
        Assert.IsFalse(resultado);
    }

    [TestMethod]
    public void EsAdministrador_DebeSerTrue_CuandoUsuarioTieneRolAdmin()
    {
        // Arrange
        var rolesDelUsuario = new List<string> { RolesConstantes.ADMINISTRADOR, RolesConstantes.REVISOR_CATALOGO };
        var usuario = new Usuario(0,"Juan", "Perez", "juan@test.com", DateTime.Now.AddYears(-44), roles: rolesDelUsuario);

        // Act y Assert
        Assert.IsTrue(usuario.EsAdministrador);
    }

    [TestMethod]
    public void EsRevisor_DebeSerTrue_CuandoUsuarioTieneRolRevisor()
    {
        // Arrange
        var rolesDelUsuario = new List<string> { RolesConstantes.ADMINISTRADOR, RolesConstantes.REVISOR_CATALOGO };
        var usuario = new Usuario(0,"Juan", "Perez", "juan@test.com", DateTime.Now.AddYears(-44), roles: rolesDelUsuario);

        // Act y Assert
        Assert.IsTrue(usuario.EsRevisor);
    }
    
    [TestMethod]
    public void EsAdministrador_DebeSerFalse_CuandoUsuarioNoTieneRolAdmin()
    {
        // Arr
        var rolesDelUsuario = new List<string> {RolesConstantes.REVISOR_CATALOGO  };
        var usuario = new Usuario(0,"Juan", "Perez", "juan@test.com", DateTime.Now.AddYears(-44), roles: rolesDelUsuario);

        // Act y As 
        Assert.IsFalse(usuario.EsAdministrador);
    }
    
    
    
}