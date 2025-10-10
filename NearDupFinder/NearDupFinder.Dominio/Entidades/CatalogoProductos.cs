using NearDupFinder.Dominio.Exceptions;

namespace NearDupFinder.Dominio;

public class CatalogoProductos
{
        public string Titulo { get; private set; }
        public string? Descripcion { get; private set; }

        public CatalogoProductos(string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
            {
                throw new CatalogoProductosException("No se soporta títulos vacíos");
            }
            if (titulo.Length>120)
            {
                throw new CatalogoProductosException("El titulo debe tener una longitud menor a 120 caracteres");
            }
            this.Titulo = titulo;
        }
        public CatalogoProductos(string titulo, string? descripcion)
            : this(titulo) 
        {
            if (descripcion.Length>400)
            {
                throw new CatalogoProductosException("La descripcion debe tener una longitud menor a 400 caracteres");
            }
            this.Descripcion = descripcion;
        }
        public void Actualizar(string nuevoTitulo, string? nuevaDescripcion)
        {
            if (string.IsNullOrWhiteSpace(nuevoTitulo))
                throw new CatalogoProductosException("No se soporta títulos vacíos");

            if (nuevoTitulo.Length > 120)
                throw new CatalogoProductosException("El título debe tener una longitud menor a 120 caracteres");

            if (nuevaDescripcion != null && nuevaDescripcion.Length > 400)
                throw new CatalogoProductosException("La descripción debe tener una longitud menor a 400 caracteres");

            this.Titulo = nuevoTitulo;
            this.Descripcion = nuevaDescripcion;
        }
}