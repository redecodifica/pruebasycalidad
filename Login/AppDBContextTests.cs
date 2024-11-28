using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Controllers;
using ProyectoFinal.Data;
using ProyectoFinal.Models;
using ProyectoFinal.ViewModels;
using System.Threading.Tasks;

namespace Login
{
    [TestFixture]
    internal class AppDBContextTests
    {
        private AppDBContext _context;

        [SetUp]
        public void Setup()
        {
            // Configurar una base de datos en memoria única para cada prueba usando un GUID
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_" + System.Guid.NewGuid()) // Base de datos única
                .Options;

            _context = new AppDBContext(options);
            _context.Database.EnsureCreated(); // Asegura que la base de datos esté creada antes de cada prueba
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted(); // Limpia la base de datos después de cada prueba
            _context.Dispose();
        }

        [Test]
        public async Task CanInsertUsuarioIntoDatabase()
        {
            // Arrange
            var usuario = new Usuario
            {
                Nombre_usuario = "testuser",
                email = "testuser@gmail.com",
                contraseña = "password123"
            };

            // Act
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();

            // Assert
            var usuarioEnDb = await _context.Usuarios.FirstOrDefaultAsync(u => u.email == "testuser@gmail.com");
            Assert.IsNotNull(usuarioEnDb);
            Assert.AreEqual("testuser", usuarioEnDb.Nombre_usuario);
        }

        [Test]
        public async Task SeedDataContainsAdminUser()
        {
            // Act
            var adminUser = await _context.Usuarios.FirstOrDefaultAsync(u => u.email == "admin@gmail.com");

            // Assert
            Assert.IsNotNull(adminUser);
            Assert.AreEqual("admin", adminUser.Nombre_usuario);
            Assert.AreEqual("123456", adminUser.contraseña);
        }

        [Test]
        public async Task CanInsertPacienteIntoDatabase()
        {
            // Arrange
            var paciente = new Paciente
            {
                Nombres = "John",
                ApellidoPaterno = "Doe",
                ApellidoMaterno = "Smith",
                Dni = 12345678,
                Telefono = 987654321,
                Direccion = "123 Main St",
                email = "johndoe@example.com",
                contraseña = "securepassword"
            };

            // Act
            await _context.Pacientes.AddAsync(paciente);
            await _context.SaveChangesAsync();

            // Assert
            var pacienteEnDb = await _context.Pacientes.FirstOrDefaultAsync(p => p.email == "johndoe@example.com");
            Assert.IsNotNull(pacienteEnDb);
            Assert.AreEqual("John", pacienteEnDb.Nombres);
        }

        [Test]
        public void UsuarioEntityConfiguration_IsCorrect()
        {
            // Arrange & Act
            var entityType = _context.Model.FindEntityType(typeof(Usuario));

            // Assert
            Assert.AreEqual("Usuario", entityType.GetTableName());
            Assert.AreEqual(50, entityType.FindProperty("Nombre_usuario").GetMaxLength());
            Assert.AreEqual(50, entityType.FindProperty("email").GetMaxLength());
            Assert.AreEqual(50, entityType.FindProperty("contraseña").GetMaxLength());
        }
    }
}
