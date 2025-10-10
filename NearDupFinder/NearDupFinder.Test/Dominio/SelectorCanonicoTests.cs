using NearDupFinder.Dominio;
using NearDupFinder.Dominio.Entidades;

namespace NearDupFinder.Test.Dominio;


[TestClass]
public class SelectorCanonicoTests
{
     [TestMethod]
    public void SeleccionarCanonicoItemConDescripcionMasLarga()
    {
        // Arrange
        var catalogo = new Catalogo { Id = 1, Titulo = "Test Catalogo" };
        
        var item1 = new Item
        {
            Id = 1,
            Titulo = "iPhone 13",
            Descripcion = "Smartphone Apple",
            Catalogo = catalogo
        };
        
        var item2 = new Item
        {
            Id = 2,
            Titulo = "iPhone 13",
            Descripcion = "Smartphone Apple con 128GB de almacenamiento y pantalla OLED",
            Catalogo = catalogo
        };
        
        var items = new List<Item> { item1, item2 };
        
        // Act
        var canonico = SelectorCanonico.SeleccionarCanonico(items);
        
        // Assert
        Assert.AreEqual(item2, canonico);
    }
    
    [TestMethod]
    public void SeleccionarCanonicoDescripcionesIguales()
    {
        // Arrange
        var catalogo = new Catalogo { Id = 1, Titulo = "Test Catalogo" };

        var item1 = new Item
        {
            Id = 1,
            Titulo = "iPhone 13",
            Descripcion = "Smartphone Apple",
            Catalogo = catalogo
        };

        var item2 = new Item
        {
            Id = 2,
            Titulo = "iPhone 13 Pro Max 256GB",
            Descripcion = "Smartphone Apple",
            Catalogo = catalogo
        };

        var items = new List<Item> { item1, item2 };

        // Act
        var canonico = SelectorCanonico.SeleccionarCanonico(items);

        // Assert
        Assert.AreEqual(item2, canonico);
    }
    
    [TestMethod]
    public void SeleccionarCanonicoTituloYDescripcionIguales()
    {
        // Arrange
        var catalogo = new Catalogo { Id = 1, Titulo = "Test Catalogo" };

        var item1 = new Item
        {
            Id = 1,
            Titulo = "iPhone 13",
            Descripcion = "Smartphone Apple",
            Catalogo = catalogo
        };

        var item2 = new Item
        {
            Id = 2,
            Titulo = "iPhone 13",
            Descripcion = "Smartphone Apple",
            Catalogo = catalogo
        };

        var items = new List<Item> { item2, item1 }; 

        // Act
        var canonico = SelectorCanonico.SeleccionarCanonico(items);

        // Assert
        Assert.IsTrue(canonico.Id < item2.Id); 
        Assert.AreEqual(canonico, item1); 
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void SeleccionarCanonicoConListaNula()
    {
        // Arrange
        List<Item> items = null;
        
        // Act
        SelectorCanonico.SeleccionarCanonico(items);
        
        // Assert se maneja con ExpectedException
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void SeleccionarCanonicoConListaVacia()
    {
        // Arrange
        var items = new List<Item>();
        
        // Act
        SelectorCanonico.SeleccionarCanonico(items);
        
        // Assert se maneja con ExpectedException
    }

    [TestMethod]
    public void SeleccionarCanonicoConUnSoloItemRetornaEseItem()
    {
        // Arrange
        var catalogo = new Catalogo { Id = 1, Titulo = "Test Catalogo" };
        
        var item1 = new Item
        {
            Id = 1,
            Titulo = "iPhone 13",
            Descripcion = "Smartphone Apple",
            Catalogo = catalogo
        };
        
        var items = new List<Item> { item1 };
        
        // Act
        var canonico = SelectorCanonico.SeleccionarCanonico(items);
        
        // Assert
        Assert.AreEqual(item1, canonico);
    }

    [TestMethod]
    public void EsMejorCanonicoDescripcionMasCorta()
    {
        // Arrange
        var catalogo = new Catalogo { Id = 1, Titulo = "Test Catalogo" };
        
        var item1 = new Item
        {
            Id = 1,
            Titulo = "iPhone 13",
            Descripcion = "Smartphone Apple con muchas características",
            Catalogo = catalogo
        };
        
        var item2 = new Item
        {
            Id = 2,
            Titulo = "iPhone 13",
            Descripcion = "Smartphone",
            Catalogo = catalogo
        };
        
        var items = new List<Item> { item1, item2 };
        
        // Act
        var canonico = SelectorCanonico.SeleccionarCanonico(items);
        
        // Assert
        Assert.AreEqual(item1, canonico);
    }

    [TestMethod]
    public void EsMejorCanonicoTituloMasCorto()
    {
        // Arrange
        var catalogo = new Catalogo { Id = 1, Titulo = "Test Catalogo" };
        
        var item1 = new Item
        {
            Id = 1,
            Titulo = "iPhone 13 Pro",
            Descripcion = "Smartphone Apple",
            Catalogo = catalogo
        };
        
        var item2 = new Item
        {
            Id = 2,
            Titulo = "iPhone",
            Descripcion = "Smartphone Apple",
            Catalogo = catalogo
        };
        
        var items = new List<Item> { item1, item2 };
        
        // Act
        var canonico = SelectorCanonico.SeleccionarCanonico(items);
        
        // Assert
        Assert.AreEqual(item1, canonico);
    }

    [TestMethod]
    public void EsMejorCanonicoIdMayor()
    {
        // Arrange
        var catalogo = new Catalogo { Id = 1, Titulo = "Test Catalogo" };
        
        var item1 = new Item
        {
            Id = 5,
            Titulo = "iPhone 13",
            Descripcion = "Smartphone Apple",
            Catalogo = catalogo
        };
        
        var item2 = new Item
        {
            Id = 3,
            Titulo = "iPhone 13",
            Descripcion = "Smartphone Apple",
            Catalogo = catalogo
        };
        
        var items = new List<Item> { item1, item2 };
        
        // Act
        var canonico = SelectorCanonico.SeleccionarCanonico(items);
        
        // Assert
        Assert.AreEqual(item2, canonico);
    }

}

    
        //test faltantes
       