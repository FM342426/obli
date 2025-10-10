using NearDupFinder.Dominio;
using NearDupFinder.Dominio.Entidades;
namespace NearDupFinder.Test.Dominio;

[TestClass]
public class FusionadorItemsTest
{
    [TestMethod]
    public void FusionarCanonicoConMarcaVaciaCompletaConMarca()
    {
        // Arrange
        var catalogo = new Catalogo { Id = 1, Titulo = "Test Catalogo" };
            
        var canonico = new Item
        {
            Id = 1,
            Titulo = "iPhone 13",
            Descripcion = "Smartphone Apple con características extendidas",
            Marca = "", 
            Modelo = "",
            Categoria = "",
            Catalogo = catalogo
        };
            
        var miembro = new Item
        {
            Id = 2,
            Titulo = "iPhone",
            Descripcion = "Smartphone",
            Marca = "Apple",
            Modelo = "A2482",
            Categoria = "Smartphones",
            Catalogo = catalogo
        };
            
        var miembros = new List<Item> { canonico, miembro };
            
        // Act
        FusionadorItems.Fusionar(canonico, miembros);
            
        // Assert
        Assert.AreEqual("Apple", canonico.Marca);
        Assert.AreEqual("A2482", canonico.Modelo);
        Assert.AreEqual("Smartphones", canonico.Categoria);
    }
    [TestMethod]
    public void FusionarTodosLosCamposDelCanonicoLlenosNoModificaCanonico()
    {
        // Arrange
        var catalogo = new Catalogo { Id = 1, Titulo = "Test Catalogo" };
            
        var canonico = new Item
        {
            Id = 1, 
            Titulo = "iPhone 13",
            Descripcion = "Smartphone Apple completo",
            Marca = "Apple",
            Modelo = "A2482",
            Categoria = "Smartphones",
            Catalogo = catalogo
        };
            
        var miembro = new Item
        {
            Id = 2, 
            Titulo = "iPhone",
            Descripcion = "Smartphone",
            Marca = "Apple Inc",
            Modelo = "A2482-B",
            Categoria = "Teléfonos móviles",
            Catalogo = catalogo
        };
            
        var miembros = new List<Item> { canonico, miembro };
            
        // Act
        FusionadorItems.Fusionar(canonico, miembros);
            
        // Assert
        Assert.AreEqual("Apple", canonico.Marca);
        Assert.AreEqual("A2482", canonico.Modelo);
        Assert.AreEqual("Smartphones", canonico.Categoria);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void FusionarConCanonicoNulo()
    {
        // Arrange
        Item canonico = null;
        var miembros = new List<Item> { new Item() };
    
        // Act
        FusionadorItems.Fusionar(canonico, miembros);
    
        
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void FusionarConMiembrosNulos()
    {
        // Arrange
        var canonico = new Item
        {
            Id = 1,
            Titulo = "Test",
            Marca = "Apple"
        };
        List<Item> miembros = null;
    
        // Act
        FusionadorItems.Fusionar(canonico, miembros);
    
        
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void FusionarConMiembrosVacios()
    {
        // Arrange
        var canonico = new Item
        {
            Id = 1,
            Titulo = "Test",
            Marca = "Apple"
        };
        var miembros = new List<Item>();
    
        // Act
        FusionadorItems.Fusionar(canonico, miembros);
    
        
    }

    [TestMethod]
    public void FusionarConMiembrosSinValoresValidosAsignaStringVacio()
    {
        // Arrange
        var catalogo = new Catalogo { Id = 1, Titulo = "Test Catalogo" };
    
        var canonico = new Item
        {
            Id = 1,
            Titulo = "Producto",
            Marca = "",
            Modelo = "",
            Categoria = "",
            Catalogo = catalogo
        };
    
        var miembro = new Item
        {
            Id = 2,
            Titulo = "Otro",
            Marca = null,
            Modelo = "",
            Categoria = null,
            Catalogo = catalogo
        };
    
        var miembros = new List<Item> { canonico, miembro };
    
        // Act
        FusionadorItems.Fusionar(canonico, miembros);
    
        // Assert
        Assert.AreEqual(string.Empty, canonico.Marca);
        Assert.AreEqual(string.Empty, canonico.Modelo);
        Assert.AreEqual(string.Empty, canonico.Categoria);
    }
    [TestMethod]
    public void FusionarCanonicoConCamposNulosCompletaConValores()
    {
        // Arrange
        var catalogo = new Catalogo { Id = 1, Titulo = "Test Catalogo" };
    
        var canonico = new Item
        {
            Id = 1,
            Titulo = "iPhone 13",
            Descripcion = "Smartphone Apple",
            Marca = null,  
            Modelo = null, 
            Categoria = null, 
            Catalogo = catalogo
        };
    
        var miembro = new Item
        {
            Id = 2,
            Titulo = "iPhone",
            Descripcion = "Smartphone",
            Marca = "Apple",
            Modelo = "A2482",
            Categoria = "Smartphones",
            Catalogo = catalogo
        };
    
        var miembros = new List<Item> { canonico, miembro };
    
        // Act
        FusionadorItems.Fusionar(canonico, miembros);
    
        // Assert
        Assert.AreEqual("Apple", canonico.Marca);
        Assert.AreEqual("A2482", canonico.Modelo);
        Assert.AreEqual("Smartphones", canonico.Categoria);
    }

}
