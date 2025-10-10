using NearDupFinder.Dominio.Entidades;

namespace NearDupFinder.Test.Dominio;

[TestClass]
public class TokenizerTest
{
    [TestMethod]
    public void Tokenize_DebeSepararElTextoPorEspacios()
    {
        // Arrange
        var textoNormalizado = "hola mundo de prueba";
        var esperado = new List<string> { "hola", "mundo", "de", "prueba" };

        // Act
        var resultado = Tokenizer.Tokenize(textoNormalizado);

        // Assert
        CollectionAssert.AreEqual(esperado, resultado);
    }
    
    [TestMethod]
    public void Tokenize_DebeEliminarTokensDeLongitudUno()
    {
        // Arrange
        var textoNormalizado = "a uno dos tres cuatro cinco seis";
        var esperado = new List<string> { "uno", "dos", "tres", "cuatro", "cinco" ,"seis"};

        // Act
        var resultado = Tokenizer.Tokenize(textoNormalizado);

        // Assert
        CollectionAssert.AreEqual(esperado, resultado);
    }

    
    [TestMethod]
    public void Tokenize_DebeManejarStringVacioCorrectamente()
    {
        // Arrange
        var textoVacio = "";
        var esperado = new List<string>();

        // Act
        var resultado = Tokenizer.Tokenize(textoVacio);

        // Assert
        CollectionAssert.AreEqual(esperado, resultado);
    }
    
}