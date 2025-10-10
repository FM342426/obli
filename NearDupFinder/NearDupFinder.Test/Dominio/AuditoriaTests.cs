using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NearDupFinder.Aplicacion;
using NearDupFinder.Dominio.Entidades;
using NearDupFinder.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NearDupFinder.Aplicacion;
using NearDupFinder.Dominio.Entidades;
using NearDupFinder.Dominio.Interfaces;

[TestClass]
public class AuditoriaTests
{
    [TestMethod]
    public async Task LogAccion_Automatico_DebeObtenerUsuarioDelContextoYPasarAlRepositorio()
    {
        // Arr
        var mockRepo = new Mock<IAuditoriaRepository>();
        var mockAuthStateProvider = new Mock<AuthenticationStateProvider>();
        
        var userEmail = "usuario.logueado@test.com";
        var claims = new[] { new Claim(ClaimTypes.Email, userEmail) };
        var identity = new ClaimsIdentity(claims, "TestAuthentication");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        var authState = new AuthenticationState(claimsPrincipal);
        
        mockAuthStateProvider
            .Setup(x => x.GetAuthenticationStateAsync())
            .ReturnsAsync(authState);

        var service = new AuditoriaService(mockRepo.Object, mockAuthStateProvider.Object);
        
        var accion = "Item YYYYYYY modificado";
        var detalles = "Campo 'XXXX' cambiado de Z a y.";

        // Act
        await service.LogAccion(accion, detalles);

        // Ass
        mockRepo.Verify(repo => repo.Log(It.Is<AuditoriaEntry>(
            e => e.Usuario == userEmail && 
                 e.Accion == accion &&
                 e.Detalles == detalles &&
                 e.Timestamp != default(DateTime)
        )), Times.Once);
    }

    [TestMethod]
    public async Task LogAccion_SinUsuarioAutenticado_DebeUsarSistema()
    {
        // Arr
        var mockRepo = new Mock<IAuditoriaRepository>();
        var mockAuthStateProvider = new Mock<AuthenticationStateProvider>();
        
        var identity = new ClaimsIdentity();
        var claimsPrincipal = new ClaimsPrincipal(identity);
        var authState = new AuthenticationState(claimsPrincipal);
        
        mockAuthStateProvider
            .Setup(x => x.GetAuthenticationStateAsync())
            .ReturnsAsync(authState);

        var service = new AuditoriaService(mockRepo.Object, mockAuthStateProvider.Object);
        
        var accion = "Acción del sistema";
        var detalles = "Detalle automático";

        // Act
        await service.LogAccion(accion, detalles);

        // Ass
        mockRepo.Verify(repo => repo.Log(It.Is<AuditoriaEntry>(
            e => e.Usuario == "Sistema" && 
                 e.Accion == accion &&
                 e.Detalles == detalles
        )), Times.Once);
    }

    [TestMethod]
    public async Task LogAccion_UsuarioSinClaimEmail_DebeUsarNombreOSistema()
    {
        // Arr
        var mockRepo = new Mock<IAuditoriaRepository>();
        var mockAuthStateProvider = new Mock<AuthenticationStateProvider>();
        
        var userName = "JuanPerez";
        var claims = new[] { new Claim(ClaimTypes.Name, userName) };
        var identity = new ClaimsIdentity(claims, "TestAuthentication");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        var authState = new AuthenticationState(claimsPrincipal);
        
        mockAuthStateProvider
            .Setup(x => x.GetAuthenticationStateAsync())
            .ReturnsAsync(authState);

        var service = new AuditoriaService(mockRepo.Object, mockAuthStateProvider.Object);
        
        var accion = "Acción sin email";

        // Act
        await service.LogAccion(accion);

        // Ass
        mockRepo.Verify(repo => repo.Log(It.Is<AuditoriaEntry>(
            e => e.Usuario == userName && e.Accion == accion
        )), Times.Once);
    }

    [TestMethod]
    public async Task LogAccion_ClaimEmailMinuscula_DebeReconocerlo()
    {
        // Arr
        var mockRepo = new Mock<IAuditoriaRepository>();
        var mockAuthStateProvider = new Mock<AuthenticationStateProvider>();
        
        var userEmail = "usuario@test.com";
        var claims = new[] { new Claim("email", userEmail) }; 
        var identity = new ClaimsIdentity(claims, "TestAuthentication");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        var authState = new AuthenticationState(claimsPrincipal);
        
        mockAuthStateProvider
            .Setup(x => x.GetAuthenticationStateAsync())
            .ReturnsAsync(authState);

        var service = new AuditoriaService(mockRepo.Object, mockAuthStateProvider.Object);
        
        var accion = "Test email minúscula";

        // Act
        await service.LogAccion(accion);

        // Ass
        mockRepo.Verify(repo => repo.Log(It.Is<AuditoriaEntry>(
            e => e.Usuario == userEmail
        )), Times.Once);
    }

    [TestMethod]
    public async Task GetLogs_DebeDevolverLaListaDelRepositorio()
    {
        // Arr
        var mockRepo = new Mock<IAuditoriaRepository>();
        var mockAuthStateProvider = new Mock<AuthenticationStateProvider>();

        var logsEsperados = new List<AuditoriaEntry>
        {
            new AuditoriaEntry("user1", "Accion 1"),
            new AuditoriaEntry("user2", "Accion 2")
        };
        
        mockRepo.Setup(repo => repo.GetLogs()).ReturnsAsync(logsEsperados);
        
        var service = new AuditoriaService(mockRepo.Object, mockAuthStateProvider.Object);

        // Act
        var logsRecibidos = await service.GetLogs();

        // Ass
        Assert.IsNotNull(logsRecibidos);
        Assert.AreEqual(2, logsRecibidos.Count);
        CollectionAssert.AreEqual(logsEsperados, logsRecibidos);
    }

    [TestMethod]
    public async Task GetLogs_ListaVacia_DebeRetornarListaVacia()
    {
        // Arr
        var mockRepo = new Mock<IAuditoriaRepository>();
        var mockAuthStateProvider = new Mock<AuthenticationStateProvider>();

        mockRepo.Setup(repo => repo.GetLogs()).ReturnsAsync(new List<AuditoriaEntry>());
        
        var service = new AuditoriaService(mockRepo.Object, mockAuthStateProvider.Object);

        // Act
        var logsRecibidos = await service.GetLogs();

        // As
        Assert.IsNotNull(logsRecibidos);
        Assert.AreEqual(0, logsRecibidos.Count);
    }
}