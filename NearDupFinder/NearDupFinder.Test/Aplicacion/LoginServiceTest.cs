using NearDupFinder.Aplicacion;
using NearDupFinder.Dominio.Entidades;
using NearDupFinder.Dominio.Interfaces;
using Moq;
using Microsoft.AspNetCore.Http;
using NearDupFinder.Dominio.Utiles;

[TestClass]
public class LoginServiceTests 
{
    private Mock<IUsuarioRepositorio> _mockRepositorio;
    private Seguridad _seguridad;
    private LoginService _loginService;

    [TestInitialize]
    public void Setup()
    {
        _mockRepositorio = new Mock<IUsuarioRepositorio>();
        _seguridad = new Seguridad(_mockRepositorio.Object);
        _loginService = new LoginService(_seguridad);
    }

    [TestMethod]
    public async Task Autenticar_ConCredencialesValidas_DebeRetornarUsuario()
    {
        // Arrange
        var email = "test@example.com";
        var password = "password123";
        var hashedPassword = PasswordHasher.Hash(password);
        var usuarioExistente = new Usuario { Id = 1, Email = email, PasswordHash = hashedPassword };
        
        _mockRepositorio.Setup(r => r.ObtenerPorEmail(email)).ReturnsAsync(usuarioExistente);

        // Act
        var resultado = await _loginService.Autenticar(email, password);

        // Assert
        Assert.IsNotNull(resultado);
        Assert.AreEqual(usuarioExistente.Id, resultado.Id);
    }

    [TestMethod]
    public async Task Autenticar_ConPasswordIncorrecto_DebeRetornarNull()
    {
        // Arrange
        var email = "test@example.com";
        var passwordCorrecto = "password123";
        var passwordIncorrecto = "malpassword";
        var hashedPassword = PasswordHasher.Hash(passwordCorrecto);
        var usuarioExistente = new Usuario { Id = 1, Email = email, PasswordHash = hashedPassword };

        _mockRepositorio.Setup(r => r.ObtenerPorEmail(email)).ReturnsAsync(usuarioExistente);

        // Act
        var resultado = await _loginService.Autenticar(email, passwordIncorrecto);

        // Assert
        Assert.IsNull(resultado);
    }
    
    [TestMethod]
    public async Task Autenticar_CuandoUsuarioNoExiste_DebeRetornarNull()
    {
        // Arrange
        var email = "noexiste@gmail.com";
        _mockRepositorio.Setup(r => r.ObtenerPorEmail(email)).ReturnsAsync((Usuario?)null);

        // Act
        var resultado = await _loginService.Autenticar(email, "password123");

        // Assert
        Assert.IsNull(resultado);
    }
}