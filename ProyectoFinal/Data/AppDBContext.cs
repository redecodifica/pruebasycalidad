using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Models;

namespace ProyectoFinal.Data
{
    public class AppDBContext : DbContext // Asegúrate de que herede de DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Analisis> AnalisisC { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configuración de la entidad Usuario
            modelBuilder.Entity<Usuario>(tb =>
            {
                tb.HasKey(col => col.Id_usuario);
                tb.Property(col => col.Id_usuario).UseIdentityColumn().ValueGeneratedOnAdd();
                tb.Property(col => col.Nombre_usuario).HasMaxLength(50);
                tb.Property(col => col.email).HasMaxLength(50);
                tb.Property(col => col.contraseña).HasMaxLength(50);

                tb.HasData(new Usuario
                {
                    Id_usuario = 1,
                    Nombre_usuario = "admin",
                    email = "admin@gmail.com",
                    contraseña = "123456"
                });
            });

            // Configuración de la entidad Paciente
            modelBuilder.Entity<Paciente>(tb =>
            {
                tb.HasKey(col => col.idPaciente);
                tb.Property(col => col.idPaciente).UseIdentityColumn().ValueGeneratedOnAdd();
                tb.Property(col => col.Nombres).HasMaxLength(50);
                tb.Property(col => col.ApellidoPaterno).HasMaxLength(50);
                tb.Property(col => col.ApellidoMaterno).HasMaxLength(50);
                tb.Property(col => col.Dni).HasMaxLength(8);
                tb.Property(col => col.Telefono).HasMaxLength(10);
                tb.Property(col => col.Direccion).HasMaxLength(50);
                tb.Property(col => col.email).HasMaxLength(50);
                tb.Property(col => col.contraseña).HasMaxLength(50);
            });

            // Configuración de la entidad Analisis
            modelBuilder.Entity<Analisis>(tb =>
            {
                tb.HasKey(col => col.idAnalisis);
                tb.Property(col => col.idAnalisis).UseIdentityColumn().ValueGeneratedOnAdd();
                tb.Property(col => col.NombreAnalisis).HasMaxLength(50);
                tb.Property(col => col.Precio).HasMaxLength(50);
            });

            modelBuilder.Entity<Paciente>().ToTable("Paciente");
            modelBuilder.Entity<Usuario>().ToTable("Usuario");
            modelBuilder.Entity<Analisis>().ToTable("Analisis");
        }
    }
}
