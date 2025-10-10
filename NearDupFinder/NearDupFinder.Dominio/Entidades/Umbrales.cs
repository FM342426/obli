namespace NearDupFinder.Dominio.Entidades;

public class Umbrales
{
    private const double t_alert = 0.60;
    private const double t_dup = 0.75;
    public static string EvaluarItems(Item itemA, Item itemB)
    {
        double score = CalculadoraScore.CalcularScoreEntreItems(itemA, itemB);
        return Evaluar(score);
    }

    public static string Evaluar(double score)
    {
        if (score >= t_dup)
        {
            return "DUPLICADO SUGERIDO";
        }
        if (score >= t_alert)
        {
            return "POSIBLE DUPLICADO"; 
        }
        
        return "NO DUPLICADO";
    }
}