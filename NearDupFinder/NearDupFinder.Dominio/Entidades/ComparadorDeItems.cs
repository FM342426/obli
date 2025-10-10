namespace NearDupFinder.Dominio.Entidades;

public class ComparadorDeItems
{
    public static double CalcularJaccardTitulo(string tituloA, string tituloB)
    {
        return CalcularJaccard(tituloA, tituloB);
    }
    
    public static double CalcularJaccardDescripcion(string descripcionA, string descripcionB)
    {
        return CalcularJaccard(descripcionA, descripcionB);
    }
    
    public static int CalcularMarcaEq(string marcaA, string marcaB)
    {
        return CalcularIgualdad(marcaA, marcaB);
    }
    
    public static int CalcularModeloEq(string modeloA, string modeloB)
    {
        return CalcularIgualdad(modeloA, modeloB);
    }
    
    private static double CalcularJaccard(string textoA, string textoB)
    {
        if (string.IsNullOrEmpty(textoA) && string.IsNullOrEmpty(textoB))
            return 0.0;
        
        var textoNormalizadoA = Normalizador.Normalizar(textoA);
        var textoNormalizadoB = Normalizador.Normalizar(textoB);
        
        var tokensA = Tokenizer.Tokenize(textoNormalizadoA);
        var tokensB = Tokenizer.Tokenize(textoNormalizadoB);
        
        var union = tokensA.Union(tokensB).Count();
        if (union == 0) return 0.0;
        
        var interseccion = tokensA.Intersect(tokensB).Count();
        
        return (double)interseccion / union;
    }
    
    private static int CalcularIgualdad(string campoA, string campoB)
    {
        if (string.IsNullOrEmpty(campoA) || string.IsNullOrEmpty(campoB))
            return 0;
        
        var campoNormalizadoA = Normalizador.Normalizar(campoA);
        var campoNormalizadoB = Normalizador.Normalizar(campoB);
        
        return campoNormalizadoA == campoNormalizadoB ? 1 : 0;
    }
}