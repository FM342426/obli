using NearDupFinder.Dominio.Exceptions;

namespace NearDupFinder.Test;
using NearDupFinder.Dominio;

[TestClass]
public class ItemTest
{
    
    #region titulo

    [TestMethod]
    public void ItemAsignarTitulo()
    {
        var item = new Item();

        item.Titulo = "nuevo item";
        
        Assert.AreEqual("nuevo item",item.Titulo);
    }

    [TestMethod]
    public void ItemValidarTituloNull()
    {
        var item = new Item();
        item.Titulo = null;

        var excepcion = Assert.ThrowsException<InvalidItemException>(() => item.Validar());

        Assert.AreEqual("El titulo es obligatorio",excepcion.Message);
    }
    
    [TestMethod]
    public void ItemValidarTituloVacio()
    {
        var item = new Item();
        item.Titulo = "";
        
        var excepcion = Assert.ThrowsException<InvalidItemException>(() => item.Validar());

        Assert.AreEqual("El titulo es obligatorio",excepcion.Message);
    }

    [TestMethod]
    public void ItemValidarConTitulo()
    {
        var item = new Item();
        
        item.Titulo = "Producto valido";
        item.Descripcion = "Descripcion valida";
        item.Catalogo = _catalogoTest;
        item.Validar();
    }

    [TestMethod]
    public void ItemValidarLongitudFueraMaxTitulo()
    {
        var item = new Item();
        
        item.Titulo = new String('A', 121);
        var excepcion = Assert.ThrowsException<InvalidItemException>(() => item.Validar());

        Assert.AreEqual("El titulo no debe exceder 120 caracteres",excepcion.Message);
    }

    [TestMethod]
    public void ItemValidarLongitud120CaracteresTitulo()
    {
        var item = new Item();
        item.Titulo = new String('A', 120);
        item.Descripcion = "Descripcion valida";
        item.Catalogo = _catalogoTest;

        item.Validar();
    }
    [TestMethod]
    public void ItemValidarLongitudMinimaCaracteresTitulo()
    {
        var item = new Item();
        item.Titulo = "A";
        item.Descripcion = "Descripcion valida";
        item.Catalogo = _catalogoTest;

        item.Validar();
    }
    #endregion
    
    #region descripcion

    [TestMethod]

    public void ItemDescripcion()
    {
        var item = new Item();
        item.Titulo = "titulo valido";
        item.Descripcion = "descripcion del item";

        Assert.AreEqual("descripcion del item",item.Descripcion);
    }
    
    [TestMethod]
    public void ItemValidarDescripcionNull()
    {
        var item = new Item();
        item.Titulo = "titulo valido";
        item.Descripcion = null;

        var excepcion = Assert.ThrowsException<InvalidItemException>(() => item.Validar());

        Assert.AreEqual("La descripcion es obligatorio",excepcion.Message);
    }
    
    [TestMethod]
    public void ItemValidarDescripcionVacio()
    {
        var item = new Item();
        item.Titulo = "titulo valido";
        item.Descripcion = "";
        
        var excepcion = Assert.ThrowsException<InvalidItemException>(() => item.Validar());

        Assert.AreEqual("La descripcion es obligatorio",excepcion.Message);
    }
    
    [TestMethod]
    public void ItemValidarLongitudFueraMaxDescripcion()
    {
        var item = new Item();
        item.Titulo = "Producto válido";
        item.Descripcion = new string('D', 401); 
        
        var excepcion = Assert.ThrowsException<InvalidItemException>(() => item.Validar());

        Assert.AreEqual("La descripcion no debe exceder 400 caracteres",excepcion.Message);
    }

    [TestMethod]
    public void ItemValidarLongitud400CaracteresDescripcion()
    {
        var item = new Item();
        item.Titulo = "Producto válido";
        item.Descripcion = new string('D', 400); 
        item.Catalogo = _catalogoTest;

            
        item.Validar();
    }

    [TestMethod]
    public void temValidarLongitudMinimaCaracteresDescripcion()
    {
        var item = new Item();
        item.Titulo = "Producto válido";
        item.Descripcion = "D";
        item.Catalogo = _catalogoTest;

            
        item.Validar();
    }

    
    #endregion
    
    #region marca
    [TestMethod]
    
    public void ItemValidarLongitudFueraMaxMarca()
    {
        
        var item = new Item();
        item.Titulo = "Producto valido";
        item.Descripcion = "Descripcion valida";
        item.Marca = new string('M', 61); 
            
        
        var excepcion = Assert.ThrowsException<InvalidItemException>(() => item.Validar());

        Assert.AreEqual("la marca no debe exceder 60 caracteres",excepcion.Message);
    }

    [TestMethod]
    public void ItemValidarLongitud60CaracteresDescripcion()
    {
        
        var item = new Item();
        item.Titulo = "Producto válido";
        item.Descripcion = "Descripción válida";
        item.Marca = new string('M', 60); 
        item.Catalogo = _catalogoTest;

            
        
        item.Validar();
    }
    #endregion
    
    #region modelo
    [TestMethod]
    public void ItemValidarLongitudFueraMaxModelo()
    {
        
        var item = new Item();
        item.Titulo = "Producto valido";
        item.Descripcion = "Descripcion valida";
        item.Modelo = new string('M', 61); 
        

            
        
        var excepcion = Assert.ThrowsException<InvalidItemException>(() => item.Validar());

        Assert.AreEqual("el modelo no debe exceder 60 caracteres",excepcion.Message);
    }

    [TestMethod]
    public void ItemValidarLongitud60CaracteresModelo()
    {
        
        var item = new Item();
        item.Titulo = "Producto válido";
        item.Descripcion = "Descripción válida";
        item.Modelo = new string('A', 60); 
        item.Catalogo = _catalogoTest;

            
        
        item.Validar();
    }
    #endregion
    
    #region categoria
    [TestMethod]
    public void ItemValidarLongitudFueraMaxCategoria()
    {
        
        var item = new Item();
        item.Titulo = "Producto valido";
        item.Descripcion = "Descripcion valida";
        item.Categoria = new string ('A', 41);
            
        
        var excepcion = Assert.ThrowsException<InvalidItemException>(() => item.Validar());

        Assert.AreEqual("la categoria no debe exceder 40 caracteres",excepcion.Message);
    }

    [TestMethod]
    public void ItemValidarLongitud60CaracteresCategoria()
    {
        
        var item = new Item();
        item.Titulo = "Producto válido";
        item.Descripcion = "Descripción válida";
        item.Categoria = new string('A', 40);
        item.Catalogo = _catalogoTest;

            
        
        item.Validar();
    }
    
    #endregion
    
    #region catalogo
  
    private Catalogo _catalogoTest;
    
    [TestInitialize]
    public void Setup()
    {
        _catalogoTest = new Catalogo 
        { 
            Id = 1, 
            Titulo = "Catálogo Test",
            Descripcion = "Descripción del catálogo"
        };
    }
    
    [TestMethod]
    public void Item_PuedeAsignarCatalogo()
    {
        
        var item = new Item();
            
        
        item.Catalogo = _catalogoTest;
            
        
        Assert.AreEqual(_catalogoTest, item.Catalogo);
        Assert.AreEqual(1, item.Catalogo.Id);
    }

    [TestMethod]
    
    public void ValidarItem_CatalogoNulo_DebeLanzarExcepcion()
    {
        
        var item = new Item();
        item.Titulo = "Producto válido";
        item.Descripcion = "Descripción válida";
        item.Catalogo = null; 
            
        
        var excepcion = Assert.ThrowsException<InvalidItemException>(() => item.Validar());

        Assert.AreEqual("El catalogo es obligatorio",excepcion.Message);
    }

    [TestMethod]
    public void ValidarItem_ConCatalogo_NoDebeLanzarExcepcion()
    {
        
        var item = new Item();
        item.Titulo = "Producto válido";
        item.Descripcion = "Descripción válida";
        item.Catalogo = _catalogoTest;
            
        
        item.Validar();
    }
    #endregion
    
}