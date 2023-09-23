using Microsoft.EntityFrameworkCore;
using Sibe.API.Models;
using Sibe.API.Models.Enums;

namespace Sibe.API.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
            Categoria = Set<Categoria>();
            Componente = Set<Componente>();
            PrestamoEstudiante = Set<PrestamoEstudiante>();
            PrestamoProfesor = Set<PrestamoProfesor>();
            Equipo = Set<Equipo>();
            Departamento = Set<Departamento>();
            Estado = Set<Estado>();
            HistorialEquipo = Set<HistorialEquipo>();
            Profesor = Set<Profesor>();
            Rol = Set<Rol>();
            Usuario = Set<Usuario>();
        }

        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Componente> Componente { get; set; }
        public DbSet<PrestamoEstudiante> PrestamoEstudiante { get; set; }
        public DbSet<PrestamoProfesor> PrestamoProfesor { get; set; }
        public DbSet<Equipo> Equipo { get; set; }
        public DbSet<Departamento> Departamento { get; set; }
        public DbSet<Estado> Estado { get; set; }
        public DbSet<HistorialEquipo> HistorialEquipo { get; set; }
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

            // Tablas de relacion para los comprobantes
            modelBuilder.Entity<PrestamoEstudiante>()
                .HasMany(pe => pe.Componentes) // lista de componentes prestados
                .WithMany()
                .UsingEntity(j =>
                {
                    j.ToTable("PrestamoEstudianteComponente");
                    j.Property<int>("PrestamoEstudianteId"); 
                    j.Property<int>("ComponenteId"); 
                    j.HasKey("PrestamoEstudianteId", "ComponenteId"); // Clave primaria compuesta
                });

            modelBuilder.Entity<PrestamoEstudiante>()
                .HasMany(pe => pe.Equipo) // Lista de equipo prestado
                .WithMany() 
                .UsingEntity(j =>
                {
                    j.ToTable("PrestamoEstudianteEquipo");
                    j.Property<int>("PrestamoEstudianteId");
                    j.Property<int>("EquipoId"); 
                    j.HasKey("PrestamoEstudianteId", "EquipoId");
                });

            modelBuilder.Entity<PrestamoProfesor>()
                .HasMany(pp => pp.Componentes) // Lista de componentes prestados
                .WithMany() 
                .UsingEntity(j =>
                {
                    j.ToTable("PrestamoProfesorComponente");
                    j.Property<int>("PrestamoProfesorId"); 
                    j.Property<int>("ComponenteId"); 
                    j.HasKey("PrestamoProfesorId", "ComponenteId"); // Clave primaria compuesta
                });

            modelBuilder.Entity<PrestamoProfesor>()
                .HasMany(pp => pp.Equipo) // Lista de equipo prestado
                .WithMany() 
                .UsingEntity(j =>
                {
                    j.ToTable("PrestamoProfesorEquipo");
                    j.Property<int>("PrestamoProfesorId");
                    j.Property<int>("EquipoId");
                    j.HasKey("PrestamoProfesorId", "EquipoId"); // Clave primaria compuesta
                });


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
                new Departamento { Id = 3, Nombre = "AREA ACADEMICA DE INGENIERIA EN COMPUTADORES" }                
            );
        }
    }
}
