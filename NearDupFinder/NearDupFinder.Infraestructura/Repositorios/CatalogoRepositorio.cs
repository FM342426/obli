
using NearDupFinder.Dominio;
using NearDupFinder.Dominio.Exceptions;

namespace ProyectoOb;

public class CatalogoRepositorio : ICatalogoRepositorio
{
    private readonly List<CatalogoProductos> listaCatalogosProductos;

    public CatalogoRepositorio()
    {
        listaCatalogosProductos = new List<CatalogoProductos>();
    }

    public void addCatalogoProducto(CatalogoProductos catalogoProductos)
    {
        if (catalogoProductos == null)
        {
            throw new ArgumentNullException("catalogoProductos");
        }
        listaCatalogosProductos.Add(catalogoProductos);
    }

    public List<CatalogoProductos> GetAll()
    {
        return listaCatalogosProductos;
    }
    
    public void Update(CatalogoProductos catalogo, string nuevoTitulo, string? nuevaDescripcion)
    {
        if (!listaCatalogosProductos.Contains(catalogo))
            throw new CatalogoProductosException("El catálogo no existe en la lista.");

        catalogo.Actualizar(nuevoTitulo, nuevaDescripcion);
    }


    public void removeCatalogoProducto(CatalogoProductos catalogoProductos)
    {
        if (!listaCatalogosProductos.Contains(catalogoProductos))
        {
            throw new CatalogoProductosException("El catálogo no existe en la lista.");
        }
        listaCatalogosProductos.Remove(catalogoProductos);
    }
}