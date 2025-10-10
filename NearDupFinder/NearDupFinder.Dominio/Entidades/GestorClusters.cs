using NearDupFinder.Dominio.Interfaces;
using NearDupFinder.Dominio.Repositorios;



namespace NearDupFinder.Dominio.Entidades;

public class GestorClusters
{
   private readonly IClusterRepositorio _repositorio; // 
        
        // Constructor con repositorio 
        public GestorClusters(IClusterRepositorio repositorio)
        {
            _repositorio = repositorio;
        }
        
        // Constructor sin parámetros (para tests - usa repositorio en memoria)
        public GestorClusters() : this(new ClusterRepositorioMemoria())
        {
        }
        
        public void ConfirmarDuplicado(Item itemA, Item itemB)
        {
            ValidarItems(itemA, itemB);
            
            bool aEnCluster = PerteneceACluster(itemA);
            bool bEnCluster = PerteneceACluster(itemB);
            
            if (!aEnCluster && !bEnCluster)
            {
                CrearNuevoCluster(itemA, itemB);
            }
            else if (aEnCluster && !bEnCluster)
            {
                AgregarItemAClusterExistente(itemA, itemB);
            }
            else if (!aEnCluster && bEnCluster)
            {
                AgregarItemAClusterExistente(itemB, itemA);
            }
            else
            {
                UnirClusters(itemA, itemB);
            }
        }
        
        private void ValidarItems(Item itemA, Item itemB)
        {
            if (itemA == null || itemB == null)
                throw new ArgumentException("Los items no pueden ser nulos");
            
            if (itemA.Catalogo.Id != itemB.Catalogo.Id)
                throw new ArgumentException("Los items deben pertenecer al mismo catálogo");
        }
        
        private void CrearNuevoCluster(Item itemA, Item itemB)
        {
            var nuevoCluster = new Cluster(new List<Item> { itemA, itemB });
            
            _repositorio.GuardarCluster(nuevoCluster); // Esto asigna el ID
            _repositorio.AsignarItemACluster(itemA.Id, nuevoCluster.Id);
            _repositorio.AsignarItemACluster(itemB.Id, nuevoCluster.Id);
            
            RecalcularCanonico(nuevoCluster);
            _repositorio.ActualizarCluster(nuevoCluster);
        }
        
        private void AgregarItemAClusterExistente(Item itemEnCluster, Item itemNuevo)
        {
            int? clusterId = _repositorio.ObtenerClusterIdPorItem(itemEnCluster.Id);
            if (!clusterId.HasValue) return;
            
            var cluster = _repositorio.ObtenerClusterPorId(clusterId.Value);
            if (cluster == null) return;
            
            cluster.Miembros.Add(itemNuevo);
            _repositorio.AsignarItemACluster(itemNuevo.Id, clusterId.Value);
            
            RecalcularCanonico(cluster);
            _repositorio.ActualizarCluster(cluster);
        }
        
        private void UnirClusters(Item itemA, Item itemB)
        {
            int? clusterIdA = _repositorio.ObtenerClusterIdPorItem(itemA.Id);
            int? clusterIdB = _repositorio.ObtenerClusterIdPorItem(itemB.Id);
            
            if (!clusterIdA.HasValue || !clusterIdB.HasValue) return;
            if (clusterIdA == clusterIdB) return;
            
            var clusterA = _repositorio.ObtenerClusterPorId(clusterIdA.Value);
            var clusterB = _repositorio.ObtenerClusterPorId(clusterIdB.Value);
            
            if (clusterA == null || clusterB == null) return;
            
            foreach (var miembro in clusterB.Miembros)
            {
                if (!clusterA.Miembros.Contains(miembro))
                {
                    clusterA.Miembros.Add(miembro);
                }
                _repositorio.AsignarItemACluster(miembro.Id, clusterIdA.Value);
            }
            
            _repositorio.EliminarCluster(clusterIdB.Value);
            RecalcularCanonico(clusterA);
            _repositorio.ActualizarCluster(clusterA);
        }
        
        private void RecalcularCanonico(Cluster cluster)
        {
            cluster.RecalcularCanonico();
            FusionadorItems.Fusionar(cluster.Canonico, cluster.Miembros);
        }
        
        public void ExcluirItem(Item item)
        {
            if (item == null)
                throw new ArgumentException("El item no puede ser nulo");
            
            if (!PerteneceACluster(item))
                return;
            
            int? clusterId = _repositorio.ObtenerClusterIdPorItem(item.Id);
            if (!clusterId.HasValue) return;
            
            var cluster = _repositorio.ObtenerClusterPorId(clusterId.Value);
            if (cluster == null) return;
            
            cluster.Miembros.Remove(item);
            _repositorio.RemoverItemDeCluster(item.Id);
            
            if (cluster.Miembros.Count < 2)
            {
                foreach (var miembro in cluster.Miembros)
                {
                    _repositorio.RemoverItemDeCluster(miembro.Id);
                }
                _repositorio.EliminarCluster(clusterId.Value);
            }
            else
            {
                RecalcularCanonico(cluster);
                _repositorio.ActualizarCluster(cluster);
            }
        }
        
        public bool PerteneceACluster(Item item)
        {
            return _repositorio.ObtenerClusterIdPorItem(item.Id).HasValue;
        }
        
        public Cluster? ObtenerCluster(Item item)
        {
            var clusterId = _repositorio.ObtenerClusterIdPorItem(item.Id);
            if (!clusterId.HasValue) return null;
            
            return _repositorio.ObtenerClusterPorId(clusterId.Value);
        }
        
        public List<Cluster> ObtenerTodosClusters()
        {
            return _repositorio.ObtenerTodosClusters();
        }
        
        public List<Cluster> ObtenerClustersPorCatalogo(int catalogoId)
        {
            return _repositorio.ObtenerClustersPorCatalogo(catalogoId);
        }
        
    }

