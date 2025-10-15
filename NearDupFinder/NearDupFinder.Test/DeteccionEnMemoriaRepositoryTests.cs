using NearDupFinder.Dominio;
using NearDupFinder.Dominio.Entidades;
using NearDupFinder.Infraestructura.Repositorios;

namespace ProyectoOb;

[TestClass]
public class DeteccionEnMemoriaRepositoryTests
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
    public async Task crearDeteccion()
    {
        DeteccionEnMemoriaRepository deteccionEnMemoriaRepository = new DeteccionEnMemoriaRepository();
        Deteccion deteccion = new Deteccion(ItemA(), ItemB());
        await deteccionEnMemoriaRepository.AgregarDeteccion(deteccion);
        var lista = await deteccionEnMemoriaRepository.ObtenerDeteccionesPendientes();

        Assert.AreEqual(1, lista.Count);
        Assert.AreEqual(deteccion, lista[0]);
    }

    [TestMethod]
    public async Task obtenerDeteccionPorId_devuelveLaDeteccion()
    {
        DeteccionEnMemoriaRepository deteccionEnMemoriaRepository = new DeteccionEnMemoriaRepository();
        Deteccion deteccion = new Deteccion(ItemA(), ItemB());
        await deteccionEnMemoriaRepository.AgregarDeteccion(deteccion);
        var obt = await deteccionEnMemoriaRepository.ObtenerDeteccionPorId(deteccion.Id);
        Assert.AreEqual(deteccion, obt);

    }

    [TestMethod]
    public void confirmarDeteccion_llamaADeteccionConfirmar()
    {
        DeteccionEnMemoriaRepository deteccionEnMemoriaRepository = new DeteccionEnMemoriaRepository();
        Deteccion deteccion = new Deteccion(ItemA(), ItemB());
        deteccionEnMemoriaRepository.AgregarDeteccion(deteccion);
        deteccionEnMemoriaRepository.ConfirmarDeteccion(deteccion.Id);
        Assert.AreEqual("Confirmado", deteccion.Estado);

    }
    [TestMethod]
    public void DescartarDeteccion_llamaADeteccionDescartar()
    {
        DeteccionEnMemoriaRepository deteccionEnMemoriaRepository = new DeteccionEnMemoriaRepository();
        Deteccion deteccion = new Deteccion(ItemA(), ItemB());
        deteccionEnMemoriaRepository.AgregarDeteccion(deteccion);
        deteccionEnMemoriaRepository.DescartarDeteccion(deteccion.Id);
        Assert.AreEqual("Descartado", deteccion.Estado);

    }
    [TestMethod]
    
    public async Task TieneDeteccionesPendientesTest()
    {
        DeteccionEnMemoriaRepository deteccionEnMemoriaRepository = new DeteccionEnMemoriaRepository();
        Deteccion deteccion = new Deteccion(ItemA(), ItemB());
        await  deteccionEnMemoriaRepository.AgregarDeteccion(deteccion);
        var tieneDetecciones = await deteccionEnMemoriaRepository.TieneDeteccionesPendientes(ItemA().Id);
        Assert.AreEqual(true,  tieneDetecciones);
    }
}




