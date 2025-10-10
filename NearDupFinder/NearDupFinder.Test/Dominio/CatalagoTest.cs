using NearDupFinder.Dominio;
using NearDupFinder.Dominio.Exceptions;

[TestClass]
public class CatalagoTest
{
    [TestMethod]
    public void CatalogoProductos_TituloVacio_LanzaCatalogoProductosException()
    {
        CatalogoProductosException exception = Assert.ThrowsException<CatalogoProductosException>(() =>
        {
            CatalogoProductos catalogoProductos = new CatalogoProductos("");
        });

        Assert.AreEqual("No se soporta títulos vacíos", exception.Message);
    }
     [TestMethod]
        public void CatalogoProductos_TituloMayor120_LanzaCatalogoProductosException()
        {
            string titulo = new string('A', 121);
            CatalogoProductosException exception = Assert.ThrowsException<CatalogoProductosException>(() =>
            {
                CatalogoProductos catalogoProductos = new CatalogoProductos(titulo);
            });
    
            Assert.AreEqual("El titulo debe tener una longitud menor a 120 caracteres", exception.Message);
        }
        [TestMethod]
        public void CatalogoProductos_TituloValido_SinDescripcion()
        {
            CatalogoProductos catalogoProductos = new CatalogoProductos("Producto válido");
            Assert.AreEqual("Producto válido", catalogoProductos.Titulo);
            Assert.IsNull(catalogoProductos.Descripcion);
        }

        [TestMethod]
        public void CatalogoProductos_DescOpc()
        {
            CatalogoProductos catalogoProductos = new CatalogoProductos("Mi producto", "Detalle del producto");
            Assert.AreEqual("Mi producto", catalogoProductos.Titulo);
            Assert.AreEqual("Detalle del producto", catalogoProductos.Descripcion);
        }
    
            [TestMethod]
            public void CatalogoProductos_DescMayor400_LanzaCatalogoProductosException()
            {
                string descripcion = new string('D', 401);
                string titulo = new string('A', 100);
                CatalogoProductosException exception = Assert.ThrowsException<CatalogoProductosException>(() =>
                {
                    CatalogoProductos catalogoProductos = new CatalogoProductos(titulo, descripcion);
                });
        
                Assert.AreEqual("La descripcion debe tener una longitud menor a 400 caracteres", exception.Message);
            }
                [TestMethod]
                public void ActualizaciónCatalogoProductos()
                {
                    string descripcion = new string('D', 40);
                    string titulo = new string('A', 100);
                        CatalogoProductos catalogoProductos = new CatalogoProductos(titulo, descripcion);
                        catalogoProductos.Actualizar("titulo", "descripción");
            
                    Assert.AreEqual(catalogoProductos.Titulo, "titulo");
                    Assert.AreEqual(catalogoProductos.Descripcion, "descripción");
                }
                [TestMethod]
                public void ActualizaciónCatalogoProductos_TitMayor120_LanzaCatalogoProductoException()
                {
                    string descripcion = new string('D', 40);
                    string titulo = new string('A', 100);
                    CatalogoProductos catalogoProductos = new CatalogoProductos(titulo, descripcion);
                    string tituloNuevo = new string('A', 121);
                    CatalogoProductosException exception = Assert.ThrowsException<CatalogoProductosException>(() =>
                    {
                       catalogoProductos.Actualizar(tituloNuevo, catalogoProductos.Descripcion);
                    });
            
                    Assert.AreEqual("El título debe tener una longitud menor a 120 caracteres", exception.Message);
                }
                [TestMethod]
                public void ActualizaciónCatalogoProductos_DescMayor400_LanzaCatalogoProductoException()
                {
                    string descripcion = new string('D', 40);
                    string titulo = new string('A', 100);
                    CatalogoProductos catalogoProductos = new CatalogoProductos(titulo, descripcion);
                    string descNueva = new string('A', 4001);
                    CatalogoProductosException exception = Assert.ThrowsException<CatalogoProductosException>(() =>
                    {
                        catalogoProductos.Actualizar(catalogoProductos.Titulo, descNueva);
                    });
            
                    Assert.AreEqual("La descripción debe tener una longitud menor a 400 caracteres", exception.Message);
                }
                [TestMethod]
                public void ActualizaciónCatalogoProductos_TitVacio_LanzaCatalogoProductoException()
                {
                    string descripcion = new string('D', 40);
                    string titulo = new string('A', 100);
                    CatalogoProductos catalogoProductos = new CatalogoProductos(titulo, descripcion);
                    string tituloNuevo = "";
                    CatalogoProductosException exception = Assert.ThrowsException<CatalogoProductosException>(() =>
                    {
                        catalogoProductos.Actualizar(tituloNuevo, catalogoProductos.Descripcion);
                    });
            
                    Assert.AreEqual("No se soporta títulos vacíos", exception.Message);
                }
                
}
