using Dominio.Dtos;
using NearDupFinder.Dominio.Entidades;

namespace NearDupFinder.Test.Dominio;

using Microsoft.VisualStudio.TestTools.UnitTesting;
 
using System;
using System.Collections.Generic;

[TestClass]
public class UsuarioDTOTests
{
    [TestMethod]
    public void Constructor_Deberia_InicializarPropiedades_ConValoresPorDefecto()
    {
        // Arr
        var dto = new UsuarioDTO();

        // Ass
        Assert.AreEqual(0, dto.Id);
        Assert.AreEqual(string.Empty, dto.Nombre);
        Assert.AreEqual(string.Empty, dto.Password);
        Assert.IsNotNull(dto.RolesSeleccionados);
        Assert.AreEqual(0, dto.RolesSeleccionados.Count);
        Assert.AreEqual(DateTime.Today.Year - 18, dto.FechaNacimiento.Year);
    }

    [TestMethod]
    public void Propiedades_Deberian_AsignarYObtenerValores_Correctamente()
    {
        // Arr
        var dto = new UsuarioDTO();
        var fechaPrueba = new DateTime(2000, 5, 20);
        var rolesPrueba = new List<string> { RolesConstantes.ADMINISTRADOR, RolesConstantes.REVISOR_CATALOGO };

        // Act
        dto.Id = 123;
        dto.Nombre = "Juan";
        dto.Apellido = "Pérez";
        dto.Email = "juan.perez@example.com";
        dto.FechaNacimiento = fechaPrueba;
        dto.Password = "Password123";
        dto.ConfirmarPassword = "Password123";
        dto.RolesSeleccionados = rolesPrueba;

        // Assert
        Assert.AreEqual(123, dto.Id);
        Assert.AreEqual("Juan", dto.Nombre);
        Assert.AreEqual("Pérez", dto.Apellido);
        Assert.AreEqual("juan.perez@example.com", dto.Email);
        Assert.AreEqual(fechaPrueba, dto.FechaNacimiento);
        Assert.AreEqual("Password123", dto.Password);
        Assert.AreEqual("Password123", dto.ConfirmarPassword);
        Assert.AreEqual(2, dto.RolesSeleccionados.Count);
        Assert.AreEqual(RolesConstantes.ADMINISTRADOR, dto.RolesSeleccionados[0]);
    }
}