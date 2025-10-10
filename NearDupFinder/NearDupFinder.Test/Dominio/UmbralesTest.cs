using NearDupFinder.Dominio;
using NearDupFinder.Dominio.Entidades;


namespace NearDupFinder.Test.Dominio;

[TestClass]
public class UmbralesTest
{
    [TestMethod]
    public void EvaluarItemsTest()
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
        var score = Umbrales.EvaluarItems(itemA,itemB);
        
        // Assert
        Assert.AreEqual("DUPLICADO SUGERIDO", score);
    }
    
    [TestMethod]
    public void EvaluarTest()
    {
        // Arrange
        double t_alert = 0.60;
        double score = 0.61;
        double score2 = 0.50;
        // Act
        var res = Umbrales.Evaluar(score);
        var res2 = Umbrales.Evaluar(score2);

        // Assert
        Assert.AreEqual("POSIBLE DUPLICADO", res);
        Assert.AreEqual("NO DUPLICADO", res2);

    }

    [TestMethod]
    public void EvaluarTest2()
    {
        //Arrange
        double t_dup = 0.75;
        double score = 0.76;
        double score2 = 0.74;
        double score3 = 0.59;
        //Act
        var res = Umbrales.Evaluar(score);
        var res2 = Umbrales.Evaluar(score2);
        var res3 = Umbrales.Evaluar(score3);
        //Assert
        Assert.AreEqual("DUPLICADO SUGERIDO", res);
        Assert.AreEqual("POSIBLE DUPLICADO", res2);
        Assert.AreEqual("NO DUPLICADO", res3);
    }
}

