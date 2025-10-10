using NearDupFinder.Dominio;
using NearDupFinder.Dominio.Entidades;

namespace NearDupFinder.Test.Dominio;

[TestClass]
public class CalculadoraScoreTest
{
    [TestMethod]
    public void CalcularScoreCamposSonCero()
    {
        // Arrange
        double jaccardTitulo = 0.0;
        double jaccardDescripcion = 0.0;
        int marcaEq = 0;
        int modeloEq = 0;
        
        // Act
        var score = CalculadoraScore.Calcular(
            jaccardTitulo, jaccardDescripcion, marcaEq, modeloEq);
        
        // Assert
        Assert.AreEqual(0.0, score);
    }
    
    [TestMethod]
    public void CalcularScoreCamposSonUno()
    {
        // Arrange
        double jaccardTitulo = 1.0;
        double jaccardDescripcion = 1.0;
        int marcaEq = 1;
        int modeloEq = 1;
    
        // Act
        var score = CalculadoraScore.Calcular(
            jaccardTitulo, jaccardDescripcion, marcaEq, modeloEq);
    
        // Assert
        Assert.AreEqual(1.0, score);
    }
    [TestMethod]
    public void CalcularScoreConValoresDelEjemploDeLetra()
    {
        // Arrange
        double jaccardTitulo = 0.80;
        double jaccardDescripcion = 0.50;
        int marcaEq = 1;
        int modeloEq = 0;
    
        // Act
        var score = CalculadoraScore.Calcular(
            jaccardTitulo, jaccardDescripcion, marcaEq, modeloEq);
    
        // Assert
        Assert.AreEqual(0.635, score, 0.001);
    }
    [TestMethod] 
    public void CalcularScoreSoloTituloEs1()
    {
        // Arrange
        double jaccardTitulo = 1.0;
        double jaccardDescripcion = 0.0;
        int marcaEq = 0;
        int modeloEq = 0;
    
        // Act
        var score = CalculadoraScore.Calcular(
            jaccardTitulo, jaccardDescripcion, marcaEq, modeloEq);
    
        // Assert
        Assert.AreEqual(0.45, score, 0.001);
    }
    [TestMethod]
    public void CalcularScoreEntreItems()
    {
        // Arrange
        var itemA = new Item 
        { 
            Titulo = "iPhone 13",
            Descripcion = "Smartphone Apple",
            Marca = "Apple",
            Modelo = "13"
        };
    
        var itemB = new Item 
        { 
            Titulo = "iPhone 13",
            Descripcion = "Smartphone Apple", 
            Marca = "Apple",
            Modelo = "13"
        };
    
        // Act
        var score = CalculadoraScore.CalcularScoreEntreItems(itemA, itemB);
    
        // Assert
        Assert.AreEqual(1.0, score); 
    }
    
    [TestMethod]
    public void CalcularScoreEntreItemsCuandoItemAEsNulo()
    {
        // Arrange
        Item itemA = null;
        var itemB = new Item { Titulo = "Test" };
    
        // Act
        var score = CalculadoraScore.CalcularScoreEntreItems(itemA, itemB);
    
        // Assert
        Assert.AreEqual(0.0, score);
    }
    [TestMethod]
    public void CalcularScoreEntreItemsConItemsDiferentes()
    {
        // Arrange
        var itemA = new Item 
        { 
            Titulo = "iPhone 13 Pro",
            Descripcion = "Smartphone Apple premium",
            Marca = "Apple",
            Modelo = "13 Pro"
        };
    
        var itemB = new Item 
        { 
            Titulo = "iPhone 13",
            Descripcion = "Telefono Apple",
            Marca = "Apple",
            Modelo = "13"
        };
    
        // Act
        var score = CalculadoraScore.CalcularScoreEntreItems(itemA, itemB);
    
        // Assert
        Assert.IsTrue(score > 0.3 && score < 0.8);
        
    }
    
}