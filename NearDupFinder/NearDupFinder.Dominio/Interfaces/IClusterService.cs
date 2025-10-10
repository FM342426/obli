using NearDupFinder.Dominio.Entidades;

namespace NearDupFinder.Dominio.Interfaces;

public interface IClusterService
{
    void ConfirmarDuplicados(int itemAId, int itemBId);
    void ExcluirItemDeCluster(int itemId);
    Cluster? ObtenerClusterPorItem(int itemId);
    List<Cluster> ObtenerClustersPorCatalogo(int catalogoId);
    List<Cluster> ObtenerTodosClusters();

}