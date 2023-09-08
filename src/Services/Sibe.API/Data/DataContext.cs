using Microsoft.EntityFrameworkCore;
using Sibe.API.Models;
using Sibe.API.Models.Enums;

namespace Sibe.API.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Estado>().HasData(
                new Estado { Id = 1, Descripcion = "EN BODEGA" },
                new Estado { Id = 2, Descripcion = "EN MANTENIMIENTO" },
                new Estado { Id = 3, Descripcion = "AGOTADO" }
            );

            modelBuilder.Entity<Categoria>().HasData(
                // COMPONENTE
                new Categoria { Id = 1, Descripcion = "TTL", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 2, Descripcion = "OPERACIONALES", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 3, Descripcion = "RESISTENCIAS", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 4, Descripcion = "POTENCIOMETROS", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 5, Descripcion = "CAPACITORES", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 6, Descripcion = "PRECISION", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 7, Descripcion = "CRISTALES", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 8, Descripcion = "CMOS", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 9, Descripcion = "BASES", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 10, Descripcion = "TECLADO HEXADECIMAL", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 11, Descripcion = "LCD", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 12, Descripcion = "OSCILOSCOPIO MINI", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 13, Descripcion = "CABLE WIRE WRAP", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 14, Descripcion = "PUERTO SERIAL", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 15, Descripcion = "DISIPADOR", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 16, Descripcion = "SENSORES", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 17, Descripcion = "TRANSISTORES/DIODOS", Tipo = TipoCategoria.COMPONENTE },
                new Categoria { Id = 18, Descripcion = "ADC/DAC", Tipo = TipoCategoria.COMPONENTE },

                // EQUIPO
                new Categoria { Id = 19, Descripcion = "MULTIMETRO", Tipo = TipoCategoria.EQUIPO },
                new Categoria { Id = 20, Descripcion = "GENERADOR FUNCIONES", Tipo = TipoCategoria.EQUIPO },
                new Categoria { Id = 21, Descripcion = "FUENTE", Tipo = TipoCategoria.EQUIPO },
                new Categoria { Id = 22, Descripcion = "MONITOR", Tipo = TipoCategoria.EQUIPO }
            );
        }

        public DbSet<Estado> Estados { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Equipo> Equipo { get; set; }
        public DbSet<Componente> Componentes { get; set; }
    }
}
