using NearDupFinder.Dominio.Entidades;
using NearDupFinder.Dominio.Interfaces;

namespace NearDupFinder.Infraestructura.Repositorios;

public class DeteccionRepositorio : IDeteccionRepositorio
{
    private readonly List<Deteccion> listaDetecciones;
    public DeteccionRepositorio()
    {
        listaDetecciones = new List<Deteccion>();
    }

    public void AgregarDeteccion(Deteccion deteccion)
    {
        deteccion.Id = listaDetecciones.Any() 
            ? listaDetecciones.Max(x => x.Id) + 1 
            : 1;
        listaDetecciones.Add(deteccion);
    }
    public List<Deteccion> ObtenerDeteccionesPendientes()
    {
        return listaDetecciones
            .Where(d => d.Estado == "Pendiente")
            .OrderByDescending(d => d.Score)
            .ThenBy(d => d.IdMenor)
            .ThenBy(d => d.IdMayor)
            .ToList();
    }
    public Deteccion ObtenerDeteccionPorId(int deteccionId)
    {
        return listaDetecciones.FirstOrDefault(d => d.Id == deteccionId);
    }
    public void ConfirmarDeteccion(int deteccionId)
    {
        var deteccion = ObtenerDeteccionPorId(deteccionId);
          
        deteccion.Confirmar();
    }
    public void DescartarDeteccion(int deteccionId)
    {
        var deteccion = ObtenerDeteccionPorId(deteccionId);
       

        deteccion.Descartar();
    }

    public bool TieneDeteccionesPendientes(int itemId)
    {
        return listaDetecciones.Any(d => 
            d.Estado == "Pendiente" && 
            (d.ItemAId == itemId || d.ItemBId == itemId)
        );
    }
    public void EliminarDeteccionesPorItemId(int itemId)
    {
        var deteccionesAEliminar = listaDetecciones
            .Where(d => d.ItemAId == itemId || d.ItemBId == itemId)
            .ToList();

        foreach (var deteccion in deteccionesAEliminar)
        {
            listaDetecciones.Remove(deteccion);
        }
    }
    
}