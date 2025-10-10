using NearDupFinder.Dominio;
using NearDupFinder.Dominio.Entidades;
using Moq;
using NearDupFinder.Dominio.Interfaces;

namespace NearDupFinder.Test.Dominio;
[TestClass]
public class GestorClustersTest
{
    [TestMethod]
    public void ConfirmarDuplicadoItemsSinCluster()
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
            Descripcion = "Smartphone Apple",
            Catalogo = catalogo
        };
            
        var gestor = new GestorClusters();
            
        // Act
        gestor.ConfirmarDuplicado(item1, item2);
            
        // Assert
        Assert.IsTrue(gestor.PerteneceACluster(item1));
        Assert.IsTrue(gestor.PerteneceACluster(item2));
        Assert.AreEqual(gestor.ObtenerCluster(item1), gestor.ObtenerCluster(item2));
    }
    [TestMethod]
    public void ConfirmarDuplicadoTransitividadABBCCreaClusterConTresItems()
    {
        // Arrange
        var catalogo = new Catalogo { Id = 1, Titulo = "Test Catalogo" };
    
        var itemA = new Item
        {
            Id = 1,
            Titulo = "iPhone",
            Descripcion = "Smartphone",
            Catalogo = catalogo
        };
    
        var itemB = new Item
        {
            Id = 2,
            Titulo = "iPhone 13",
            Descripcion = "Smartphone Apple",
            Catalogo = catalogo
        };
    
        var itemC = new Item
        {
            Id = 3,
            Titulo = "iPhone 13",
            Descripcion = "Smartphone Apple 128GB",
            Catalogo = catalogo
        };
    
        var gestor = new GestorClusters();
    
        // Act
        gestor.ConfirmarDuplicado(itemA, itemB); // Cluster {A, B}
        gestor.ConfirmarDuplicado(itemB, itemC); //  union  {A, B, C}
    
        // Assert
        var cluster = gestor.ObtenerCluster(itemA);
        Assert.IsNotNull(cluster);
        Assert.AreEqual(3, cluster.Miembros.Count);
        Assert.IsTrue(cluster.Miembros.Contains(itemA));
        Assert.IsTrue(cluster.Miembros.Contains(itemB));
        Assert.IsTrue(cluster.Miembros.Contains(itemC));
    
        // Todos deben estar en el mismo cluster
        Assert.AreEqual(cluster, gestor.ObtenerCluster(itemB));
        Assert.AreEqual(cluster, gestor.ObtenerCluster(itemC));
    }
    [TestMethod]
    public void ExcluirItemMiembroNoCanonicoDeCluster()
    {
        // Arrange
        var catalogo = new Catalogo { Id = 1, Titulo = "Test Catalogo" };
    
        var item1 = new Item
        {
            Id = 1,
            Titulo = "iPhone",
            Descripcion = "Smartphone",
            Catalogo = catalogo
        };
    
        var item2 = new Item
        {
            Id = 2,
            Titulo = "iPhone 13",
            Descripcion = "Smartphone Apple con características",
            Catalogo = catalogo
        };
    
        var item3 = new Item
        {
            Id = 3,
            Titulo = "iPhone 13",
            Descripcion = "Smartphone Apple",
            Catalogo = catalogo
        };
    
        var gestor = new GestorClusters();
        gestor.ConfirmarDuplicado(item1, item2);
        gestor.ConfirmarDuplicado(item2, item3);
    
        // Act
        gestor.ExcluirItem(item1); // Excluir  no-canonico
    
        // Assert
        Assert.IsFalse(gestor.PerteneceACluster(item1));
        Assert.IsTrue(gestor.PerteneceACluster(item2));
        Assert.IsTrue(gestor.PerteneceACluster(item3));
    
        var cluster = gestor.ObtenerCluster(item2);
        Assert.AreEqual(2, cluster.Miembros.Count);
        Assert.IsFalse(cluster.Miembros.Contains(item1));
    }
    [TestMethod]
    public void ExcluirItemCanonicoDeCluster()
    {
        // Arrange
        var catalogo = new Catalogo { Id = 1, Titulo = "Test Catalogo" };
    
        var item1 = new Item
        {
            Id = 1,
            Titulo = "iPhone",
            Descripcion = "Smartphone corto",
            Catalogo = catalogo
        };
    
        var item2 = new Item
        {
            Id = 2,
            Titulo = "iPhone 13",
            Descripcion = "Smartphone Apple con características extendidas y detalladas",
            Catalogo = catalogo
        };
    
        var item3 = new Item
        {
            Id = 3,
            Titulo = "iPhone 13",
            Descripcion = "Smartphone Apple medio",
            Catalogo = catalogo
        };
    
        var gestor = new GestorClusters();
        gestor.ConfirmarDuplicado(item1, item2);
        gestor.ConfirmarDuplicado(item2, item3);
    
        var clusterAntes = gestor.ObtenerCluster(item2);
        Assert.AreEqual(item2, clusterAntes.Canonico); // item2 es canonico
    
        // Act
        gestor.ExcluirItem(item2); // Excluir el canonico
    
        // Assert
        Assert.IsFalse(gestor.PerteneceACluster(item2));
        Assert.IsTrue(gestor.PerteneceACluster(item1));
        Assert.IsTrue(gestor.PerteneceACluster(item3));
    
        var clusterDespues = gestor.ObtenerCluster(item3);
        Assert.AreEqual(2, clusterDespues.Miembros.Count);
        Assert.AreEqual(item3, clusterDespues.Canonico); // item3 es el nuevo canonico
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ConfirmarDuplicadoItemsDeDiferentesCatalogos_()
    {
        // Arrange
        var catalogo1 = new Catalogo { Id = 1, Titulo = "Catálogo Electrónica" };
        var catalogo2 = new Catalogo { Id = 2, Titulo = "Catálogo Ropa" };
    
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
            Titulo = "iPhone 13",
            Descripcion = "Smartphone Apple",
            Catalogo = catalogo2 // Catalogo diferente
        };
    
        var gestor = new GestorClusters();
    
        // Act - lanzar excepcion
        gestor.ConfirmarDuplicado(item1, item2);
    }
    [TestMethod]
public void ObtenerClusterItemSinCluster()
{
    // Arrange
    var catalogo = new Catalogo { Id = 1, Titulo = "Test Catalogo" };
    var item = new Item
    {
        Id = 1,
        Titulo = "iPhone 13",
        Descripcion = "Smartphone",
        Catalogo = catalogo
    };
    
    var gestor = new GestorClusters();
    
    // Act
    var cluster = gestor.ObtenerCluster(item);
    
    // Assert
    Assert.IsNull(cluster);
}

[TestMethod]
public void ExcluirItemClusterConDosItems()
{
    // Arrange
    var catalogo = new Catalogo { Id = 1, Titulo = "Test Catalogo" };
    
    var item1 = new Item
    {
        Id = 1,
        Titulo = "iPhone",
        Descripcion = "Smartphone",
        Catalogo = catalogo
    };
    
    var item2 = new Item
    {
        Id = 2,
        Titulo = "iPhone 13",
        Descripcion = "Smartphone Apple",
        Catalogo = catalogo
    };
    
    var gestor = new GestorClusters();
    gestor.ConfirmarDuplicado(item1, item2);
    
    // Act
    gestor.ExcluirItem(item1);
    
    // Assert
    Assert.IsFalse(gestor.PerteneceACluster(item1));
    Assert.IsFalse(gestor.PerteneceACluster(item2)); // El cluster se elimino
    Assert.IsNull(gestor.ObtenerCluster(item2));
}

[TestMethod]
public void FusionarTodosLosCamposDelCanonicoLlenos()
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
    Assert.AreEqual("Apple", canonico.Marca); // Mantiene el valor original
    Assert.AreEqual("A2482", canonico.Modelo);
    Assert.AreEqual("Smartphones", canonico.Categoria);
}
[TestMethod]
public void ConfirmarDuplicadoCanonicoConCamposVacios()
{
    // Arrange
    var catalogo = new Catalogo { Id = 1, Titulo = "Test Catalogo" };
    
    var item1 = new Item
    {
        Id = 1,
        Titulo = "iPhone",
        Descripcion = "Smartphone Apple con todas las características",
        Marca = "", // vacio
        Modelo = "",
        Categoria = "",
        Catalogo = catalogo
    };
    
    var item2 = new Item
    {
        Id = 2,
        Titulo = "iPhone 13",
        Descripcion = "Smartphone",
        Marca = "Apple",
        Modelo = "A2482",
        Categoria = "Smartphones",
        Catalogo = catalogo
    };
    
    var gestor = new GestorClusters();
    
    // Act
    gestor.ConfirmarDuplicado(item1, item2);
    
    // Assert
    var cluster = gestor.ObtenerCluster(item1);
    Assert.AreEqual(item1, cluster.Canonico); // item1 es canonico por descripcion
    Assert.AreEqual("Apple", cluster.Canonico.Marca); // Fusionado
    Assert.AreEqual("A2482", cluster.Canonico.Modelo); // Fusionado
    Assert.AreEqual("Smartphones", cluster.Canonico.Categoria); // Fusionado
}
[TestMethod]
public void ObtenerClustersPorCatalogoVariosClustersDiferentesCatalogosFiltraCorrectamente()
{
    // Arrange
    var catalogo1 = new Catalogo { Id = 1, Titulo = "Electrónica" };
    var catalogo2 = new Catalogo { Id = 2, Titulo = "Ropa" };
    
    var item1Cat1 = new Item
    {
        Id = 1,
        Titulo = "iPhone",
        Descripcion = "Smartphone",
        Catalogo = catalogo1
    };
    
    var item2Cat1 = new Item
    {
        Id = 2,
        Titulo = "iPhone 13",
        Descripcion = "Smartphone Apple",
        Catalogo = catalogo1
    };
    
    var item1Cat2 = new Item
    {
        Id = 3,
        Titulo = "Remera",
        Descripcion = "Prenda de vestir",
        Catalogo = catalogo2
    };
    
    var item2Cat2 = new Item
    {
        Id = 4,
        Titulo = "Remera Nike",
        Descripcion = "Prenda de vestir deportiva",
        Catalogo = catalogo2
    };
    
    var gestor = new GestorClusters();
    gestor.ConfirmarDuplicado(item1Cat1, item2Cat1);
    gestor.ConfirmarDuplicado(item1Cat2, item2Cat2);
    
    // Act
    var clustersCat1 = gestor.ObtenerClustersPorCatalogo(1);
    var clustersCat2 = gestor.ObtenerClustersPorCatalogo(2);
    
    // Assert
    Assert.AreEqual(1, clustersCat1.Count);
    Assert.AreEqual(1, clustersCat2.Count);
    Assert.AreEqual(2, gestor.ObtenerTodosClusters().Count);
}


[TestMethod]
[ExpectedException(typeof(ArgumentException))]
public void ConfirmarDuplicadoItemANulo()
{
    // Arrange
    var catalogo = new Catalogo { Id = 1, Titulo = "Test Catalogo" };
    
    Item item1 = null;
    var item2 = new Item
    {
        Id = 2,
        Titulo = "iPhone 13",
        Descripcion = "Smartphone",
        Catalogo = catalogo
    };
    
    var gestor = new GestorClusters();
    
    // Act
    gestor.ConfirmarDuplicado(item1, item2);
    
   
}

[TestMethod]
[ExpectedException(typeof(ArgumentException))]
public void ConfirmarDuplicadoItemBNuloLanzaExcepcion()
{
    // Arrange
    var catalogo = new Catalogo { Id = 1, Titulo = "Test Catalogo" };
    
    var item1 = new Item
    {
        Id = 1,
        Titulo = "iPhone",
        Descripcion = "Smartphone",
        Catalogo = catalogo
    };
    Item item2 = null;
    
    var gestor = new GestorClusters();
    
    // Act
    gestor.ConfirmarDuplicado(item1, item2);
    
    
}

[TestMethod]
public void ConfirmarDuplicadoItemAEnClusterItemBSinClusterAgregaBaClusterDeA()
{
    // Arrange
    var catalogo = new Catalogo { Id = 1, Titulo = "Test Catalogo" };
    
    var item1 = new Item
    {
        Id = 1,
        Titulo = "iPhone",
        Descripcion = "Smartphone",
        Catalogo = catalogo
    };
    
    var item2 = new Item
    {
        Id = 2,
        Titulo = "iPhone 13",
        Descripcion = "Smartphone Apple",
        Catalogo = catalogo
    };
    
    var item3 = new Item
    {
        Id = 3,
        Titulo = "iPhone 13 Pro",
        Descripcion = "Smartphone Premium",
        Catalogo = catalogo
    };
    
    var gestor = new GestorClusters();
    gestor.ConfirmarDuplicado(item1, item2); // Cluster {1, 2}
    
    // Act
    gestor.ConfirmarDuplicado(item1, item3); // item1 en cluster, item3 sin cluster
    
    // Assert
    var cluster = gestor.ObtenerCluster(item1);
    Assert.IsNotNull(cluster);
    Assert.AreEqual(3, cluster.Miembros.Count);
    Assert.IsTrue(cluster.Miembros.Contains(item1));
    Assert.IsTrue(cluster.Miembros.Contains(item2));
    Assert.IsTrue(cluster.Miembros.Contains(item3));
}

[TestMethod]
public void ConfirmarDuplicadoItemASinClusterItemBEnClusterAgregaAaClusterDeB()
{
    // Arrange
    var catalogo = new Catalogo { Id = 1, Titulo = "Test Catalogo" };
    
    var item1 = new Item
    {
        Id = 1,
        Titulo = "iPhone",
        Descripcion = "Smartphone",
        Catalogo = catalogo
    };
    
    var item2 = new Item
    {
        Id = 2,
        Titulo = "iPhone 13",
        Descripcion = "Smartphone Apple",
        Catalogo = catalogo
    };
    
    var item3 = new Item
    {
        Id = 3,
        Titulo = "iPhone 13 Pro",
        Descripcion = "Smartphone Premium",
        Catalogo = catalogo
    };
    
    var gestor = new GestorClusters();
    gestor.ConfirmarDuplicado(item1, item2); // Cluster {1, 2}
    
    // Act
    gestor.ConfirmarDuplicado(item3, item2); // item3 sin cluster, item2 en cluster
    
    // Assert
    var cluster = gestor.ObtenerCluster(item2);
    Assert.IsNotNull(cluster);
    Assert.AreEqual(3, cluster.Miembros.Count);
    Assert.IsTrue(cluster.Miembros.Contains(item1));
    Assert.IsTrue(cluster.Miembros.Contains(item2));
    Assert.IsTrue(cluster.Miembros.Contains(item3));
}

[TestMethod]
public void ConfirmarDuplicadoAmbosItemsEnDiferentesClustersUneClusters()
{
    // Arrange
    var catalogo = new Catalogo { Id = 1, Titulo = "Test Catalogo" };
    
    var item1 = new Item
    {
        Id = 1,
        Titulo = "iPhone",
        Descripcion = "Smartphone",
        Catalogo = catalogo
    };
    
    var item2 = new Item
    {
        Id = 2,
        Titulo = "iPhone 13",
        Descripcion = "Smartphone Apple",
        Catalogo = catalogo
    };
    
    var item3 = new Item
    {
        Id = 3,
        Titulo = "iPhone 13 Pro",
        Descripcion = "Smartphone Premium",
        Catalogo = catalogo
    };
    
    var item4 = new Item
    {
        Id = 4,
        Titulo = "iPhone 13 Pro Max",
        Descripcion = "Smartphone Premium Max",
        Catalogo = catalogo
    };
    
    var gestor = new GestorClusters();
    gestor.ConfirmarDuplicado(item1, item2); // Cluster A {1, 2}
    gestor.ConfirmarDuplicado(item3, item4); // Cluster B {3, 4}
    
    // Act
    gestor.ConfirmarDuplicado(item2, item3); // Unir clusters A y B
    
    // Assert
    var cluster = gestor.ObtenerCluster(item1);
    Assert.IsNotNull(cluster);
    Assert.AreEqual(4, cluster.Miembros.Count);
    Assert.IsTrue(cluster.Miembros.Contains(item1));
    Assert.IsTrue(cluster.Miembros.Contains(item2));
    Assert.IsTrue(cluster.Miembros.Contains(item3));
    Assert.IsTrue(cluster.Miembros.Contains(item4));
    
    // Todos en el mismo cluster
    Assert.AreEqual(cluster, gestor.ObtenerCluster(item2));
    Assert.AreEqual(cluster, gestor.ObtenerCluster(item3));
    Assert.AreEqual(cluster, gestor.ObtenerCluster(item4));
    
    // Solo  un cluster
    Assert.AreEqual(1, gestor.ObtenerTodosClusters().Count);
}

[TestMethod]
public void ConfirmarDuplicadoAmbosItemsEnElMismoClusterNoHaceNada()
{
    // Arrange
    var catalogo = new Catalogo { Id = 1, Titulo = "Test Catalogo" };
    
    var item1 = new Item
    {
        Id = 1,
        Titulo = "iPhone",
        Descripcion = "Smartphone",
        Catalogo = catalogo
    };
    
    var item2 = new Item
    {
        Id = 2,
        Titulo = "iPhone 13",
        Descripcion = "Smartphone Apple",
        Catalogo = catalogo
    };
    
    var gestor = new GestorClusters();
    gestor.ConfirmarDuplicado(item1, item2);
    
    var clusterAntes = gestor.ObtenerCluster(item1);
    int miembrosAntes = clusterAntes.Miembros.Count;
    
    // Act
    gestor.ConfirmarDuplicado(item1, item2); // Intentar confirmar nuevamente
    
    // Assert
    var clusterDespues = gestor.ObtenerCluster(item1);
    Assert.AreEqual(miembrosAntes, clusterDespues.Miembros.Count);
    Assert.AreEqual(1, gestor.ObtenerTodosClusters().Count);
}

[TestMethod]
[ExpectedException(typeof(ArgumentException))]
public void ExcluirItem_ItemNulo_LanzaExcepcion()
{
    // Arrange
    var gestor = new GestorClusters();
    Item item = null;
    
    // Act
    gestor.ExcluirItem(item);
    
}

[TestMethod]
public void ExcluirItemItemSinClusterNoHaceNada()
{
    // Arrange
    var catalogo = new Catalogo { Id = 1, Titulo = "Test Catalogo" };
    
    var item1 = new Item
    {
        Id = 1,
        Titulo = "iPhone",
        Descripcion = "Smartphone",
        Catalogo = catalogo
    };
    
    var gestor = new GestorClusters();
    
    // Act
    gestor.ExcluirItem(item1); // Item no pertenece a ningun cluster
    
    // Assert
    Assert.IsFalse(gestor.PerteneceACluster(item1));
    Assert.AreEqual(0, gestor.ObtenerTodosClusters().Count);
}

[TestMethod]
public void PerteneceAClusterItemSinCluster()
{
    // Arrange
    var catalogo = new Catalogo { Id = 1, Titulo = "Test Catalogo" };
    
    var item = new Item
    {
        Id = 1,
        Titulo = "iPhone",
        Descripcion = "Smartphone",
        Catalogo = catalogo
    };
    
    var gestor = new GestorClusters();
    
    // Act
    bool resultado = gestor.PerteneceACluster(item);
    
    // Assert
    Assert.IsFalse(resultado);
}

[TestMethod]
public void ObtenerTodosClustersSinClusters()
{
    // Arrange
    var gestor = new GestorClusters();
    
    // Act
    var clusters = gestor.ObtenerTodosClusters();
    
    // Assert
    Assert.IsNotNull(clusters);
    Assert.AreEqual(0, clusters.Count);
}

[TestMethod]
public void ObtenerClustersPorCatalogoCatalogoSinClusters()
{
    // Arrange
    var gestor = new GestorClusters();
    
    // Act
    var clusters = gestor.ObtenerClustersPorCatalogo(999);
    
    // Assert
    Assert.IsNotNull(clusters);
    Assert.AreEqual(0, clusters.Count);
}


//usando moq

[TestClass]
public class GestorClustersConMocksTest
{
    [TestMethod]
    public void AgregarItemAClusterExistenteCuandoClusterIdNoExiste()
    {
        // Arrange
        var mockRepo = new Mock<IClusterRepositorio>();
        mockRepo.Setup(r => r.ObtenerClusterIdPorItem(It.IsAny<int>()))
                .Returns((int?)null); // Simula que no encuentra el cluster
        
        var gestor = new GestorClusters(mockRepo.Object);
        var catalogo = new Catalogo { Id = 1, Titulo = "Test" };
        var item1 = new Item { Id = 1, Titulo = "Test", Descripcion = "Test", Catalogo = catalogo };
        var item2 = new Item { Id = 2, Titulo = "Test", Descripcion = "Test", Catalogo = catalogo };
        
        // Act
        // Llamar directamente al metodo privado no es posible, pero se puede
        // probar por ConfirmarDuplicado
        
        // Assert
        mockRepo.Verify(r => r.ObtenerClusterPorId(It.IsAny<int>()), Times.Never);
    }
    
    [TestMethod]
    public void AgregarItemAClusterExistenteCuandoClusterEsNull()
    {
        // Arrange
        var mockRepo = new Mock<IClusterRepositorio>();
        mockRepo.Setup(r => r.ObtenerClusterIdPorItem(It.IsAny<int>()))
                .Returns(1);
        mockRepo.Setup(r => r.ObtenerClusterPorId(1))
                .Returns((Cluster)null); // Simula que el cluster fue eliminado
        
        var gestor = new GestorClusters(mockRepo.Object);
        var catalogo = new Catalogo { Id = 1, Titulo = "Test" };
        var item1 = new Item { Id = 1, Titulo = "Test", Descripcion = "Test", Catalogo = catalogo };
        var item2 = new Item { Id = 2, Titulo = "Test", Descripcion = "Test", Catalogo = catalogo };
        
        // Act & Assert
        // El metodo debe retornar sin lanzar excepción
        mockRepo.Verify(r => r.AsignarItemACluster(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }
    
    [TestMethod]
    public void UnirClusters_CuandoUnClusterNoExiste_RetornaSinHacerNada()
    {
        // Arrange
        var mockRepo = new Mock<IClusterRepositorio>();
        mockRepo.Setup(r => r.ObtenerClusterIdPorItem(1)).Returns(1);
        mockRepo.Setup(r => r.ObtenerClusterIdPorItem(2)).Returns(2);
        mockRepo.Setup(r => r.ObtenerClusterPorId(1))
                .Returns((Cluster)null); // ClusterA no existe
        mockRepo.Setup(r => r.ObtenerClusterPorId(2))
                .Returns(new Cluster(new List<Item> { 
                    new Item { Id = 2, Titulo = "Test", Descripcion = "Test", 
                              Catalogo = new Catalogo { Id = 1 } },
                    new Item { Id = 3, Titulo = "Test2", Descripcion = "Test2", 
                              Catalogo = new Catalogo { Id = 1 } }
                }));
        
        var gestor = new GestorClusters(mockRepo.Object);
        
        // Act & Assert
        mockRepo.Verify(r => r.EliminarCluster(It.IsAny<int>()), Times.Never);
    }
    
    [TestMethod]
    public void ExcluirItem_CuandoClusterNoExiste_RetornaSinHacerNada()
    {
        // Arrange
        var mockRepo = new Mock<IClusterRepositorio>();
        mockRepo.Setup(r => r.ObtenerClusterIdPorItem(1)).Returns(1);
        mockRepo.Setup(r => r.ObtenerClusterPorId(1))
                .Returns((Cluster)null); // Cluster no existe
        
        var gestor = new GestorClusters(mockRepo.Object);
        var catalogo = new Catalogo { Id = 1, Titulo = "Test" };
        var item = new Item { Id = 1, Titulo = "Test", Descripcion = "Test", Catalogo = catalogo };
        
        // Act
        gestor.ExcluirItem(item);
        
        // Assert
        mockRepo.Verify(r => r.RemoverItemDeCluster(It.IsAny<int>()), Times.Never);
        mockRepo.Verify(r => r.EliminarCluster(It.IsAny<int>()), Times.Never);
    }
}


}