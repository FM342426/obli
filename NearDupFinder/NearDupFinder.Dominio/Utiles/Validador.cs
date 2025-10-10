using System.Net.Mail;
using NearDupFinder.Dominio.Exceptions;

namespace NearDupFinder.Dominio.Utiles;

public class Validador
{
    public static bool IsEmailValido(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            // Uso el constructor de MailAddress para valida el formato del email
            var direccionEmail = new MailAddress(email);
           
            return true;
        }
        catch (FormatException)
        {
            return false;
        }

    }
    
    public static void IsPasswordValido(string password)
    {
       
        if (string.IsNullOrWhiteSpace(password))
            throw new LoginException("El password no puede ser nulo o vacío.");
        
        if (password.Length < 8)
        {
            throw new LoginException("El password debe tener al menos 8 caracteres.");
        }
        
        if (!password.Any(char.IsUpper))
        {
            throw new LoginException("El password debe contener al menos una letra mayúscula.");
        }
        
        if (!password.Any(char.IsLower))
        {
            throw new LoginException("El password debe contener al menos una letra minúscula.");
        }
        
        if (!password.Any(char.IsDigit))
        {
            throw new LoginException("El password debe contener al menos un número.");
        }
        
        if (password.All(char.IsLetterOrDigit))
        {
            throw new LoginException("El password debe contener al menos un carácter especial.");
        }
    }
    
}