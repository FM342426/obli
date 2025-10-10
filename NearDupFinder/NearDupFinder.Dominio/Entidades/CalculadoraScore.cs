namespace NearDupFinder.Dominio.Entidades;

public class CalculadoraScore
{
    private const double PESO_TITULO = 0.45;
    private const double PESO_DESCRIPCION = 0.35;
    private const double PESO_MARCA = 0.10;
    private const double PESO_MODELO = 0.10;
    public static double Calcular(
        double jaccardTitulo, 
        double jaccardDescripcion, 
        int marcaEq, 
        int modeloEq)
    {
        return PESO_TITULO * jaccardTitulo 
               + PESO_DESCRIPCION * jaccardDescripcion 
               + PESO_MARCA * marcaEq 
               + PESO_MODELO * modeloEq;
    }
    public static double CalcularScoreEntreItems(Item itemA, Item itemB)
    {
        if (itemA == null || itemB == null)
            return 0.0;
    
        var jaccardTitulo = ComparadorDeItems.CalcularJaccardTitulo(
            itemA.Titulo, itemB.Titulo);
    
        var jaccardDescripcion = ComparadorDeItems.CalcularJaccardDescripcion(
            itemA.Descripcion, itemB.Descripcion);
    
        var marcaEq = ComparadorDeItems.CalcularMarcaEq(
            itemA.Marca, itemB.Marca);
    
        var modeloEq = ComparadorDeItems.CalcularModeloEq(
            itemA.Modelo, itemB.Modelo);
    
        return Calcular(jaccardTitulo, jaccardDescripcion, marcaEq, modeloEq);
    }
    
}