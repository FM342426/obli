using System.Text;

namespace NearDupFinder.Dominio.Utiles;

public static class PasswordHasher
{
    public static string Hash(string password)
    {
        using var sha = System.Security.Cryptography.SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    public static bool Verificar(string password, string hash)
        => Hash(password) == hash;
}