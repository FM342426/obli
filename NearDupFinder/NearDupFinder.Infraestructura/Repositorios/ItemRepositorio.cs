using NearDupFinder.Dominio.Entidades;
using NearDupFinder.Dominio.Exceptions;
using NearDupFinder.Dominio.Interfaces;
using NearDupFinder.Dominio.Servicios;

namespace NearDupFinder.Infraestructura.Repositorios;
using NearDupFinder.Dominio.Repositorios;
using NearDupFinder.Dominio;

public class ItemRepositorio : IItemRepositorio
{
    private readonly List<Item> items;
    private readonly IDeteccionRepositorio _deteccion;



    public ItemRepositorio(IDeteccionRepositorio deteccion)
    {
        items = new List<Item>();
        _deteccion = deteccion;
    }

    public void Add(Item item)
    {
        item.Validar();
            
        // Asignar ID
        item.Id = items.Any() 
            ? items.Max(x => x.Id) + 1 
            : 1;
            
        items.Add(item);
        EjecutarDeteccionAutomatica(item);

    }


    

    public Item? GetById(int id)
    {
        return items.FirstOrDefault(i => i.Id == id);
    }

    public IEnumerable<Item> GetAll()
    {
        return items;
    }
    
    public void Update(Item item)
    {
        item.Validar();
            
        var existingItem = items.FirstOrDefault(i => i.Id == item.Id);
        if (existingItem == null)
        {
            throw new ItemNotFoundException($"Item con ID {item.Id} no encontrado");
        }
            
        _deteccion.EliminarDeteccionesPorItemId(item.Id);
        var index = items.IndexOf(existingItem);
        items[index] = item;
        EjecutarDeteccionAutomatica(item);
    }

    
    public void Delete(int id)
    {
        var item = items.FirstOrDefault(i => i.Id == id);
        if (item == null)
        {
            throw new ItemNotFoundException($"Item con ID {id} no encontrado");
        }
        _deteccion.EliminarDeteccionesPorItemId(item.Id);
        items.Remove(item);
    }
    private void EjecutarDeteccionAutomatica(Item itemNuevo)
    {
        var itemsMismoCatalogo = items.Where(i => i.Id != itemNuevo.Id &&  i.Catalogo?.Id == itemNuevo.Catalogo?.Id
        );

        foreach (var itemExistente in itemsMismoCatalogo)
        {
            var score = CalculadoraScore.CalcularScoreEntreItems(itemNuevo, itemExistente);
            var estadoUmbral = Umbrales.Evaluar(score);

            if (estadoUmbral != "NO DUPLICADO")
            {
                var deteccion = new Deteccion(itemNuevo, itemExistente);
                _deteccion.AgregarDeteccion(deteccion);
            }
        }
    }
}
    
    