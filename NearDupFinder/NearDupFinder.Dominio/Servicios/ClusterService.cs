using NearDupFinder.Dominio.Entidades;
using NearDupFinder.Dominio.Exceptions;
using NearDupFinder.Dominio.Interfaces;
using NearDupFinder.Dominio.Repositorios;

namespace NearDupFinder.Dominio.Servicios;

public class ClusterService : IClusterService
{
     private readonly GestorClusters _gestorClusters;
        private readonly IItemRepositorio _itemRepositorio;
        private readonly IAuditoriaService _auditoriaService;
        
        public ClusterService(
            GestorClusters gestorClusters,
            IItemRepositorio itemRepositorio,
            IAuditoriaService auditoriaService)
        {
            _gestorClusters = gestorClusters;
            _itemRepositorio = itemRepositorio;
            _auditoriaService = auditoriaService;
        }
        
        public void ConfirmarDuplicados(int itemAId, int itemBId)
        {
            var itemA = _itemRepositorio.GetById(itemAId); 
            var itemB = _itemRepositorio.GetById(itemBId); 
            
            if (itemA == null || itemB == null)
                throw new ItemNotFoundException("Uno o ambos items no existen");
            
            _gestorClusters.ConfirmarDuplicado(itemA, itemB);
            
            // Registrar auditoría 
           // _auditoriaService.RegistrarAccion(
             //   "ConfirmarDuplicado",
               // $"Items {itemAId} y {itemBId} confirmados como duplicados",
                //DateTime.Now
            //);
        }
        
        public void ExcluirItemDeCluster(int itemId)
        {
            var item = _itemRepositorio.GetById(itemId); // <-- CAMBIO
            if (item == null)
                throw new ItemNotFoundException($"Item {itemId} no existe");
            
            _gestorClusters.ExcluirItem(item);
            
           // _auditoriaService.RegistrarAccion(
             //   "ExcluirDeCluster",
               // $"Item {itemId} excluido de su cluster",
                //DateTime.Now
            //);
        }
        
        public Cluster? ObtenerClusterPorItem(int itemId)
        {
            var item = _itemRepositorio.GetById(itemId); // <-- CAMBIO
            if (item == null) return null;
            
            return _gestorClusters.ObtenerCluster(item);
        }
        
        public List<Cluster> ObtenerClustersPorCatalogo(int catalogoId)
        {
            return _gestorClusters.ObtenerClustersPorCatalogo(catalogoId);
        }
        
        public List<Cluster> ObtenerTodosClusters()
        {
            return _gestorClusters.ObtenerTodosClusters();
        }
    }

    
