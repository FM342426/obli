using NearDupFinder.Aplicacion;

namespace ProyectoOb.Aplicacion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NearDupFinder.Aplicacion.Dtos;
//using NearDupFinder.Aplicacion.Interfaces;
//using NearDupFinder.Aplicacion.Servicios;
using NearDupFinder.Dominio.Exceptions;
using NearDupFinder.Dominio.Interfaces;
using NearDupFinder.Infraestructura.Repositorios;

[TestClass]
public class CatalogoServiceTests
{
     private ICatalogoService _service;
        private ICatalogoRepositorio _repositorio;

        [TestInitialize]
        public void Setup()
        {
            _repositorio = new CatalogoRepositorio();
            _service = new CatalogoService(_repositorio);
        }

        [TestMethod]
        public async Task Crear_ConDatosValidos_DeberiaCrearCatalogo()
        {
            // Arrange
            var dto = new CrearCatalogoDto("Catálogo de Prueba", "Descripción de prueba");

            // Act
            var resultado = await _service.Crear(dto);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual("Catálogo de Prueba", resultado.Titulo);
            Assert.AreEqual("Descripción de prueba", resultado.Descripcion);
            Assert.IsTrue(resultado.Id > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(CatalogoException))]
        public async Task  Crear_ConTituloVacio_DeberiaLanzarExcepcion()
        {
            // Arrange
            var dto = new CrearCatalogoDto("", "Descripción");

            // Act
            await _service.Crear(dto);

            // Assert - Se espera excepción
        }

        [TestMethod]
        [ExpectedException(typeof(CatalogoException))]
        public async Task  Crear_ConTituloMayorA120Caracteres_DeberiaLanzarExcepcion()
        {
            // Arrange
            var tituloLargo = new string('a', 121);
            var dto = new CrearCatalogoDto(tituloLargo, null);

            // Act
            await _service.Crear(dto);

            // Assert - Se espera excepción
        }

        [TestMethod]
        [ExpectedException(typeof(CatalogoException))]
        public async Task Crear_ConDescripcionMayorA400Caracteres_DeberiaLanzarExcepcion()
        {
            // Arrange
            var descripcionLarga = new string('a', 401);
            var dto = new CrearCatalogoDto("Título válido", descripcionLarga);

            // Act
            await _service.Crear(dto);

            // Assert - Se espera excepción
        }

        [TestMethod]
        [ExpectedException(typeof(CatalogoException))]
        public async Task  Crear_ConTituloDuplicado_DeberiaLanzarExcepcion()
        {
            // Arrange
            var dto1 = new CrearCatalogoDto("Título Único", "Primera descripción");
            var dto2 = new CrearCatalogoDto("Título Único", "Segunda descripción");

            // Act
            await _service.Crear(dto1);
            await _service.Crear(dto2); // Debe fallar

            // Assert - Se espera excepción
        }

        [TestMethod]
        public async Task Actualizar_ConDatosValidos_DeberiaActualizarCatalogo()
        {
            // Arrange
            var dtoCrear = new CrearCatalogoDto("Título Original", "Descripción original");
            var catalogo = _service.Crear(dtoCrear);
            var dtoActualizar = new ActualizarCatalogoDto("Título Actualizado", "Nueva descripción");

            // Act
            _service.Actualizar(catalogo.Id, dtoActualizar);
            var catalogoActualizado = await _service.ObtenerPorId(catalogo.Id);

            // Assert
            Assert.IsNotNull(catalogoActualizado);
            Assert.AreEqual("Título Actualizado", catalogoActualizado.Titulo);
            Assert.AreEqual("Nueva descripción", catalogoActualizado.Descripcion);
        }

        [TestMethod]
        [ExpectedException(typeof(CatalogoException))]
        public async Task Actualizar_ConIdInexistente_DeberiaLanzarExcepcion()
        {
            // Arrange
            var dto = new ActualizarCatalogoDto("Título", "Descripción");

            // Act
            await _service.Actualizar(999, dto);

            // Assert - Se espera excepción
        }

        [TestMethod]
        public async Task  Eliminar_ConIdExistente_DeberiaEliminarCatalogo()
        {
            // Arrange
            var dto = new CrearCatalogoDto("Catálogo a Eliminar", null);
            var catalogo =await  _service.Crear(dto);

            // Act
            await _service.Eliminar(catalogo.Id);
            var catalogoEliminado = await _service.ObtenerPorId(catalogo.Id);

            // Assert
            Assert.IsNull(catalogoEliminado);
        }

        [TestMethod]
        [ExpectedException(typeof(CatalogoException))]
        public async Task  Eliminar_ConIdInexistente_DeberiaLanzarExcepcion()
        {
            // Act
            await _service.Eliminar(999);

            // Assert - Se espera excepción
        }

        [TestMethod]
        public async Task Buscar_ConFiltro_DeberiaRetornarCoincidencias()
        {
            // Arrange
            await _service.Crear(new CrearCatalogoDto("Productos de Limpieza", "Artículos de limpieza"));
            await _service.Crear(new CrearCatalogoDto("Electrónicos", "Dispositivos electrónicos"));
            await _service.Crear(new CrearCatalogoDto("Productos de Oficina", "Material de oficina"));

            // Act
            var resultado = await _service.Buscar("productos");

            // Assert
            Assert.AreEqual(2, resultado.Count);
        }

        [TestMethod]
        public async Task ObtenerTodos_DeberiaRetornarTodosLosCatalogos()
        {
            // Arrange
            await _service.Crear(new CrearCatalogoDto("Catálogo 1", null));
            await _service.Crear(new CrearCatalogoDto("Catálogo 2", null));
            await _service.Crear(new CrearCatalogoDto("Catálogo 3", null));

            // Act
            var catalogos = await _service.ObtenerTodos();

            // Assert
            Assert.AreEqual(3, catalogos.Count);
        }
}