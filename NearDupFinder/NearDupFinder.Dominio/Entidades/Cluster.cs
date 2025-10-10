namespace NearDupFinder.Dominio.Entidades;

public class Cluster
{
    public int Id { get; set; }
    public Item Canonico { get; private set; }
    public List<Item> Miembros { get; private set; }
    public int CatalogoId { get; private set; }
        
    public Cluster(List<Item> items)
    {
        if (items == null || items.Count < 2)
            throw new ArgumentException("Un cluster debe tener al menos 2 items");
            
        // Validar que todos los items sean del mismo catalogo
        var catalogoId = items[0].Catalogo.Id;
        if (items.Any(i => i.Catalogo.Id != catalogoId))
            throw new ArgumentException("Todos los items deben pertenecer al mismo catálogo");
            
        CatalogoId = catalogoId;
        Miembros = new List<Item>(items);
        Canonico = SelectorCanonico.SeleccionarCanonico(Miembros);
    }
    public void RecalcularCanonico()
    {
        if (Miembros.Count > 0)
        {
            Canonico = SelectorCanonico.SeleccionarCanonico(Miembros);
        }
    }
}
