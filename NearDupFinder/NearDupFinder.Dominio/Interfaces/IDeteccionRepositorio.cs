using NearDupFinder.Dominio.Entidades;

namespace NearDupFinder.Dominio.Interfaces;

public interface IDeteccionRepositorio
{
        void AgregarDeteccion(Deteccion deteccion);
        List<Deteccion> ObtenerDeteccionesPendientes();
        Deteccion ObtenerDeteccionPorId(int deteccionId);
        void ConfirmarDeteccion(int deteccionId);
        void DescartarDeteccion(int deteccionId);
        bool TieneDeteccionesPendientes(int itemId);
        void EliminarDeteccionesPorItemId(int itemId);

}

