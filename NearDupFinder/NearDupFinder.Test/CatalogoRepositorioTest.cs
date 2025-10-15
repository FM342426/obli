using NearDupFinder.Dominio;
using NearDupFinder.Dominio.Exceptions;
using NearDupFinder.Infraestructura.Repositorios;

namespace ProyectoOb;

[TestClass]
public class CatalogoRepositorioTest
{
    [TestMethod]
    public async Task crearCatalogo()
    {
        CatalogoRepositorio catalogoRepositorio = new CatalogoRepositorio();
        Catalogo nuevoCatalogoProducto = new Catalogo("titulo", "descripcion");
        await catalogoRepositorio.AddCatalogo(nuevoCatalogoProducto);
        var lista = await catalogoRepositorio.GetAll();
        
        Assert.AreEqual(1, lista.Count);
        Assert.AreEqual(nuevoCatalogoProducto, lista[0]);
    }

    [TestMethod]
    public void AgregaCatalogoNull_ArgumentNullException()
    {
        CatalogoRepositorio catalogoRepositorio = new CatalogoRepositorio();
        ArgumentNullException exception = Assert.ThrowsException<ArgumentNullException>(() =>
        {
            catalogoRepositorio.AddCatalogo(null);
        });
        Assert.AreEqual("Value cannot be null. (Parameter 'catalogo')", exception.Message);
    }
    [TestMethod]
    public void RemoveCatalogoNoExiste_CatalogoProductosException()
    {
        CatalogoRepositorio catalogoRepositorio = new CatalogoRepositorio();
        Catalogo nuevoCatalogoProducto = new Catalogo("titulo", "descripcion");
        CatalogoException exception = Assert.ThrowsException<CatalogoException>(() =>
        {
            catalogoRepositorio.Remove(nuevoCatalogoProducto);
        });
        Assert.AreEqual("El catálogo no existe.", exception.Message);
    }
    [TestMethod]
    public async Task RemoveCatalogoExiste()
    {
        CatalogoRepositorio catalogoRepositorio = new CatalogoRepositorio();
        Catalogo nuevoCatalogoProducto = new Catalogo("titulo", "descripcion");
        await catalogoRepositorio.AddCatalogo(nuevoCatalogoProducto);
        await catalogoRepositorio.Remove(nuevoCatalogoProducto);
        var lista = await catalogoRepositorio.GetAll();
        Assert.AreEqual(0, lista.Count);
    }
    [TestMethod]
    public async Task UpdateCatalogo_ValoresValidos_DeberiaActualizar()
    {
        CatalogoRepositorio catalogoRepositorio = new CatalogoRepositorio();
        Catalogo catalogo = new Catalogo("titulo", "descripcion");
        
        await catalogoRepositorio.AddCatalogo(catalogo);
        catalogo.Actualizar("titulo nuevo","descripcion nueva");
        await catalogoRepositorio.Update(catalogo);

        var lista = await catalogoRepositorio.GetAll();
        
        Assert.AreEqual("titulo nuevo", lista[0].Titulo);
        Assert.AreEqual("descripcion nueva", lista[0].Descripcion);
    }
}