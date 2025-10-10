
using NearDupFinder.Dominio;
using NearDupFinder.Dominio.Exceptions;

namespace ProyectoOb;

[TestClass]
public class CatalogoRepositorioTest
{
    [TestMethod]
    public void crearCatalogo()
    {
        CatalogoRepositorio catalogoRepositorio = new CatalogoRepositorio();
        CatalogoProductos nuevoCatalogoProducto = new CatalogoProductos("titulo", "descripcion");
        catalogoRepositorio.addCatalogoProducto(nuevoCatalogoProducto);
        var lista = catalogoRepositorio.GetAll();
        Assert.AreEqual(1, catalogoRepositorio.GetAll().Count);
        Assert.AreEqual(nuevoCatalogoProducto, lista[0]);
    }

    [TestMethod]
    public void AgregaCatalogoNull_ArgumentNullException()
    {
        CatalogoRepositorio catalogoRepositorio = new CatalogoRepositorio();
        ArgumentNullException exception = Assert.ThrowsException<ArgumentNullException>(() =>
        {
            catalogoRepositorio.addCatalogoProducto(null);
        });
        Assert.AreEqual("Value cannot be null. (Parameter 'catalogoProductos')", exception.Message);
    }
    [TestMethod]
    public void RemoveCatalogoNoExiste_CatalogoProductosException()
    {
        CatalogoRepositorio catalogoRepositorio = new CatalogoRepositorio();
        CatalogoProductos nuevoCatalogoProducto = new CatalogoProductos("titulo", "descripcion");
        CatalogoProductosException exception = Assert.ThrowsException<CatalogoProductosException>(() =>
        {
            catalogoRepositorio.removeCatalogoProducto(nuevoCatalogoProducto);
        });
        Assert.AreEqual("El catálogo no existe en la lista.", exception.Message);
    }
    [TestMethod]
    public void RemoveCatalogoExiste()
    {
        CatalogoRepositorio catalogoRepositorio = new CatalogoRepositorio();
        CatalogoProductos nuevoCatalogoProducto = new CatalogoProductos("titulo", "descripcion");
        catalogoRepositorio.addCatalogoProducto(nuevoCatalogoProducto);
        catalogoRepositorio.removeCatalogoProducto(nuevoCatalogoProducto);
        var lista = catalogoRepositorio.GetAll();
        Assert.AreEqual(0, lista.Count);
    }
    [TestMethod]
    public void UpdateCatalogo_ValoresValidos_DeberiaActualizar()
    {
        CatalogoRepositorio catalogoRepositorio = new CatalogoRepositorio();
        CatalogoProductos catalogo = new CatalogoProductos("titulo", "descripcion");
        catalogoRepositorio.addCatalogoProducto(catalogo);

        catalogoRepositorio.Update(catalogo,"titulo nuevo" ,"descripcion nueva" );
        
        Assert.AreEqual("titulo nuevo", catalogoRepositorio.GetAll()[0].Titulo);
        Assert.AreEqual("descripcion nueva", catalogoRepositorio.GetAll()[0].Descripcion);
    }
}