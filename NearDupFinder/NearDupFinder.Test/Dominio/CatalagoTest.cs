using NearDupFinder.Dominio;
using NearDupFinder.Dominio.Exceptions;

[TestClass]
public class CatalagoTest
{
    [TestMethod]
    public void CatalogoProductos_TituloVacio_LanzaCatalogoProductosException()
    {
        CatalogoException exception = Assert.ThrowsException<CatalogoException>(() =>
        {
            Catalogo catalogoProductos = new Catalogo("");
        });

        Assert.AreEqual("El título es obligatorio", exception.Message);
    }
     [TestMethod]
        public void CatalogoProductos_TituloMayor120_LanzaCatalogoProductosException()
        {
            string titulo = new string('A', 121);
            CatalogoException exception = Assert.ThrowsException<CatalogoException>(() =>
            {
                Catalogo catalogoProductos = new Catalogo(titulo);
            });
    
            Assert.AreEqual("El título debe tener entre 1 y 120 caracteres", exception.Message);
        }
        [TestMethod]
        public void CatalogoProductos_TituloValido_SinDescripcion()
        {
            Catalogo catalogoProductos = new Catalogo("Producto válido");
            Assert.AreEqual("Producto válido", catalogoProductos.Titulo);
            Assert.IsNull(catalogoProductos.Descripcion);
        }

        [TestMethod]
        public void CatalogoProductos_DescOpc()
        {
            Catalogo catalogoProductos = new Catalogo("Mi producto", "Detalle del producto");
            Assert.AreEqual("Mi producto", catalogoProductos.Titulo);
            Assert.AreEqual("Detalle del producto", catalogoProductos.Descripcion);
        }
    
            [TestMethod]
            public void CatalogoProductos_DescMayor400_LanzaCatalogoProductosException()
            {
                string descripcion = new string('D', 401);
                string titulo = new string('A', 100);
                CatalogoException exception = Assert.ThrowsException<CatalogoException>(() =>
                {
                    Catalogo catalogoProductos = new Catalogo(titulo, descripcion);
                });
        
                Assert.AreEqual("La descripción no puede exceder 400 caracteres", exception.Message);
            }
                [TestMethod]
                public void ActualizaciónCatalogoProductos()
                {
                    string descripcion = new string('D', 40);
                    string titulo = new string('A', 100);
                        Catalogo catalogoProductos = new Catalogo(titulo, descripcion);
                        catalogoProductos.Actualizar("titulo", "descripción");
            
                    Assert.AreEqual(catalogoProductos.Titulo, "titulo");
                    Assert.AreEqual(catalogoProductos.Descripcion, "descripción");
                }
                [TestMethod]
                public void ActualizaciónCatalogoProductos_TitMayor120_LanzaCatalogoProductoException()
                {
                    string descripcion = new string('D', 40);
                    string titulo = new string('A', 100);
                    Catalogo catalogoProductos = new Catalogo(titulo, descripcion);
                    string tituloNuevo = new string('A', 121);
                    CatalogoException exception = Assert.ThrowsException<CatalogoException>(() =>
                    {
                       catalogoProductos.Actualizar(tituloNuevo, catalogoProductos.Descripcion);
                    });
            
                    Assert.AreEqual("El título debe tener entre 1 y 120 caracteres", exception.Message);
                }
                [TestMethod]
                public void ActualizaciónCatalogoProductos_DescMayor400_LanzaCatalogoProductoException()
                {
                    string descripcion = new string('D', 40);
                    string titulo = new string('A', 100);
                    Catalogo catalogoProductos = new Catalogo(titulo, descripcion);
                    string descNueva = new string('A', 4001);
                    CatalogoException exception = Assert.ThrowsException<CatalogoException>(() =>
                    {
                        catalogoProductos.Actualizar(catalogoProductos.Titulo, descNueva);
                    });
            
                    Assert.AreEqual("La descripción no puede exceder 400 caracteres", exception.Message);
                }
                [TestMethod]
                public void ActualizaciónCatalogoProductos_TitVacio_LanzaCatalogoProductoException()
                {
                    string descripcion = new string('D', 40);
                    string titulo = new string('A', 100);
                    Catalogo catalogoProductos = new Catalogo(titulo, descripcion);
                    string tituloNuevo = "";
                    CatalogoException exception = Assert.ThrowsException<CatalogoException>(() =>
                    {
                        catalogoProductos.Actualizar(tituloNuevo, catalogoProductos.Descripcion);
                    });
            
                    Assert.AreEqual("El título es obligatorio", exception.Message);
                }
                
}
