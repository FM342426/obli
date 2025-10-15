using Moq;
using NearDupFinder.Dominio.Entidades;
using NearDupFinder.Dominio.Exceptions;
using NearDupFinder.Dominio.Interfaces;
using NearDupFinder.Dominio.Utiles;

namespace NearDupFinder.Test.Dominio;

[TestClass]
public class LoginTest
{
    
    private Seguridad _servicioLogin;
    private Mock<IServicioSeguridadHash> _mockServicioSeguridadHash;
    private Mock<IUsuarioRepositorio> _mockRepositorio;
    
    
    [TestInitialize]
    public void Inicializar()  
    { 
        _mockRepositorio = new Mock<IUsuarioRepositorio>();
        _mockServicioSeguridadHash = new Mock<IServicioSeguridadHash>();
        _servicioLogin = new Seguridad(_mockRepositorio.Object);
    }
    
    [TestCleanup]
    public void Limpiar() 
    { 
        _servicioLogin = null;
        _mockRepositorio = null;
        _mockServicioSeguridadHash = null;
    }
    
    [TestMethod]
    public async Task Login_DebeFallar_CuandoEmailEsVacio()
    {
        // Arrange
   
        // Act
        var resultado = await _servicioLogin.Autenticar("", "1234");

        // Assert
        Assert.IsNull(resultado); 
    }
    
    [TestMethod]
    public async Task Login_DebeFallar_CuandoPasswordEsVacio()
    {
        // Arrange
        
        
        // Act
        var resultado = await _servicioLogin.Autenticar("usuario@gmail.com", "");

        // Assert
        Assert.IsNull(resultado);
    }
     

    [TestMethod]
    public async Task  Login_DebeFallar_CuandoEmailNoTieneFormatoValido()
    {
        // Arrange
        
        // Act
        var resultado = await _servicioLogin.Autenticar("usuariogmail.com", "1234");

        // Assert
        Assert.IsNull(resultado);
    }

    
    [TestMethod]
    public async Task  Login_DebeFallar_SiUsuarioNoExiste()
    {
        // Arrange  
        _mockRepositorio.Setup(repo => repo.ObtenerPorEmail(It.IsAny<string>())).ReturnsAsync((Usuario)null);

        // Act
        var resultado = await _servicioLogin.Autenticar("noexiste@gmail.com", "1234");
        
        // Assert
        Assert.IsNull(resultado);
    }
    
    [TestMethod]
    public async Task Login_DebeFallar_SiPasswordEsIncorrecto()
    {
        // Arrange
        var usuarioFalso = new Usuario
        {
            Email = "usuario@gmail.com", 
            Nombre ="Juan",
            Apellido="", 
            FechaNacimiento=DateTime.Now
        };
        _mockRepositorio.Setup(repo => repo.ObtenerPorEmail("usuario@gmail.com")).ReturnsAsync(usuarioFalso);
        _mockServicioSeguridadHash.Setup(s => s.Verificar("password_incorrecto", "hash_correcto")).Returns(false);

        // Act
        var resultado =await _servicioLogin.Autenticar("usuario@gmail.com", "password_incorrecto");

        // Assert
        Assert.IsNull(resultado);
    }

    
    [TestMethod]
    public async Task Login_DebeSerExitoso_CuandoCredencialesSonCorrectas()
    {
        // Arrange
        var email = "admin@gmail.com";
        var passwordCorrecto = "1234";
        var passwordHash = PasswordHasher.Hash("1234");  

        var usuarioEsperado = new Usuario
        {
            Email = email,
            Nombre = "admin",
            PasswordHash = passwordHash
        };

       
        _mockRepositorio.Setup(repo => repo.ObtenerPorEmail(email))
            .ReturnsAsync(usuarioEsperado);

         
        _mockServicioSeguridadHash.Setup(s => s.Verificar(passwordCorrecto, passwordHash))
            .Returns(true);

        // Act
        var resultado = await _servicioLogin.Autenticar(email, passwordCorrecto);

        // Assert
        Assert.IsNotNull(resultado);
        Assert.AreEqual(usuarioEsperado.Email, resultado.Email);
    }

    [TestMethod]
    public async Task Login_DebeDevolverNull_CuandoRepositorioLanzaExcepcion()
    {
        // Arrange
        var email = "usuario@gmail.com";
        var password = "1234";

         
        _mockRepositorio.Setup(repo => repo.ObtenerPorEmail(email))
            .ThrowsAsync(new LoginException("Error simulado de conexión a la BD"));

        // Act
        
        var resultado = await _servicioLogin.Autenticar(email, password);

        // Assert
        Assert.IsNull(resultado);
    }
    
}