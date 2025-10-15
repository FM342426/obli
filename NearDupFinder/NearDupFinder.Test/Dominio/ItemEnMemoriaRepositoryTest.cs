using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Moq;
using NearDupFinder.Aplicacion;
using NearDupFinder.Aplicacion.Interfaces;
using NearDupFinder.Dominio.Entidades;
using NearDupFinder.Dominio.Exceptions;
using NearDupFinder.Dominio.Interfaces;
using ProyectoOb;

namespace NearDupFinder.Test;
using NearDupFinder.Dominio;
using NearDupFinder.Dominio.Repositorios;
using NearDupFinder.Infraestructura.Repositorios;

[TestClass]
public class ItemEnMemoriaRepositoryTest
{
    private IItemRepositorio repositorio;
    private Catalogo catalogoTest;
    private DeteccionEnMemoriaRepository detecciones;

    
    private IItemService _itemService;
    private IAuditoriaService auditoriaService; 
    private ICatalogoService catalogoService; 
    private ICsvParserService csvParserService;
 
    
    
    [TestInitialize]
    public void Setup()
    {
        detecciones = new DeteccionEnMemoriaRepository();
        repositorio = new ItemEnMemoriaRepository(detecciones);
       
       
    
        catalogoTest = new Catalogo 
        { 
            Id = 1, 
            Titulo = "Catalogo Test",
            Descripcion = "Test"
        };
    }
    #region add
    
    [TestMethod]
    public async Task AddItemValido()
    {
        
        var item = new Item
        {
            Titulo = "Producto Test",
            Descripcion = "Descripcion Test",
            Catalogo = catalogoTest
        };

        Assert.AreEqual(0, item.Id); // 0 antes de agregar

        await repositorio.Agregar(item);
        var itemAgregado = await repositorio.ObtenerPorId(item.Id);

        Assert.IsNotNull(itemAgregado);
        Assert.AreEqual(1, item.Id); //  1 despues de agregar
        Assert.AreEqual(item.Titulo, itemAgregado.Titulo);
    }

    [TestMethod]
    public async Task Add_DosItems_IdsConsecutivos()
    {
        var item1 = CrearItemValido();
        var item2 = CrearItemValido();
        item2.Titulo = "Producto 2";

        await repositorio.Agregar(item1);
        await repositorio.Agregar(item2);
        
        var todos = await repositorio.ObtenerTodos();

        Assert.AreEqual(1, item1.Id);
        Assert.AreEqual(2, item2.Id);
        Assert.AreEqual(2, todos.Count());
    }
    
    [TestMethod]
    public async Task Add_ItemDespuesDeDelete_ReutilizaIdsCorrectamente()
    {
        var item1 = CrearItemValido();
        var item2 = CrearItemValido();

        await repositorio.Agregar(item1); 
        await repositorio.Agregar(item2); 

        await repositorio.Eliminar(item1.Id);

        var item3 = CrearItemValido();
        await repositorio.Agregar(item3); 

        Assert.AreEqual(3, item3.Id);
    }
    
    [TestMethod]
    public async Task ItemNoExistenteNull()
    {
        var item = await repositorio.ObtenerPorId(999);
        Assert.IsNull(item);
    }

    [TestMethod]
    public async Task ItemsListaVacia()
    {
        var items = await repositorio.ObtenerTodos();
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

       
        var excepcion = Assert.ThrowsException<InvalidItemException>(() => repositorio.Agregar(item));

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

        var excepcion = Assert.ThrowsException<InvalidItemException>(() => repositorio.Agregar(item));

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

       
        var excepcion = Assert.ThrowsException<InvalidItemException>(() => repositorio.Agregar(item));

        Assert.AreEqual("El catalogo es obligatorio",excepcion.Message);
    }
    #endregion
    #region update
    [TestMethod]
    public async Task Update_ItemExistente_DebeActualizarse()
    {
        
        var item = CrearItemValido();
        await repositorio.Agregar(item);
            
        
        item.Titulo = "Título Actualizado";
        item.Descripcion = "Descripción Actualizada";
        await  repositorio.Actualizar(item);
            
        
        var itemActualizado = await  repositorio.ObtenerPorId(item.Id);
        Assert.AreEqual("Título Actualizado", itemActualizado.Titulo);
        Assert.AreEqual("Descripción Actualizada", itemActualizado.Descripcion);
    }

    [TestMethod]
    [ExpectedException(typeof(ItemNotFoundException))]
    public void Update_ItemNoExiste_DebeLanzarExcepcion()
    {
        var item = CrearItemValido();
        repositorio.Actualizar(item);
    }
    #endregion 
    
    
    #region delete
    [TestMethod]
    public async Task Delete_ItemExistente_DebeEliminarse()
    {
        
        var item = CrearItemValido();
        await repositorio.Agregar(item);
        await repositorio.Eliminar(item.Id);
        
        var itemEliminado = await repositorio.ObtenerPorId(item.Id);
        Assert.IsNull(itemEliminado);
    }

    [TestMethod]
    [ExpectedException(typeof(ItemNotFoundException))]
    public void Delete_ItemNoExiste_DebeLanzarExcepcion()
    {
        repositorio.Eliminar(999);
    }

    [TestMethod]
    public async Task Delete_DebeReducirCantidad()
    {
        
        var item1 = CrearItemValido();
        var item2 = CrearItemValido();
        item2.Titulo = "Producto 2";
        await repositorio.Agregar(item1);
        await repositorio.Agregar(item2);
        await repositorio.Eliminar(item1.Id);
        var items = await repositorio.ObtenerTodos();
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
    public async Task Add_ItemConSimilitudAlta_DebeCrearDeteccion()
    {
       
        var mockRepo = new Mock<IAuditoriaRepository>();
        var mockAuthStateProvider = new Mock<AuthenticationStateProvider>();
        
        var userEmail = "usuario.logueado@test.com";
        var claims = new[] { new Claim(ClaimTypes.Email, userEmail) };
        var identity = new ClaimsIdentity(claims, "TestAuthentication");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        var authState = new AuthenticationState(claimsPrincipal);
        
        mockAuthStateProvider
            .Setup(x => x.GetAuthenticationStateAsync())
            .ReturnsAsync(authState);

        auditoriaService = new AuditoriaService(mockRepo.Object, mockAuthStateProvider.Object);
        _itemService = new ItemService(repositorio, auditoriaService, csvParserService, catalogoService, detecciones);

        
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

        await repositorio.Agregar(item1);
        await _itemService.EjecutarDeteccionAutomatica(item1);
        await repositorio.Agregar(item2);
        await _itemService.EjecutarDeteccionAutomatica(item2);
        
        var lista = await detecciones.ObtenerDeteccionesPendientes();
        Assert.AreEqual(1,lista.Count());
        var deteccion = lista.First();
        Assert.AreEqual("Pendiente", deteccion.Estado);
        Assert.IsTrue(deteccion.Score >= 0.60);
    }
}
    
