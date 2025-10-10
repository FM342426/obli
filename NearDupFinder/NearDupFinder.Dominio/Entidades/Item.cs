using NearDupFinder.Dominio.Exceptions;

namespace NearDupFinder.Dominio;



public class Item
{

    public int Id { get; set; }
    
    public string? Titulo { get; set; }
    public string? Descripcion { get; set; }
    public string? Marca { get; set; }      
    public string? Modelo { get; set; }     
    public string? Categoria { get; set; }
    public Catalogo? Catalogo { get; set; }
    
    
    
    public void Validar()
    {  
        ValidarTitulo();
        ValidarDescripcion();
        ValidarMarca();
        ValidarModelo();
        ValidarCategoria();
        ValidarCatalogo();
    }
    
    private void ValidarTitulo()
    {
        if (string.IsNullOrEmpty(Titulo))
            throw new InvalidItemException("El titulo es obligatorio");
        if (Titulo.Length > 120)
            throw new InvalidItemException("El titulo no debe exceder 120 caracteres");
    }

    private void ValidarDescripcion()
    {
        if (string.IsNullOrEmpty(Descripcion))
            throw new InvalidItemException("La descripcion es obligatorio");
        if (Descripcion.Length > 400)
            throw new InvalidItemException("La descripcion no debe exceder 400 caracteres");
    }
    
    private void ValidarMarca()
    {
        if (!string.IsNullOrEmpty(Marca) && Marca.Length > 60)
            throw new InvalidItemException("la marca no debe exceder 60 caracteres");
    }
    private void ValidarModelo()
    {
        if (!string.IsNullOrEmpty(Modelo) && Modelo.Length > 60)
            throw new InvalidItemException("el modelo no debe exceder 60 caracteres");
    }
    
    private void ValidarCategoria()
    {
        if (!string.IsNullOrEmpty(Categoria) && Categoria.Length > 40)
            throw new InvalidItemException("la categoria no debe exceder 40 caracteres");
    }
    private void ValidarCatalogo()
    {
        if (Catalogo == null)
            throw new InvalidItemException("El catalogo es obligatorio");
    }
    
    

}