using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace NearDupFinder.Dominio.Entidades;

public class Normalizador
{
    public static string? Normalizar(string texto)
    { 
        if (string.IsNullOrEmpty(texto)) 
            return string.Empty;
        
        var textoProcesado =  QuitarTildes(texto.ToLower());
        textoProcesado = LimpiarCaracteresYEspacios(textoProcesado);
        return textoProcesado;
    }
    
    private static string QuitarTildes(string texto)
    {
        string textoNormalizado = texto.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in textoNormalizado)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        string retorno = stringBuilder.ToString().Normalize(NormalizationForm.FormC).ToLower();
        return retorno;
    }
    
    private static string LimpiarCaracteresYEspacios(string texto)
    {
        string sinCaracteresEspeciales = Regex.Replace(texto, "[^a-z0-9]", " ");
        string conEspaciosColapsados = Regex.Replace(sinCaracteresEspeciales, @"\s+", " ").Trim();
        return conEspaciosColapsados;
    }
} 