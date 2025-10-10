using NearDupFinder.Dominio.Entidades;
using NearDupFinder.Dominio.Interfaces;

namespace NearDupFinder.Dominio.Repositorios;

public class ClusterRepositorioMemoria : IClusterRepositorio
{
    private Dictionary<int, Cluster> _clusters = new();
    private Dictionary<int, int> _itemACluster = new();
    private int _proximoId = 1;
        
    public void GuardarCluster(Cluster cluster)
    {
        cluster.Id = _proximoId;
        _clusters[_proximoId] = cluster;
        _proximoId++;
    }
        
    public Cluster? ObtenerClusterPorId(int clusterId)
    {
        return _clusters.GetValueOrDefault(clusterId);
    }
        
    public List<Cluster> ObtenerTodosClusters()
    {
        return _clusters.Values.ToList();
    }
        
    public List<Cluster> ObtenerClustersPorCatalogo(int catalogoId)
    {
        return _clusters.Values
            .Where(c => c.CatalogoId == catalogoId)
            .ToList();
    }
        
    public void EliminarCluster(int clusterId)
    {
        _clusters.Remove(clusterId);
    }
        
    public void ActualizarCluster(Cluster cluster)
    {
        // En memoria no necesita actualización
    }
        
    public int? ObtenerClusterIdPorItem(int itemId)
    {
        if (_itemACluster.TryGetValue(itemId, out int clusterId))
            return clusterId;
        return null;
    }
        
    public void AsignarItemACluster(int itemId, int clusterId)
    {
        _itemACluster[itemId] = clusterId;
    }
        
    public void RemoverItemDeCluster(int itemId)
    {
        _itemACluster.Remove(itemId);
    }
}
