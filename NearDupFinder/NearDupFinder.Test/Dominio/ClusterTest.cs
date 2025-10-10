using NearDupFinder.Dominio;
using NearDupFinder.Dominio.Entidades;

namespace NearDupFinder.Test.Dominio;

[TestClass]
public class ClusterTest
{
    [TestMethod]
    public void CrearCluster_ConDosItems_SeleccionaCanonicoCorrectamente()
    {
        // Arrange
        var catalogo = new Catalogo { Id = 1, Titulo = "Test Catalogo" };
            
        var item1 = new Item
        {
            Id = 1,
            Titulo = "iPhone 13",
            Descripcion = "Smartphone",
            Catalogo = catalogo
        };
            
        var item2 = new Item
        {
            Id = 2,
            Titulo = "iPhone 13",
            Descripcion = "Smartphone Apple con muchas características",
            Catalogo = catalogo
        };
            
        // Act
        var cluster = new Cluster(new List<Item> { item1, item2 });
            
        // Assert
        Assert.AreEqual(item2, cluster.Canonico);
        Assert.AreEqual(2, cluster.Miembros.Count);
        Assert.IsTrue(cluster.Miembros.Contains(item1));
        Assert.IsTrue(cluster.Miembros.Contains(item2));
    }
    //test faltantes
    [TestMethod]
[ExpectedException(typeof(ArgumentException))]
public void CrearClusterConListaNula()
{
    // Arrange
    List<Item> items = null;
    
    // Act
    new Cluster(items);
    
}

[TestMethod]
[ExpectedException(typeof(ArgumentException))]
public void CrearClusterConListaVacia()
{
    // Arrange
    var items = new List<Item>();
    
    // Act
    new Cluster(items);
    
    
}

[TestMethod]
[ExpectedException(typeof(ArgumentException))]
public void CrearClusterConUnSoloItem()
{
    // Arrange
    var catalogo = new Catalogo { Id = 1, Titulo = "Test Catalogo" };
    
    var item1 = new Item
    {
        Id = 1,
        Titulo = "iPhone 13",
        Descripcion = "Smartphone",
        Catalogo = catalogo
    };
    
    var items = new List<Item> { item1 };
    
    // Act
    new Cluster(items);
    
    
}

[TestMethod]
[ExpectedException(typeof(ArgumentException))]
public void CrearClusterConItemsDeDiferentesCatalogos()
{
    // Arrange
    var catalogo1 = new Catalogo { Id = 1, Titulo = "Catalogo 1" };
    var catalogo2 = new Catalogo { Id = 2, Titulo = "Catalogo 2" };
    
    var item1 = new Item
    {
        Id = 1,
        Titulo = "iPhone 13",
        Descripcion = "Smartphone",
        Catalogo = catalogo1
    };
    
    var item2 = new Item
    {
        Id = 2,
        Titulo = "Samsung Galaxy",
        Descripcion = "Smartphone Android",
        Catalogo = catalogo2
    };
    
    var items = new List<Item> { item1, item2 };
    
    // Act
    new Cluster(items);
    
   
}

[TestMethod]
public void CrearClusterAsignaCatalogoIdCorrectamente()
{
    // Arrange
    var catalogo = new Catalogo { Id = 5, Titulo = "Test Catalogo" };
    
    var item1 = new Item
    {
        Id = 1,
        Titulo = "iPhone 13",
        Descripcion = "Smartphone",
        Catalogo = catalogo
    };
    
    var item2 = new Item
    {
        Id = 2,
        Titulo = "iPhone 13 Pro",
        Descripcion = "Smartphone Premium",
        Catalogo = catalogo
    };
    
    // Act
    var cluster = new Cluster(new List<Item> { item1, item2 });
    
    // Assert
    Assert.AreEqual(5, cluster.CatalogoId);
}
    
}
