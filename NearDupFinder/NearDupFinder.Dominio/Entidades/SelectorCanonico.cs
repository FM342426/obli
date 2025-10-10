namespace NearDupFinder.Dominio.Entidades;

public class SelectorCanonico
{
    public static Item SeleccionarCanonico(List<Item> items)
    {
        if (items == null || items.Count == 0)
            throw new ArgumentException("La lista de items no puede estar vacía");
            
        if (items.Count == 1)
            return items[0];
            
        Item canonico = items[0];
            
        foreach (var item in items.Skip(1))
        {
            if (EsMejorCanonico(item, canonico))
            {
                canonico = item;
            }
        }
            
        return canonico;
    }
        
    
    private static bool EsMejorCanonico(Item itemA, Item itemB)
    {
        // Regla 1: Descripcion más larga
        if (itemA.Descripcion.Length > itemB.Descripcion.Length)
            return true;
            
        if (itemA.Descripcion.Length < itemB.Descripcion.Length)
            return false;
            
        // Regla 2: Titulo mas largo (empate en descripción)
        if (itemA.Titulo.Length > itemB.Titulo.Length)
            return true;
            
        if (itemA.Titulo.Length < itemB.Titulo.Length)
            return false;
            
        // Regla 3: Id menor (empate en descripcion y titulo)
        return itemA.Id < itemB.Id;
    }
    

}