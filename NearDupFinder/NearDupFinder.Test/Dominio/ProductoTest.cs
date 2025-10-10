using NearDupFinder.Dominio.Entidades;
using NearDupFinder.Dominio.Exceptions;

namespace NearDupFinder.Test.Dominio;

[TestClass]
public class ProductoTest
{
    [TestMethod]
    public void Constructor_ConDatosValidos_DebeCrearInstanciaCorrectamente()
    {
        // Arrange
        var titulo = "Mi Primer Producto";
        var descripcion = "Esta es una descripción valida.";

        // Act
        var producto = new Producto(1, titulo, descripcion);

        // Assert
        Assert.IsNotNull(producto);
        Assert.AreEqual(1, producto.Id);
        Assert.AreEqual(titulo, producto.Titulo);
        Assert.AreEqual(descripcion, producto.Descripcion);
    }
    
    [TestMethod]
    public void Constructor_DebeLanzarExcepcion_SiTituloEsNull()
    {
        // Act & Assert
        Assert.ThrowsException<ProductoException>(() => 
            new Producto(1, null, "una descripción"));
    }

    [TestMethod]
    public void Constructor_DebeLanzarExcepcion_SiTituloEsVacio()
    {
        // Act & Assert
        Assert.ThrowsException<ProductoException>(() => 
            new Producto(1, "", "una descripción"));
    }

    [TestMethod]
    public void Constructor_DebeLanzarExcepcion_SiTituloEsEspaciosEnBlanco()
    {
        // Act & Assert
        Assert.ThrowsException<ProductoException>(() => 
            new Producto(1, " ", "una descripción"));
    }
    
    [TestMethod]
    public void Constructor_DebeLanzarExcepcion_SiTituloExcede120Caracteres()
    {
        // Arrange
        var tituloLargo = new string('a', 121);

        // Act & Assert
        Assert.ThrowsException<ProductoException>(() => 
            new Producto(1, tituloLargo, "descripción"));
    }
    
    [TestMethod]
    public void Constructor_DebeLanzarExcepcion_SiDescripcionExcede400Caracteres()
    {
        // Arrange
        var descripcionLarga = new string('a', 401);

        // Act & Assert
        Assert.ThrowsException<ProductoException>(() => 
            new Producto(1, "Título Válido", descripcionLarga));
    }
    
    [TestMethod]
    public void Actualizar_ConDatosValidos_DebeModificarPropiedades()
    {
        // Arrange
        var producto = new Producto(1, "Título Original", "Desc Original");
        var nuevoTitulo = "Título Modificado";
        var nuevaDescripcion = "Descripción Modificada";

        // Act
        producto.Actualizar(nuevoTitulo, nuevaDescripcion);

        // Assert
        Assert.AreEqual(nuevoTitulo, producto.Titulo);
        Assert.AreEqual(nuevaDescripcion, producto.Descripcion);
    }
}