using NearDupFinder.Dominio.Entidades;
using NearDupFinder.Dominio.Exceptions;
using ProyectoOb;

namespace NearDupFinder.Test;
using NearDupFinder.Dominio;
using NearDupFinder.Dominio.Repositorios;
using NearDupFinder.Infraestructura.Repositorios;

[TestClass]
public class ItemRepositorioTest
{
    private IItemRepositorio repositorio;
    private Catalogo catalogoTest;
    private DeteccionRepositorio detecciones;

    
    [TestInitialize]
    public void Setup()
    {
        detecciones = new DeteccionRepositorio();
        repositorio = new ItemRepositorio(detecciones);
        catalogoTest = new Catalogo 
        { 
            Id = 1, 
            Titulo = "Catalogo Test",
            Descripcion = "Test"
        };
    }
    #region add
    
    [TestMethod]
    public void AddItemValido()
    {
        
        var item = new Item
        {
            Titulo = "Producto Test",
            Descripcion = "Descripcion Test",
            Catalogo = catalogoTest
        };

        Assert.AreEqual(0, item.Id); // 0 antes de agregar

        repositorio.Add(item);
        var itemAgregado = repositorio.GetById(item.Id);

        Assert.IsNotNull(itemAgregado);
        Assert.AreEqual(1, item.Id); //  1 despues de agregar
        Assert.AreEqual(item.Titulo, itemAgregado.Titulo);
    }

    [TestMethod]
    public void Add_DosItems_IdsConsecutivos()
    {
        var item1 = CrearItemValido();
        var item2 = CrearItemValido();
        item2.Titulo = "Producto 2";

        repositorio.Add(item1);
        repositorio.Add(item2);

        Assert.AreEqual(1, item1.Id);
        Assert.AreEqual(2, item2.Id);
        Assert.AreEqual(2, repositorio.GetAll().Count());
    }
    
    [TestMethod]
    public void Add_ItemDespuesDeDelete_ReutilizaIdsCorrectamente()
    {
        var item1 = CrearItemValido();
        var item2 = CrearItemValido();

        repositorio.Add(item1); 
        repositorio.Add(item2); 

        repositorio.Delete(item1.Id);

        var item3 = CrearItemValido();
        repositorio.Add(item3); 

        Assert.AreEqual(3, item3.Id);
    }
    
    [TestMethod]
    public void ItemNoExistenteNull()
    {
        
        var item = repositorio.GetById(999);

       
        Assert.IsNull(item);
    }

    [TestMethod]
    public void ItemsListaVacia()
    {
       
        var items = repositorio.GetAll();

        
        Assert.IsNotNull(items);
        Assert.AreEqual(0, ((List<Item>)items).Count);
    }
    
    [TestMethod]
    public void Add_ItemConTituloNull()
    {
        
        var item = new Item
        {
            Titulo = null, 
            Descripcion = "Descripcion valida",
            Catalogo = catalogoTest
        };

       
        var excepcion = Assert.ThrowsException<InvalidItemException>(() => repositorio.Add(item));

        Assert.AreEqual("El titulo es obligatorio",excepcion.Message);
    }
    
    [TestMethod]
    public void Add_ItemConDescripcionVacia()
    {
        
        var item = new Item
        {
            Titulo = "Título valido",
            Descripcion = "", 
            Catalogo = catalogoTest
        };

        var excepcion = Assert.ThrowsException<InvalidItemException>(() => repositorio.Add(item));

        Assert.AreEqual("La descripcion es obligatorio",excepcion.Message);
    }
    
    
    [TestMethod]
    public void Add_ItemConCatalogoNull()
    {
        var item = new Item
        {
            Titulo = "Titulo valido",
            Descripcion = "Descripcion valida",
            Catalogo = null 
        };

       
        var excepcion = Assert.ThrowsException<InvalidItemException>(() => repositorio.Add(item));

        Assert.AreEqual("El catalogo es obligatorio",excepcion.Message);
    }
    #endregion
    #region update
    [TestMethod]
    public void Update_ItemExistente_DebeActualizarse()
    {
        
        var item = CrearItemValido();
        repositorio.Add(item);
            
        
        item.Titulo = "Título Actualizado";
        item.Descripcion = "Descripción Actualizada";
        repositorio.Update(item);
            
        
        var itemActualizado = repositorio.GetById(item.Id);
        Assert.AreEqual("Título Actualizado", itemActualizado.Titulo);
        Assert.AreEqual("Descripción Actualizada", itemActualizado.Descripcion);
    }

    [TestMethod]
    [ExpectedException(typeof(ItemNotFoundException))]
    public void Update_ItemNoExiste_DebeLanzarExcepcion()
    {
        
        var item = CrearItemValido();
        
            
       
        repositorio.Update(item);
    }
    #endregion 
    
    
    #region delete
    [TestMethod]
    public void Delete_ItemExistente_DebeEliminarse()
    {
        
        var item = CrearItemValido();
        repositorio.Add(item);
            
        
        repositorio.Delete(item.Id);
            
        
        var itemEliminado = repositorio.GetById(item.Id);
        Assert.IsNull(itemEliminado);
    }

    [TestMethod]
    [ExpectedException(typeof(ItemNotFoundException))]
    public void Delete_ItemNoExiste_DebeLanzarExcepcion()
    {
        
        repositorio.Delete(999);
    }

    [TestMethod]
    public void Delete_DebeReducirCantidad()
    {
        
        var item1 = CrearItemValido();
        var item2 = CrearItemValido();
        item2.Titulo = "Producto 2";
        repositorio.Add(item1);
        repositorio.Add(item2);
            
        
        repositorio.Delete(item1.Id);
            
        
        var items = repositorio.GetAll();
        Assert.AreEqual(1, items.Count());
        Assert.AreEqual(item2.Id, items.First().Id);
    }

   
    
    #endregion 
    
    //crear item valido
    private Item CrearItemValido()
    {
        return new Item
        {
            Titulo = "Producto Test",
            Descripcion = "Descripción Test",
            Catalogo = catalogoTest
        };
    }
     
    [TestMethod]
    public void Add_ItemConSimilitudAlta_DebeCrearDeteccion()
    {
       
        var item1 = new Item
        {
            Titulo = "iPhone 13 128GB Negro",
            Descripcion = "Smartphone Apple 128GB color negro",
            Marca = "Apple",
            Modelo = "13 128GB",
            Catalogo = catalogoTest
        };

        var item2 = new Item
        {
            Titulo = "iPhone 13 128GB Negro",
            Descripcion = "Teléfono Apple 128GB color negro", 
            Marca = "Apple",
            Modelo = "13 128GB",
            Catalogo = catalogoTest
        };

        repositorio.Add(item1);
        repositorio.Add(item2);

        
        var lista = detecciones.ObtenerDeteccionesPendientes();
        Assert.AreEqual(1,lista.Count());
        var deteccion = lista.First();
        Assert.AreEqual("Pendiente", deteccion.Estado);
        Assert.IsTrue(deteccion.Score >= 0.60);
    }
}
    
