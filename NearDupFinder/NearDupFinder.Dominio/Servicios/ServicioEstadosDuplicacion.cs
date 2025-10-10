using NearDupFinder.Dominio.Entidades;
using NearDupFinder.Dominio.Interfaces;

namespace NearDupFinder.Dominio.Servicios;

public class ServicioEstadosDuplicacion : IServicioEstadosDuplicacion
{
    private readonly IDeteccionRepositorio _servicioDetecciones;
    private readonly GestorClusters _clusters;
    
    public ServicioEstadosDuplicacion(IDeteccionRepositorio servicioDetecciones, GestorClusters clusters)
    {
        _servicioDetecciones = servicioDetecciones;
        _clusters = clusters;
    }
    public string ObtenerEstadoItem(Item item)
    {
        if (_clusters.PerteneceACluster(item))
            return "En clúster";
        
        if (_servicioDetecciones.TieneDeteccionesPendientes(item.Id))
            return "Con candidatos";
        
        return "Sin candidatos";
    }
}
