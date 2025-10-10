using NearDupFinder.Dominio.Exceptions;

namespace NearDupFinder.Dominio.Entidades;


public class Producto
{
    public int Id { get; private set; }
    public string Titulo { get; private set; }
    public string Descripcion { get; private set; }

    public Producto(int id, string titulo, string descripcion)
    { 
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ProductoException("El título es obligatorio.");
        
        if (titulo.Length > 120)
            throw new ProductoException("El título no puede exceder los 120 caracteres.");
        
        if (string.IsNullOrEmpty(descripcion))
            throw new ProductoException("La descripción es obligatoria.");
        
        if (descripcion.Length > 400)
            throw new ProductoException("La descripción no puede exceder los 400 caracteres.");
        
        Id = id;
        Titulo = titulo;
        Descripcion = descripcion;
    }
 
    public void Actualizar(string nuevoTitulo, string nuevaDescripcion)
    {
        Titulo = nuevoTitulo;
        Descripcion = nuevaDescripcion;
    }

    
}