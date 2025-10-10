namespace NearDupFinder.Test.Dominio;
using NearDupFinder.Dominio.Entidades;

[TestClass]
public class ComparadorDeItemsTest
{
    #region titulo
    
    [TestMethod]
    public void CalcularJaccardTituloAmbosTitulosVacios()
    {
        // Arrange
        var tituloA = "";
        var tituloB = "";
        
        // Act
        var resultado = ComparadorDeItems.CalcularJaccardTitulo(tituloA, tituloB);
        
        // Assert
        Assert.AreEqual(0.0, resultado);
    }
    
    [TestMethod]
    public void CalcularJaccardTitulosIdentico()
    {
        // Arrange
        var tituloA = "iPhone 13 Pro";
        var tituloB = "iPhone 13 Pro";
    
        // Act
        var resultado = ComparadorDeItems.CalcularJaccardTitulo(tituloA, tituloB);
    
        // Assert
        Assert.AreEqual(1.0, resultado);
    }
    
    [TestMethod]
    public void CalcularJaccardTitulosSimilaresNoIgual()
    {
        // Arrange
        var tituloA = "iPhone 13 Pro";
        var tituloB = "iPhone 13 Max";
        
    
        // Act
        var resultado = ComparadorDeItems.CalcularJaccardTitulo(tituloA, tituloB);
    
        // Assert
        Assert.AreEqual(0.5, resultado, 0.001);
    }
    
    //nuevos test
    [TestMethod]
    public void CalcularJaccardSoloUnTituloVacio()
    {
        // Arrange
        var tituloA = "";
        var tituloB = "iPhone 13";
    
        // Act
        var resultado = ComparadorDeItems.CalcularJaccardTitulo(tituloA, tituloB);
        
        Assert.AreEqual(0.0, resultado);
    }
    [TestMethod]
    public void CalcularJaccardTitulosSonCaracteresEspeciales()
    {
        // Arrange
        var tituloA = "!!!";
        var tituloB = "###";
    
        // Act
        var resultado = ComparadorDeItems.CalcularJaccardTitulo(tituloA, tituloB);
    
        // Assert
        Assert.AreEqual(0.0, resultado);
    }

    [TestMethod]
    public void CalcularJaccardTitulosSonSoloLetras()
    {
        // Arrange
        var tituloA = "a b c";
        var tituloB = "x y z";
        
    
        // Act
        var resultado = ComparadorDeItems.CalcularJaccardTitulo(tituloA, tituloB);
    
        // Assert
        Assert.AreEqual(0.0, resultado);
    }
    [TestMethod]
    public void CalcularJaccardTitulosDiferentes()
    {
        // Arrange
        var tituloA = "Laptop Dell";
        var tituloB = "Mouse Gamer";
        
    
        // Act
        var resultado = ComparadorDeItems.CalcularJaccardTitulo(tituloA, tituloB);
    
        // Assert
        Assert.AreEqual(0.0, resultado);
    }
    #endregion
    #region descripciones
    [TestMethod]
    public void CalcularJaccardDescripcionesSimilares()
    {
        // Arrange
        var descripcionA = "Notebook ultraliviana con procesador";
        var descripcionB = "Notebook gamer con tarjeta";
        
    
        // Act
        var resultado = ComparadorDeItems.CalcularJaccardDescripcion(descripcionA, descripcionB);
    
        // Assert
        Assert.AreEqual(0.333, resultado, 0.001);
    }
    #endregion
    #region marcaEq
    [TestMethod]
    public void CalcularMarcaEq_MarcasIguales()
    {
        // Arrange
        var marcaA = "Apple";
        var marcaB = "APPLE";
    
        // Act
        var resultado = ComparadorDeItems.CalcularMarcaEq(marcaA, marcaB);
    
        // Assert
        Assert.AreEqual(1, resultado);
    }
    
    [TestMethod]
    public void CalcularMarcaEq_MarcaEsVacia()
    {
        // Arrange
        var marcaA = "Samsung";
        var marcaB = "";
    
        // Act
        var resultado = ComparadorDeItems.CalcularMarcaEq(marcaA, marcaB);
    
        // Assert
        Assert.AreEqual(0, resultado);
    }

    [TestMethod]
    public void CalcularMarcaEq_MarcasSonDiferentes()
    {
        // Arrange
        var marcaA = "Samsung";
        var marcaB = "Apple";
    
        // Act
        var resultado = ComparadorDeItems.CalcularMarcaEq(marcaA, marcaB);
    
        // Assert
        Assert.AreEqual(0, resultado);
    }
    #endregion
    #region modeloEq
    [TestMethod]
    public void CalcularModeloEq_ModelosIguales()
    {
        // Arrange
        var modeloA = "XPS-13";
        var modeloB = "XPS 13";
    
        // Act
        var resultado = ComparadorDeItems.CalcularModeloEq(modeloA, modeloB);
    
        // Assert
        Assert.AreEqual(1, resultado);
    }
    #endregion
}