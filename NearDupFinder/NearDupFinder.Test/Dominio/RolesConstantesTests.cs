using NearDupFinder.Dominio.Entidades;

namespace NearDupFinder.Test.Dominio;

[TestClass]
public class RolesConstantesTests
{
    [TestMethod]
    public void Administrador_DebeTenerElValorCorrecto()
    {
        // Arr
        string valorEsperado = "Administrador";

        // Act
        string valorActual = RolesConstantes.ADMINISTRADOR;

        // Ass
        Assert.AreEqual(valorEsperado, valorActual, "El valor del rol Administrador ha cambiado.");
    }

    [TestMethod]
    public void RevisorCatalogo_DebeTenerElValorCorrecto()
    {
        // Arr
        string valorEsperado = "Revisor de Catálogo";

        // Act
        string valorActual = RolesConstantes.REVISOR_CATALOGO;

        // Ass
        Assert.AreEqual(valorEsperado, valorActual, "El valor del rol Revisor de Catálogo ha cambiado.");
    }

    [TestMethod]
    public void TodosLosRoles_DebeContenerLaCantidadCorrectaDeRoles()
    {
        // Arrange
        int cantidadEsperada = 2;

        // Act
        int cantidadActual = RolesConstantes.TodosLosRoles.Count;

        // Assert
        Assert.AreEqual(cantidadEsperada, cantidadActual, "La cantidad de roles en la lista TodosLosRoles no es la esperada.");
    }

    [TestMethod]
    public void TodosLosRoles_DebeIncluirElRolAdministrador()
    {
        // Arran
        var listaDeRoles = RolesConstantes.TodosLosRoles;

        // Act y Ass
        CollectionAssert.Contains(listaDeRoles, RolesConstantes.ADMINISTRADOR, "La lista de roles no contiene el rol Administrador.");
    }

    [TestMethod]
    public void TodosLosRoles_DebeIncluirElRolRevisorCatalogo()
    {
        // Arr
        var listaDeRoles = RolesConstantes.TodosLosRoles;

        // Act y Ass
        CollectionAssert.Contains(listaDeRoles, RolesConstantes.REVISOR_CATALOGO, "La lista de roles no contiene el rol Revisor de Catálogo.");
    }
}