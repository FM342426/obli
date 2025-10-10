using NearDupFinder.Dominio.Entidades;

namespace NearDupFinder.Dominio.Interfaces;


    public interface IClusterRepositorio
    {
        void GuardarCluster(Cluster cluster);
        Cluster? ObtenerClusterPorId(int clusterId);
        List<Cluster> ObtenerTodosClusters();
        List<Cluster> ObtenerClustersPorCatalogo(int catalogoId);
        void EliminarCluster(int clusterId);
        void ActualizarCluster(Cluster cluster);
        
        // Metodos para  item-cluster
        int? ObtenerClusterIdPorItem(int itemId);
        void AsignarItemACluster(int itemId, int clusterId);
        void RemoverItemDeCluster(int itemId);
    }
