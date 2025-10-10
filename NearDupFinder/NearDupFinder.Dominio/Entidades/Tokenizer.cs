namespace NearDupFinder.Dominio.Entidades;

public class Tokenizer
{
    public static List<string> Tokenize(string textoNormalizado)
    {
        if (string.IsNullOrEmpty(textoNormalizado))
        {
            return new List<string>();
        }
        
        return textoNormalizado.Split(' ')
            .Where(token => token.Length > 1)
            .ToList();
    }
}