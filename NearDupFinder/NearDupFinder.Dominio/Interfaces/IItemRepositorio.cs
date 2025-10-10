namespace NearDupFinder.Dominio.Repositorios;

public interface IItemRepositorio
{
    void Add(Item item);
    Item? GetById(int id);
    IEnumerable<Item> GetAll();
    void Update(Item item);  
    void Delete(int id);
}

