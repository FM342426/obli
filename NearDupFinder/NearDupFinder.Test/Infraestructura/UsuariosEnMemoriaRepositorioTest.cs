using NearDupFinder.Dominio.Entidades;
using NearDupFinder.Dominio.Exceptions;
using NearDupFinder.Dominio.Interfaces;
using NearDupFinder.Infraestructura.Repositorios;

 
[TestClass]
public class UsuariosEnMemoriaRepositorioTest
{
    private IUsuarioRepositorio _repositorio;

     
    [TestInitialize]
    public void Setup()
    {
        _repositorio = new UsuariosEnMemoriaRepositorio();
    }

    [TestMethod]
    public async Task Constructor_DebeInicializar_ConDosUsuarios()
    {
        // Arr y Act
        var usuarios = await _repositorio.ObtenerTodos();

        // Assert
        Assert.IsNotNull(usuarios);
        Assert.AreEqual(2, usuarios.Count());
    }

    [TestMethod]
    public async Task ObtenerPorId_ConIdExistente_DebeRetornarUsuarioCorrecto()
    {
        // Arra
        var idExistente = 1;

        // Act
        var usuario = await _repositorio.ObtenerPorId(idExistente);

        // Assert
        Assert.IsNotNull(usuario);
        Assert.AreEqual(idExistente, usuario.Id);
        Assert.AreEqual("admin@gmail.com", usuario.Email);
    }

    [TestMethod]
    public async Task ObtenerPorId_ConIdInexistente_DebeRetornarNull()
    {
        // Arran
        var idInexistente = 99;

        // Act
        var usuario = await _repositorio.ObtenerPorId(idInexistente);

        // Assert
        Assert.IsNull(usuario);
    }
    
    [TestMethod]
    public async Task ObtenerPorEmail_ConEmailExistenteCaseInsensitive_DebeRetornarUsuario()
    {
        // Arrange
        var emailExistente = "ADMIN@gmail.com"; 

        // Act
        var usuario = await _repositorio.ObtenerPorEmail(emailExistente);

        // Assert
        Assert.IsNotNull(usuario);
        Assert.AreEqual(1, usuario.Id);
    }

    [TestMethod]
    public async Task Agregar_UsuarioNuevo_DebeIncrementarConteoYAsignarId()
    {
        // Arra
        var nuevoUsuario = new Usuario { Email = "nuevo@test.com", Nombre = "Nuevo", PasswordHash = "hash" };
        
        // Act
        var usuarioAgregado = await _repositorio.Agregar(nuevoUsuario);
        var todosLosUsuarios = await _repositorio.ObtenerTodos();

        // Ass
        Assert.IsNotNull(usuarioAgregado);
        Assert.AreEqual(3, usuarioAgregado.Id); 
        Assert.AreEqual(3, todosLosUsuarios.Count());
    }

    [TestMethod]
    public async Task Actualizar_ConUsuarioExistente_DebeModificarDatos()
    {
        // Arr
        var usuarioParaActualizar = await _repositorio.ObtenerPorId(1);
        Assert.IsNotNull(usuarioParaActualizar);
        usuarioParaActualizar.Nombre = "Nombre actualizado";

        // Act
        await _repositorio.Actualizar(usuarioParaActualizar);
        var usuarioActualizado = await _repositorio.ObtenerPorId(1);

        // Ass
        Assert.IsNotNull(usuarioActualizado);
        Assert.AreEqual("Nombre actualizado", usuarioActualizado.Nombre);
    }

    [TestMethod]
    public async Task Actualizar_ConUsuarioInexistente_DebeLanzarInvalidOperationException()
    {
        // Arr
        var usuarioInexistente = new Usuario { Id = 99, Email = "noexiste@test.com" };

        // Act y Ass
        await Assert.ThrowsExceptionAsync<UsuarioException>(async () => 
        {
            await _repositorio.Actualizar(usuarioInexistente);
        });
    }

    [TestMethod]
    public async Task Eliminar_ConIdExistente_DebeRetornarTrueYQuitarUsuario()
    {
        // Arr
        var idParaEliminar = 1;

        // Act
        var resultado = await _repositorio.Eliminar(idParaEliminar);
        var usuarioEliminado = await _repositorio.ObtenerPorId(idParaEliminar);
        var conteoFinal = (await _repositorio.ObtenerTodos()).Count();

        // Ass
        Assert.IsTrue(resultado);
        Assert.IsNull(usuarioEliminado);
        Assert.AreEqual(1, conteoFinal);
    }

    [TestMethod]
    public async Task Eliminar_ConIdInexistente_DebeRetornarFalse()
    {
        // Arr
        var idInexistente = 99;

        // Act
        var resultado = await _repositorio.Eliminar(idInexistente);
        var conteoFinal = (await _repositorio.ObtenerTodos()).Count();

        // Ass
        Assert.IsFalse(resultado);
        Assert.AreEqual(2, conteoFinal); 
    }

    [TestMethod]
    public async Task ExisteEmailExcluyendoId_CuandoEmailNoCoincide_DebeRetornarFalse()
    {
        // Arr
        var emailExistente = "admin@gmail.com";
        var idAExcluir = 1;

        // Ac
        var existe = await _repositorio.ExisteEmailExcluyendoId(emailExistente, idAExcluir);

        // Assert
        Assert.IsFalse(existe);
    }

    [TestMethod]
    public async Task ExisteEmailExcluyendoId_CuandoEmailCoincideConOtroId_DebeRetornarTrue()
    {
        // Arr
        await _repositorio.Agregar(new Usuario { Email = "admin@gmail.com", Nombre = "Clon" });
        var emailExistente = "admin@gmail.com";
        var idAExcluir = 1; 

        // Act
        var existe = await _repositorio.ExisteEmailExcluyendoId(emailExistente, idAExcluir);

        // Ass
        Assert.IsTrue(existe);
    }
    
    
    [TestMethod]
    public async Task ExisteEmail_ConEmailExistente_DebeRetornarTrue()
    {
        // Arrange
        var emailExistente = "USER@GMAIL.COM";

        // Act
        var existe = await _repositorio.ExisteEmail(emailExistente);

        // Assert
        Assert.IsTrue(existe);
    }

    [TestMethod]
    public async Task ExisteEmail_ConEmailInexistente_DebeRetornarFalse()
    {
        // Arrange
        var emailInexistente = "noexiste@dominio.com";

        // Act
        var existe = await _repositorio.ExisteEmail(emailInexistente);

        // Assert
        Assert.IsFalse(existe);
    }

    [TestMethod]
    public async Task ObtenerPorRol_ConRolExistente_DebeRetornarUsuariosCorrectos()
    {
        // Arr
        var rolBuscado = RolesConstantes.ADMINISTRADOR;

        // Act
        var usuariosConRol = await _repositorio.ObtenerPorRol(rolBuscado);

        // Ass
        Assert.IsNotNull(usuariosConRol);
        Assert.AreEqual(1, usuariosConRol.Count());
        Assert.AreEqual("admin@gmail.com", usuariosConRol.First().Email);
    }

    [TestMethod]
    public async Task ObtenerPorRol_ConRolInexistente_DebeRetornarListaVacia()
    {
        // Arr
        var rolInexistente = "ROL_QUE_NO_EXISTE";

        // Act
        var usuariosConRol = await _repositorio.ObtenerPorRol(rolInexistente);

        // Ass
        Assert.IsNotNull(usuariosConRol);
        Assert.AreEqual(0, usuariosConRol.Count());
    }
}