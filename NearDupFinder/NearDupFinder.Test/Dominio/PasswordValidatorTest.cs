using NearDupFinder.Dominio.Entidades;
using NearDupFinder.Dominio.Exceptions;
using NearDupFinder.Dominio.Utiles;

namespace NearDupFinder.Test.Dominio;
 
[TestClass]
public class PasswordValidatorTest
{
    [TestMethod]
    public void Validate_DebeLanzarLoginException_SiPasswordEsNull()
    {
        // Arrange
        string passwordNull = null;

        // Act - Assert
        var exception = Assert.ThrowsException<LoginException>(() => 
            Validador.IsPasswordValido(passwordNull));
        Assert.AreEqual("El password no puede ser nulo o vacío.", exception.Message);
    }

    [TestMethod]
    public void Validate_DebeLanzarLoginException_SiPasswordEsVacio()
    {
        // Arrange
        var passwordVacio = "";

        // Act - Assert
        var exception = Assert.ThrowsException<LoginException>(() => 
            Validador.IsPasswordValido(passwordVacio));
        Assert.AreEqual("El password no puede ser nulo o vacío.", exception.Message);
    }

    [TestMethod]
    public void Validate_DebeLanzarLoginException_SiPasswordEsEspaciosEnBlanco()
    {
        // Arrange
        var passwordEspacios = "   ";

        // Act - Assert
        var exception = Assert.ThrowsException<LoginException>(() => 
            Validador.IsPasswordValido(passwordEspacios));
        Assert.AreEqual("El password no puede ser nulo o vacío.", exception.Message);
    }

    [TestMethod]
    public void Validate_DebeLanzarLoginException_SiPasswordEsMenorA8Caracteres()
    {
        // Arrange
        var passwordCorta = "1234567";

        // Act - Assert
        var exception = Assert.ThrowsException<LoginException>(() => 
            Validador.IsPasswordValido(passwordCorta));
        Assert.AreEqual("El password debe tener al menos 8 caracteres.", exception.Message);
    }
    
    [TestMethod]
    public void Validate_DebeLanzarLoginException_SiPasswordNoTieneMayuscula()
    {
        // Arrange
        var passwordSinMayuscula = "password123!";

        // Act - Assert
        var exception = Assert.ThrowsException<LoginException>(() => 
            Validador.IsPasswordValido(passwordSinMayuscula));
        Assert.AreEqual("El password debe contener al menos una letra mayúscula.", exception.Message);
    }
    
    [TestMethod]
    public void Validate_DebeLanzarLoginException_SiPasswordNoTieneMinuscula()
    {
        // Arrange
        var passwordSinMinuscula = "PASSWORD123!";

        // Act - Assert
        var exception = Assert.ThrowsException<LoginException>(() => 
            Validador.IsPasswordValido(passwordSinMinuscula));
        Assert.AreEqual("El password debe contener al menos una letra minúscula.", exception.Message);
    }
    
    [TestMethod]
    public void Validate_DebeLanzarLoginException_SiPasswordNoTieneNumero()
    {
        // Arrange
        var passwordSinNumero = "PasswordChar!";

        // Act - Assert
        var exception = Assert.ThrowsException<LoginException>(() => 
            Validador.IsPasswordValido(passwordSinNumero));
        Assert.AreEqual("El password debe contener al menos un número.", exception.Message);
    }
    
    [TestMethod]
    public void Validate_DebeLanzarLoginException_SiPasswordNoTieneCaracterEspecial()
    {
        // Arrange
        var passwordSinEspecial = "Password123";

        // Act - Assert
        var exception = Assert.ThrowsException<LoginException>(() => 
            Validador.IsPasswordValido(passwordSinEspecial));
        Assert.AreEqual("El password debe contener al menos un carácter especial.", exception.Message);
    }
    
    [TestMethod]
    public void Validate_NoDebeLanzarExcepcion_SiPasswordEsValida()
    {
        // Arrange
        var passwordValida = "Password123!";

        // Act - Assert 
        Validador.IsPasswordValido(passwordValida);
    }

    [TestMethod]
    public void Validate_NoDebeLanzarExcepcion_ConDiferentesTiposDeCaracteresEspeciales()
    {
        // Arrange - Act - Assert
        var passwordsValidas = new[]
        {
            "Password123!",
            "Password123@",
            "Password123#",
            "Password123$",
            "Password123%",
            "Password123^",
            "Password123&",
            "Password123*"
        };

        foreach (var password in passwordsValidas)
        {
            Validador.IsPasswordValido(password);
        }
    }

    [TestMethod]
    public void Validate_NoDebeLanzarExcepcion_ConPasswordDeLongitudMinima()
    {
        // Arrange
        var passwordMinima = "Passw1!a";  
        Validador.IsPasswordValido(passwordMinima);
       
    }
}