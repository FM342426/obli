using NearDupFinder.Dominio;
using NearDupFinder.Dominio.Entidades;
using NearDupFinder.Infraestructura.Repositorios;

namespace ProyectoOb;

[TestClass]
public class DeteccionTests
{
    private Catalogo catalogoTest;

    private Item CrearItemValido(string marca = "", string modelo = "")
    {
        return new Item
        {
            Titulo = "Producto Test",
            Descripcion = "Descripción Test",
            Marca = marca,
            Modelo = modelo,
            Catalogo = catalogoTest
        };
    }
    
    
    [TestMethod]
    public void Deteccion_SeInicializaConEstadoPendienteYFecha()
    {
        var itemA = CrearItemValido();
        var itemB = CrearItemValido();

        var deteccion = new Deteccion(itemA, itemB);

        Assert.AreEqual("Pendiente", deteccion.Estado);
        Assert.IsTrue((DateTime.UtcNow - deteccion.FechaDeteccion).TotalSeconds < 2);
    }

    [TestMethod]
    public void JaccardTitulo_EsUnoCuandoLosTitulosSonIguales()
    {
        var itemA = CrearItemValido();
        
        var itemB = CrearItemValido();

        var deteccion = new Deteccion(itemA, itemB);
        Assert.AreEqual(1.0, deteccion.JaccardTitulo);
    }

    [TestMethod]
    public void MarcaEq_EsUnoCuandoLasMarcasCoinciden()
    {
        var itemA = CrearItemValido("Apple");
        var itemB = CrearItemValido("Apple");

        var deteccion = new Deteccion(itemA, itemB);
        
      Assert.AreEqual(1, deteccion.MarcaEq);
    }
    
    
    [TestMethod]
    public void JaccardDescripcion_EsUnoCuandoLasDescripcionesSonIguales()
    {
        var itemA = CrearItemValido();
        var itemB = CrearItemValido();

        var deteccion = new Deteccion(itemA, itemB);
        Assert.AreEqual(1.0, deteccion.JaccardDescripcion);
    }

    [TestMethod]
    public void ModeloEq_EsUnoCuandoLasMarcasCoinciden()
    {
        var itemA = CrearItemValido("", "Galaxy S24 Ultra");
        var itemB = CrearItemValido("", "Galaxy S24 Ultra");

        var deteccion = new Deteccion(itemA, itemB);
        Assert.AreEqual(1.0, deteccion.ModeloEq);
    }

    [TestMethod]
    public void Score_EsAltoCuandoLosItemsSonIdenticos()
    {
        var itemA = CrearItemValido("", "Galaxy S24 Ultra");
        var itemB = CrearItemValido("", "Galaxy S24 Ultra");

        var deteccion = new Deteccion(itemA, itemB);

        Assert.IsTrue(deteccion.Score >= 0.75);
    }
    
    [TestMethod]
    public void TokensCompartidosTitulo_ContieneTokensEnComun()
    {
        var itemA = CrearItemValido("", "");
        var itemB = CrearItemValido("", "");

        var deteccion = new Deteccion(itemA, itemB);

        CollectionAssert.Contains(deteccion.TokensCompartidosTitulo, "producto");
        CollectionAssert.Contains(deteccion.TokensCompartidosTitulo, "test");
    }

    [TestMethod]
    public void TokensCompartidosDescripcion_ContieneTokensEnComun()
    {
        var itemA = CrearItemValido("", "");
        var itemB = CrearItemValido("", "");

        var deteccion = new Deteccion(itemA, itemB);

        CollectionAssert.Contains(deteccion.TokensCompartidosDescripcion, "descripcion");
        CollectionAssert.Contains(deteccion.TokensCompartidosDescripcion, "test");
    }

    [TestMethod]
    public void Confirmar_CambiaEstadoAConfirmado()
    {
        var itemA = CrearItemValido("", "");
        var itemB = CrearItemValido("", "");
        
        var deteccion = new Deteccion(itemA, itemB);
        deteccion.Confirmar();
        Assert.AreEqual("Confirmado", deteccion.Estado);
    }
    [TestMethod]
    public void Descartar_CambiaEstadoADescartado()
    {
        var itemA = CrearItemValido("", "");
        var itemB = CrearItemValido("", "");
        
        var deteccion = new Deteccion(itemA, itemB);
        deteccion.Descartar();
        Assert.AreEqual("Descartado", deteccion.Estado);
    }

    [TestMethod]
    public void  ObtenerIdsOrdenados_RetornaIdsEnOrdenAscendente()
    {
        var itemA = new Item
        {
            Id = 1, 
            Titulo = "Item A",
            Descripcion = "Descripción A", 
            Catalogo = catalogoTest
        };

        var itemB = new Item
        {
            Id = 2,   
            Titulo = "Item B",
            Descripcion = "Descripción B",
            Catalogo = catalogoTest
        };

        var deteccion = new Deteccion(itemA, itemB);

        
        Assert.AreEqual(1, deteccion.IdMenor); 
        Assert.AreEqual(2, deteccion.IdMayor); 
    }
}

