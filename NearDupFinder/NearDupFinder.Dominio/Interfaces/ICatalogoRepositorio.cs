using NearDupFinder.Dominio;

namespace ProyectoOb;

public interface ICatalogoRepositorio
{
    void addCatalogoProducto(CatalogoProductos catalogoProductos);
    List<CatalogoProductos> GetAll();
    void Update(CatalogoProductos catalogo, string nuevoTitulo, string? nuevaDescripcion);
    void removeCatalogoProducto(CatalogoProductos catalogoProductos);
}