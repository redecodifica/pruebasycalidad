using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Controllers;
using ProyectoFinal.Data;
using ProyectoFinal.Models;
using ProyectoFinal.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Login
{
    [TestFixture]
    internal class AccesoControllerTests
    {
        [TearDown]
        public void TearDown()
        {
            // Liberar los recursos del contexto de base de datos
            if (_context != null)
            {
                _context.Dispose();
            }
            if (_controller != null)
            {
                _controller.Dispose();
            }
        }

        private AppDBContext _context;

        private AccesoController _controller;

        

        [SetUp]
        public void Setup()
        {
            // Configurar la base de datos en memoria
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new AppDBContext(options);
            _controller = new AccesoController(_context);

            // Crear un Mock de IAuthenticationService
            var authenticationServiceMock = new Mock<IAuthenticationService>();

            // Configurar el contexto HTTP simulado para autenticación
            var services = new ServiceCollection();
            services.AddSingleton(authenticationServiceMock.Object); // Agregar el mock de IAuthenticationService
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);

            // Registrar los servicios faltantes para las pruebas
            services.AddSingleton<ITempDataDictionaryFactory, TempDataDictionaryFactory>();
            services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();
            services.AddMvc(); // Esto también agrega varios servicios MVC necesarios, como IActionResultExecutor

            var serviceProvider = services.BuildServiceProvider();
            var httpContext = new DefaultHttpContext
            {
                RequestServices = serviceProvider
            };

            // Configurar el ControllerContext con un ActionContext completo
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext,
                RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
                ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor()
            };
        }

        [Test]
        public async Task Login_ValidCredentials_ReturnsRedirectToDashboard()
        {
            // Arrange
            var testUser = new Usuario { email = "test@test.com", contraseña = "password123", Nombre_usuario = "Test User" };
            _context.Usuarios.Add(testUser);
            await _context.SaveChangesAsync();

            var loginVM = new LoginVM { email = "test@test.com", contraseña = "password123" };

            // Act
            var result = await _controller.Login(loginVM) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.ActionName, Is.EqualTo("DashboardUsuario"));
            Assert.That(result.ControllerName, Is.EqualTo("Usuario"));
        }

        [Test]
        public async Task Login_InvalidCredentials_ReturnsViewWithMessage()
        {
            // Arrange
            var loginVM = new LoginVM { email = "wrong@test.com", contraseña = "wrongpassword" };

            // Act
            var result = await _controller.Login(loginVM) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.ViewData["Mensaje"], Is.EqualTo("No se encontraron usuarios"));
        }
    }
}