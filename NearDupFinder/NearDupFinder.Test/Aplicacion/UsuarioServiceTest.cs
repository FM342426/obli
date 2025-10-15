using Moq;
using NearDupFinder.Aplicacion;
using NearDupFinder.Dominio.Entidades;
using NearDupFinder.Dominio.Exceptions;
using NearDupFinder.Dominio.Interfaces;

[TestClass]
public class UsuarioServiceTests
{
    private Mock<IUsuarioRepositorio> _mockRepositorio;
    private Mock<IAuditoriaService> _mockAuditoria;
    private UsuarioService _usuarioService;

    
    private Usuario _usuarioDePrueba;

    [TestInitialize]
    public void Setup()
    {
        _mockRepositorio = new Mock<IUsuarioRepositorio>();
        _mockAuditoria = new Mock<IAuditoriaService>();
        _usuarioService = new UsuarioService(_mockRepositorio.Object, _mockAuditoria.Object);

        _usuarioDePrueba = new Usuario
        {
            Id = 1,
            Nombre = "Juan",
            Apellido = "Perez",
            Email = "juan.perez@test.com",
            PasswordHash = "hashed_password",
            
            Password = "Password@2025",
            FechaNacimiento = new DateTime(1990, 1, 1),
            Roles = new List<string> { RolesConstantes.ADMINISTRADOR }
        };
    }

    #region ObtenerPorId Tests

    [TestMethod]
    public async Task ObtenerPorId_ConIdValido_DebeRetornarUsuario()
    {
        // Arrange
        _mockRepositorio.Setup(r => r.ObtenerPorId(1)).ReturnsAsync(_usuarioDePrueba);

        // Act
        var resultado = await _usuarioService.ObtenerPorId(1);

        // Assert
        Assert.IsNotNull(resultado);
        Assert.AreEqual(1, resultado.Id);
        _mockRepositorio.Verify(r => r.ObtenerPorId(1), Times.Once);
    }

    [TestMethod]
    public async Task ObtenerPorId_ConIdCero_DebeRetornarNull()
    {
        // Act
        var resultado = await _usuarioService.ObtenerPorId(0);

        // Assert
        Assert.IsNull(resultado);
        _mockRepositorio.Verify(r => r.ObtenerPorId(It.IsAny<int>()), Times.Never);
    }

    #endregion

    #region Agregar Tests

    [TestMethod]
    public async Task Agregar_ConUsuarioValido_DebeLlamarAlRepositorioYAuditoria()
    {
        // Arrange
        _mockRepositorio.Setup(r => r.ExisteEmail(It.IsAny<string>())).ReturnsAsync(false);
        _mockRepositorio.Setup(r => r.Agregar(_usuarioDePrueba)).ReturnsAsync(_usuarioDePrueba);

        // Act
        _usuarioDePrueba.Password = "Password@2025";
        var resultado = await _usuarioService.Agregar(_usuarioDePrueba);

        // Assert
        Assert.IsNotNull(resultado);
        _mockRepositorio.Verify(r => r.Agregar(_usuarioDePrueba), Times.Once);
        _mockAuditoria.Verify(a => a.LogAccion(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public async Task Agregar_CuandoEmailYaExiste_DebeLanzarInvalidOperationException()
    {
        // Arrange
        _mockRepositorio.Setup(r => r.ExisteEmail(_usuarioDePrueba.Email)).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<LoginException>(() => _usuarioService.Agregar(_usuarioDePrueba));
    }

    [TestMethod]
    public async Task Agregar_ConNombreVacio_DebeLanzarArgumentException()
    {
        // Arrange
        _usuarioDePrueba.Nombre = "";

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentException>(() => _usuarioService.Agregar(_usuarioDePrueba));
    }

    #endregion

    #region Actualizar Tests

    [TestMethod]
    public async Task Actualizar_ConCambios_DebeLlamarAuditoria()
    {
        // Arrange
        var usuarioOriginal = new Usuario { Id = 1, Nombre = "Juan", Email = "juan.perez@test.com", Roles = new List<string> { RolesConstantes.ADMINISTRADOR } };
        var usuarioEditado = new Usuario { Id = 1, Nombre = "Juan Carlos", Email = "juan.perez@test.com", Roles = new List<string> { RolesConstantes.ADMINISTRADOR } };

        _mockRepositorio.Setup(r => r.ObtenerPorId(1)).ReturnsAsync(usuarioOriginal);
        _mockRepositorio.Setup(r => r.Actualizar(usuarioEditado)).ReturnsAsync(usuarioEditado);
        _mockRepositorio.Setup(r => r.ExisteEmailExcluyendoId(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(false);

        // Act
        var resultado = await _usuarioService.Actualizar(usuarioEditado);

        // Assert
        Assert.AreEqual("Juan Carlos", resultado.Nombre);
        _mockAuditoria.Verify(a => a.LogAccion(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public async Task Actualizar_SinCambios_NoDebeLlamarAuditoria()
    {
        // Arrange
        _mockRepositorio.Setup(r => r.ObtenerPorId(1)).ReturnsAsync(_usuarioDePrueba);
        _mockRepositorio.Setup(r => r.Actualizar(_usuarioDePrueba)).ReturnsAsync(_usuarioDePrueba);
        _mockRepositorio.Setup(r => r.ExisteEmailExcluyendoId(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(false);

        // Act
        await _usuarioService.Actualizar(_usuarioDePrueba);

        // Assert
        _mockAuditoria.Verify(a => a.LogAccion(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    #endregion

    #region Eliminar Tests

    [TestMethod]
    public async Task Eliminar_ConUsuarioValido_DebeRetornarTrueYLlamarAuditoria()
    {
        // Arrange
        _mockRepositorio.Setup(r => r.ObtenerPorId(1)).ReturnsAsync(_usuarioDePrueba);
        _mockRepositorio.Setup(r => r.Eliminar(1)).ReturnsAsync(true);

        // Act
        var resultado = await _usuarioService.Eliminar(1);

        // Assert
        Assert.IsTrue(resultado);
        _mockRepositorio.Verify(r => r.Eliminar(1), Times.Once);
        _mockAuditoria.Verify(a => a.LogAccion(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [TestMethod]
    public async Task Eliminar_UsuarioAdmin_DebeLanzarException()
    {
        // Arrange
        _usuarioDePrueba.Email = "admin@gmail.com";
        _mockRepositorio.Setup(r => r.ObtenerPorId(1)).ReturnsAsync(_usuarioDePrueba);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<Exception>(() => _usuarioService.Eliminar(1));
    }

    [TestMethod]
    public async Task Eliminar_CuandoUsuarioNoExiste_DebeRetornarFalse()
    {
        // Arrange
        _mockRepositorio.Setup(r => r.ObtenerPorId(99)).ReturnsAsync((Usuario?)null);

        // Act
        var resultado = await _usuarioService.Eliminar(99);

        // Assert
        Assert.IsFalse(resultado);
        _mockRepositorio.Verify(r => r.Eliminar(It.IsAny<int>()), Times.Never);
    }
    #endregion
    
    
    [TestMethod]
public async Task Actualizar_SinCambiosEnElUsuario_NoDebeLlamarAlServicioDeAuditoria()
{
    // Arrange
     var usuarioSinCambios = new Usuario 
    { 
        Id = 1, 
        Nombre = "Juan", 
        Apellido = "Perez", 
        Email = "juan@test.com",
        FechaNacimiento = new DateTime(1990, 1, 1),
        Roles = new List<string> { RolesConstantes.REVISOR_CATALOGO } 
    };

    
    _mockRepositorio.Setup(r => r.ObtenerPorId(1)).ReturnsAsync(usuarioSinCambios);
    _mockRepositorio.Setup(r => r.Actualizar(usuarioSinCambios)).ReturnsAsync(usuarioSinCambios);
 
    _mockRepositorio.Setup(r => r.ExisteEmailExcluyendoId(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(false);

    // Act
    await _usuarioService.Actualizar(usuarioSinCambios);

    // Assert
    
    _mockAuditoria.Verify(a => a.LogAccion(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
}

[TestMethod]
public async Task Actualizar_CuandoSoloCambianLosRoles_DebeRegistrarEnAuditoria()
{
    // Arrange
     var usuarioOriginal = new Usuario { Id = 1, Nombre = "Juan", Email = "juan@gmail.com",Roles = new List<string> { RolesConstantes.ADMINISTRADOR} };
    var usuarioEditado = new Usuario { Id = 1, Nombre = "Juan", Email = "juan2@gmail.com", Roles = new List<string> { RolesConstantes.ADMINISTRADOR, RolesConstantes.REVISOR_CATALOGO } };

    _mockRepositorio.Setup(r => r.ObtenerPorId(1)).ReturnsAsync(usuarioOriginal);
    _mockRepositorio.Setup(r => r.Actualizar(usuarioEditado)).ReturnsAsync(usuarioEditado);
    _mockRepositorio.Setup(r => r.ExisteEmailExcluyendoId(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(false);

    // Act
    await _usuarioService.Actualizar(usuarioEditado);

    // Assert
     _mockAuditoria.Verify(a => a.LogAccion(
        It.IsAny<string>(), 
        It.Is<string>(s => s.Contains("Roles cambiados"))), 
        Times.Once);
}

[TestMethod]
public async Task Agregar_ConEmailFormatoInvalido_DebeLanzarArgumentException()
{
    // Arrange
    _usuarioDePrueba.Email = "email-invalido";

    // Act & Assert
    var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => _usuarioService.Agregar(_usuarioDePrueba));
    Assert.AreEqual("El formato del email no es válido.", ex.Message);
}

[TestMethod]
public async Task Agregar_ConUsuarioSinRoles_DebeLanzarArgumentException()
{
    // Arrange
     _usuarioDePrueba.Roles = new List<string>();  

    // Act & Assert
    var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => _usuarioService.Agregar(_usuarioDePrueba));
    Assert.AreEqual("El usuario debe tener al menos un rol asignado.", ex.Message);
}

[TestMethod]
public async Task Agregar_ConRolInvalido_DebeLanzarArgumentException()
{
    // Arrange
    _usuarioDePrueba.Roles = new List<string> { "RolQueNoExiste" };

    // Act & Assert
    var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() => _usuarioService.Agregar(_usuarioDePrueba));
    Assert.AreEqual("El rol 'RolQueNoExiste' no es válido.", ex.Message);
}


[TestMethod]
public async Task Actualizar_CuandoUsuarioNoExiste_DebeLanzarInvalidOperationException()
{
    // Arrange
     _mockRepositorio.Setup(r => r.ObtenerPorId(_usuarioDePrueba.Id)).ReturnsAsync((Usuario?)null);

    // Act & Assert
    var ex = await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => _usuarioService.Actualizar(_usuarioDePrueba));
    Assert.AreEqual("Usuario no encontrado.", ex.Message);
}


#region ObtenerTodos / ObtenerPorEmail Tests

    [TestMethod]
    public async Task ObtenerTodos_CuandoHayUsuarios_DebeRetornarListaDeUsuarios()
    {
        // Arrange
        var listaUsuarios = new List<Usuario> { _usuarioDePrueba, new Usuario { Id = 2 } };
        _mockRepositorio.Setup(r => r.ObtenerTodos()).ReturnsAsync(listaUsuarios);

        // Act
        var resultado = await _usuarioService.ObtenerTodos();

        // Assert
        Assert.IsNotNull(resultado);
        Assert.AreEqual(2, resultado.Count());
        _mockRepositorio.Verify(r => r.ObtenerTodos(), Times.Once);
    }

    [TestMethod]
    public async Task ObtenerPorEmail_ConEmailValido_DebeRetornarUsuario()
    {
        // Arrange
        var email = "juan.perez@test.com";
        _mockRepositorio.Setup(r => r.ObtenerPorEmail(email)).ReturnsAsync(_usuarioDePrueba);

        // Act
        var resultado = await _usuarioService.ObtenerPorEmail(email);

        // Assert
        Assert.IsNotNull(resultado);
        Assert.AreEqual(email, resultado.Email);
    }

    [TestMethod]
    public async Task ObtenerPorEmail_ConEmailVacio_DebeRetornarNull()
    {
        // Act
        var resultado = await _usuarioService.ObtenerPorEmail("   ");

        // Assert
        Assert.IsNull(resultado);
        
        _mockRepositorio.Verify(r => r.ObtenerPorEmail(It.IsAny<string>()), Times.Never);
    }

    #endregion

  
    #region Métodos de Validación (ValidarEmail / ValidarEmailEdicion)

    [TestMethod]
    public async Task ValidarEmail_ConEmailDisponible_DebeRetornarTrue()
    {
        // Arrange
        var email = "nuevo@test.com";
        _mockRepositorio.Setup(r => r.ExisteEmail(email)).ReturnsAsync(false);

        // Act
        var resultado = await _usuarioService.ValidarEmail(email);

        // Assert
        Assert.IsTrue(resultado);
    }
    
    [TestMethod]
    public async Task ValidarEmail_ConEmailYaExistente_DebeRetornarFalse()
    {
        // Arrange
        var email = "existente@test.com";
        _mockRepositorio.Setup(r => r.ExisteEmail(email)).ReturnsAsync(true);

        // Act
        var resultado = await _usuarioService.ValidarEmail(email);

        // Assert
        Assert.IsFalse(resultado);
    }

    [TestMethod]
    public async Task ValidarEmailEdicion_ConEmailDisponible_DebeRetornarTrue()
    {
        // Arrange
        var email = "nuevo@test.com";
        var idUsuario = 1;
        _mockRepositorio.Setup(r => r.ExisteEmailExcluyendoId(email, idUsuario)).ReturnsAsync(false);

        // Act
        var resultado = await _usuarioService.ValidarEmailEdicion(email, idUsuario);

        // Assert
        Assert.IsTrue(resultado);
    }

    #endregion

 

    #region Métodos de Obtención por Rol

    [TestMethod]
    public async Task ObtenerPorRol_ConRolValido_DebeLlamarAlRepositorio()
    {
        // Arrange
        var rol = RolesConstantes.ADMINISTRADOR;
        var listaUsuarios = new List<Usuario> { _usuarioDePrueba };
        _mockRepositorio.Setup(r => r.ObtenerPorRol(rol)).ReturnsAsync(listaUsuarios);

        // Act
        var resultado = await _usuarioService.ObtenerPorRol(rol);

        // Assert
        Assert.IsTrue(resultado.Any());
        _mockRepositorio.Verify(r => r.ObtenerPorRol(rol), Times.Once);
    }

    [TestMethod]
    public async Task ObtenerPorRol_ConRolVacio_DebeRetornarListaVacia()
    {
        // Act
        var resultado = await _usuarioService.ObtenerPorRol("");

        // Assert
        Assert.IsFalse(resultado.Any());
        _mockRepositorio.Verify(r => r.ObtenerPorRol(It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public async Task ObtenerAdministradores_DebeLlamarObtenerPorRolConRolAdmin()
    {
        // Act
        await _usuarioService.ObtenerAdministradores();

        // Assert
          _mockRepositorio.Verify(r => r.ObtenerPorRol(RolesConstantes.ADMINISTRADOR), Times.Once);
    }

    [TestMethod]
    public async Task ObtenerRevisores_DebeLlamarObtenerPorRolConRolRevisor()
    {
        // Act
        await _usuarioService.ObtenerRevisores();

        // Assert
        
        _mockRepositorio.Verify(r => r.ObtenerPorRol(RolesConstantes.REVISOR_CATALOGO), Times.Once);
    }
    
    #endregion

    
    #region ExisteEmail / ExisteEmailExcluyendoId Tests

    [TestMethod]
    public async Task ExisteEmail_CuandoEmailExiste_DebeRetornarTrue()
    {
        // Arrange
        var email = "existente@test.com";
        _mockRepositorio.Setup(r => r.ExisteEmail(email)).ReturnsAsync(true);

        // Act
        var resultado = await _usuarioService.ExisteEmail("  existente@test.com  "); // Con espacios para probar el Trim()

        // Assert
        Assert.IsTrue(resultado);
        _mockRepositorio.Verify(r => r.ExisteEmail(email), Times.Once); // Verifica que se llamó con el email ya trimado
    }

    [TestMethod]
    public async Task ExisteEmail_CuandoEmailNoExiste_DebeRetornarFalse()
    {
        // Arrange
        var email = "nuevo@test.com";
        _mockRepositorio.Setup(r => r.ExisteEmail(email)).ReturnsAsync(false);

        // Act
        var resultado = await _usuarioService.ExisteEmail(email);

        // Assert
        Assert.IsFalse(resultado);
    }
    
    [TestMethod]
    public async Task ExisteEmail_ConEmailVacio_DebeRetornarFalseYSinLlamarAlRepositorio()
    {
        // Arrange: 

        // Act
        var resultado = await _usuarioService.ExisteEmail("   ");

        // Assert
        Assert.IsFalse(resultado);
         _mockRepositorio.Verify(r => r.ExisteEmail(It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public async Task ExisteEmailExcluyendoId_CuandoLlama_DebePasarParametrosCorrectosAlRepositorio()
    {
        // Arrange
        var email = "usuario@test.com";
        var idExcluir = 15;
         _mockRepositorio.Setup(r => r.ExisteEmailExcluyendoId(email, idExcluir)).ReturnsAsync(true);

        // Act
        var resultado = await _usuarioService.ExisteEmailExcluyendoId(email, idExcluir);

        // Assert
        Assert.IsTrue(resultado);
         _mockRepositorio.Verify(r => r.ExisteEmailExcluyendoId(email, idExcluir), Times.Once);
    }

    #endregion
    
}