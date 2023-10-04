using Microsoft.EntityFrameworkCore;
using Sibe.API.Models.Comprobantes;
using Sibe.API.Models.Entidades;
using Sibe.API.Models.Enums;
using Sibe.API.Models.Historicos;
using Sibe.API.Models.Inventario;

namespace Sibe.API.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
            Categoria = Set<Categoria>();
            Componente = Set<Componente>();
            PrestamoEstudiante = Set<BoletaPrestamoEstudiante>();
            PrestamoProfesor = Set<BoletaPrestamoProfesor>();
            Equipo = Set<Equipo>();
            Departamento = Set<Departamento>();
            Estado = Set<Estado>();
            HistoricoEquipo = Set<HistoricoEquipo>();
            Profesor = Set<Profesor>();
            Rol = Set<Rol>();
            Usuario = Set<Usuario>();
        }

        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Componente> Componente { get; set; }
        public DbSet<BoletaPrestamoEstudiante> PrestamoEstudiante { get; set; }
        public DbSet<BoletaPrestamoProfesor> PrestamoProfesor { get; set; }
        public DbSet<Equipo> Equipo { get; set; }
        public DbSet<Departamento> Departamento { get; set; }
        public DbSet<Estado> Estado { get; set; }
        public DbSet<HistoricoEquipo> HistoricoEquipo { get; set; }
        public DbSet<Profesor> Profesor { get; set; }
        public DbSet<Rol> Rol { get; set; }
        public DbSet<Usuario> Usuario { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Atributos importantes de settear utilizando Fluent API
            modelBuilder.Entity<Rol>()
                .HasIndex(r => r.Nombre)
                .IsUnique();

            modelBuilder.Entity<Categoria>()
                .HasIndex(c => c.Nombre)
                .IsUnique();

            modelBuilder.Entity<Componente>()
                .HasIndex(c => c.ActivoBodega)
                .IsUnique();
            modelBuilder.Entity<Componente>()
                .HasIndex(c => c.ActivoTec)
                .IsUnique();

            modelBuilder.Entity<Equipo>()
                .HasIndex(e => e.ActivoBodega)
                .IsUnique();
            modelBuilder.Entity<Equipo>()
                .HasIndex(e => e.ActivoTec)
                .IsUnique();

            // Configuración de las relaciones en las boletas
            modelBuilder.Entity<BoletaPrestamoEstudiante>()
                .HasOne(b => b.ProfesorAutorizador)
                .WithMany()
                .HasForeignKey(b => b.ProfesorAutorizadorId);

            modelBuilder.Entity<BoletaPrestamoProfesor>()
                .HasOne(b => b.Profesor)
                .WithMany()
                .HasForeignKey(b => b.ProfesorId);

            // Roles iniciales
            modelBuilder.Entity<Rol>().HasData(
                new Rol { Id = 1, Nombre = "ADMINISTRADOR", Descripcion = "Administra usuarios y gestiona información sensible." },
                new Rol { Id = 2, Nombre = "DESARROLLADOR", Descripcion = "Desarrollo y mantenimiento del sistema." },
                new Rol { Id = 3, Nombre = "ASISTENTE", Descripcion = "Asistencia y tareas administrativas." }
            );

            // Estados iniciales
            modelBuilder.Entity<Estado>().HasData(
                new Estado { Id = 1, Descripcion = "DISPONIBLE" },
                new Estado { Id = 2, Descripcion = "PRESTADO" },
                new Estado { Id = 3, Descripcion = "AGOTADO" },
                new Estado { Id = 4, Descripcion = "DAÑADO" },
                new Estado { Id = 5, Descripcion = "EN REPARACION" },
                new Estado { Id = 6, Descripcion = "RETIRADO" },
                new Estado { Id = 7, Descripcion = "APARTADO" }
            );

            // Categorias iniciales
            modelBuilder.Entity<Categoria>().HasData(
                // COMPONENTE
                new Categoria { Id = 1, Nombre = "TTL", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 2, Nombre = "OPERACIONALES", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 3, Nombre = "RESISTENCIAS", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 4, Nombre = "POTENCIOMETROS", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 5, Nombre = "CAPACITORES", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 6, Nombre = "PRECISION", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 7, Nombre = "CRISTALES", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 8, Nombre = "CMOS", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 9, Nombre = "BASES", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 10, Nombre = "TECLADO HEXADECIMAL", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 11, Nombre = "LCD", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 12, Nombre = "OSCILOSCOPIO MINI", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 13, Nombre = "CABLE WIRE WRAP", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 14, Nombre = "PUERTO SERIAL", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 15, Nombre = "DISIPADOR", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 16, Nombre = "SENSORES", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 17, Nombre = "TRANSISTORES/DIODOS", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 18, Nombre = "ADC/DAC", Tipo = TipoCategoria.COMPONENTE },
                // EQUIPO
                new Categoria { Id = 19, Nombre = "MULTIMETRO", Tipo = TipoCategoria.EQUIPO },
                new Categoria { Id = 20, Nombre = "GENERADOR FUNCIONES", Tipo = TipoCategoria.EQUIPO },
                new Categoria { Id = 21, Nombre = "FUENTE", Tipo = TipoCategoria.EQUIPO },
                new Categoria { Id = 22, Nombre = "MONITOR", Tipo = TipoCategoria.EQUIPO }
            );

            // Escuelas inicales
            modelBuilder.Entity<Departamento>().HasData(
                new Departamento { Id = 1, Nombre = "ESCUELA DE ELETRONICA" },
                new Departamento { Id = 2, Nombre = "ESCUELA DE MECATRONICA" },
                new Departamento { Id = 3, Nombre = "ESCUELA DE COMPUTADORES" }                
            );
        }
    }
}
