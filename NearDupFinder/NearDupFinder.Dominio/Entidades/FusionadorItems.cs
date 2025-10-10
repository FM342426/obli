namespace NearDupFinder.Dominio.Entidades;

public class FusionadorItems
{
    public static void Fusionar(Item canonico, List<Item> miembros)
    {
        if (canonico == null)
            throw new ArgumentException("El canónico no puede ser nulo");
            
        if (miembros == null || miembros.Count == 0)
            throw new ArgumentException("Debe haber al menos un miembro");
            
        // Fusionar Marca
        if (string.IsNullOrEmpty(canonico.Marca))
        {
            canonico.Marca = ObtenerMejorValor(miembros, m => m.Marca);
        }
            
        // Fusionar Modelo
        if (string.IsNullOrEmpty(canonico.Modelo))
        {
            canonico.Modelo = ObtenerMejorValor(miembros, m => m.Modelo);
        }
            
        // Fusionar Categoría
        if (string.IsNullOrEmpty(canonico.Categoria))
        {
            canonico.Categoria = ObtenerMejorValor(miembros, m => m.Categoria);
        }
    }
        
    private static string ObtenerMejorValor(List<Item> items, Func<Item, string> selector)
    {
        return items
            .Select(selector)
            .Where(valor => !string.IsNullOrEmpty(valor))
            .OrderByDescending(valor => valor.Length)
            .FirstOrDefault() ?? string.Empty;
    }

}