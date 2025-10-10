using NearDupFinder.Dominio;
using NearDupFinder.Dominio.Entidades;
using NearDupFinder.Infraestructura.Repositorios;

namespace ProyectoOb;

[TestClass]
public class DeteccionRepositorioTests
{
    private Catalogo catalogoTest;

    private Item ItemA()
    {
        return new Item
        {
            Titulo = "Producto Test",
            Descripcion = "Descripción Test",
            Marca = "Marca test",
            Modelo = "Modelo test",
            Catalogo = catalogoTest
        };
    }

    private Item ItemB()
    {
        return new Item
        {
            Titulo = "Producto Test",
            Descripcion = "Descripción Test",
            Marca = "Marca test",
            Modelo = "Modelo test",
            Catalogo = catalogoTest
        };
    }

    [TestMethod]
    public void crearDeteccion()
    {
        DeteccionRepositorio deteccionRepositorio = new DeteccionRepositorio();
        Deteccion deteccion = new Deteccion(ItemA(), ItemB());
        deteccionRepositorio.AgregarDeteccion(deteccion);
        var lista = deteccionRepositorio.ObtenerDeteccionesPendientes();

        Assert.AreEqual(1, deteccionRepositorio.ObtenerDeteccionesPendientes().Count);
        Assert.AreEqual(deteccion, lista[0]);
    }

    [TestMethod]
    public void obtenerDeteccionPorId_devuelveLaDeteccion()
    {
        DeteccionRepositorio deteccionRepositorio = new DeteccionRepositorio();
        Deteccion deteccion = new Deteccion(ItemA(), ItemB());
        deteccionRepositorio.AgregarDeteccion(deteccion);
        Assert.AreEqual(deteccion, deteccionRepositorio.ObtenerDeteccionPorId(deteccion.Id));

    }

    [TestMethod]
    public void confirmarDeteccion_llamaADeteccionConfirmar()
    {
        DeteccionRepositorio deteccionRepositorio = new DeteccionRepositorio();
        Deteccion deteccion = new Deteccion(ItemA(), ItemB());
        deteccionRepositorio.AgregarDeteccion(deteccion);
        deteccionRepositorio.ConfirmarDeteccion(deteccion.Id);
        Assert.AreEqual("Confirmado", deteccion.Estado);

    }
    [TestMethod]
    public void DescartarDeteccion_llamaADeteccionDescartar()
    {
        DeteccionRepositorio deteccionRepositorio = new DeteccionRepositorio();
        Deteccion deteccion = new Deteccion(ItemA(), ItemB());
        deteccionRepositorio.AgregarDeteccion(deteccion);
        deteccionRepositorio.DescartarDeteccion(deteccion.Id);
        Assert.AreEqual("Descartado", deteccion.Estado);

    }
    [TestMethod]
    
    public void TieneDeteccionesPendientesTest()
    {
        DeteccionRepositorio deteccionRepositorio = new DeteccionRepositorio();
        Deteccion deteccion = new Deteccion(ItemA(), ItemB());
        deteccionRepositorio.AgregarDeteccion(deteccion);
        Assert.AreEqual(true,  deteccionRepositorio.TieneDeteccionesPendientes(ItemA().Id));
    }
}




