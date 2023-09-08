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

            //modelBuilder.Entity<Componente>().HasData(
            //    new Componente { Id = 1, Activo = "IRG4BC10k-ND", Descripción = "Insulated Gate Bipolar Transistor n-channel (IGBT)", CategoriaId = 17 },
            //    new Componente { Id = 2, Activo = "T106D1", Descripción = "Sensible SCR (Rectificador Controlado de Silicio)", CategoriaId = 17 },
            //    new Componente { Id = 3, Activo = "MUR160", Descripción = "1.0 A Super Fast Rectifier", CategoriaId = 17 },
            //    new Componente { Id = 4, Activo = "4000", Descripción = "Dual 3-Input NOR Gate + 1 Inverter", CategoriaId = 8 },
            //    new Componente { Id = 5, Activo = "4001", Descripción = "Quad 2-Input NOR Gate", CategoriaId = 8 },
            //    new Componente { Id = 6, Activo = "100", Descripción = "Potenciómetro 100 Ohm", CategoriaId = 5 },
            //    new Componente { Id = 7, Activo = "500", Descripción = "Potenciómetro 500 Ohm", CategoriaId = 5 },
            //    new Componente { Id = 8, Activo = "1k", Descripción = "Potenciómetro 1k Ohm", CategoriaId = 5 },
            //    new Componente { Id = 9, Activo = "OP07", Descripción = "Ultalow Offset Voltage Op-Amp", CategoriaId = 2 },
            //    new Componente { Id = 10, Activo = "MFLOCN", Descripción = "Dual Universal Switched Capacitor Filter", CategoriaId = 2 }
            //);

            //modelBuilder.Entity<Equipo>().HasData(
            //    new Equipo { Id = 1, Activo = "AI-1279", CategoriaId = 19, Descripción = "Multímetro", Marca = "KI", EstadoId = 1},
            //    new Equipo { Id = 2, Activo = "AI-1419", CategoriaId = 19, Descripción = "Multímetro", Marca = "KI", EstadoId = 2},
            //    new Equipo { Id = 3, Activo = "AI-1097", CategoriaId = 19, Descripción = "Multímetro", Marca = "KI", EstadoId = 1},
            //    new Equipo { Id = 4, Activo = "AI-1237", CategoriaId = 19, Descripción = "Multímetro", Marca = "KI", EstadoId = 2},
            //    new Equipo { Id = 5, Activo = "AI-1176", CategoriaId = 19, Descripción = "Multímetro", Marca = "KI", EstadoId = 1},
            //    new Equipo { Id = 6, Activo = "AI-1238", CategoriaId = 19, Descripción = "Multímetro", Marca = "KI", EstadoId = 1},
            //    new Equipo { Id = 7, Activo = "AI-1242", CategoriaId = 19, Descripción = "Multímetro", Marca = "KI", EstadoId = 2},
            //    new Equipo { Id = 8, Activo = "AI-1062", CategoriaId = 19, Descripción = "Multímetro", Marca = "KI", EstadoId = 2},
            //    new Equipo { Id = 9, Activo = "AI-1181", CategoriaId = 19, Descripción = "Multímetro", Marca = "KI", EstadoId = 1},
            //    new Equipo { Id = 10, Activo = "AI-1252", CategoriaId = 19, Descripción = "Multímetro", Marca = "KI", EstadoId = 1},
            //    new Equipo { Id = 11, Activo = "AI-1235", CategoriaId = 19, Descripción = "Multímetro", Marca = "KI", EstadoId = 1},
            //    new Equipo { Id = 12, Activo = "AI-1060", CategoriaId = 19, Descripción = "Multímetro", Marca = "KI", EstadoId = 1},
            //    new Equipo { Id = 13, Activo = "AI-1284", CategoriaId = 20, Descripción = "Generador Funciones", Marca = "KI", EstadoId = 1},
            //    new Equipo { Id = 14, Activo = "AI-1190", CategoriaId = 20, Descripción = "Generador Funciones", Marca = "KI", EstadoId = 1},
            //    new Equipo { Id = 15, Activo = "AI-1184", CategoriaId = 20, Descripción = "Generador Funciones", Marca = "KI", EstadoId = 2},
            //    new Equipo { Id = 16, Activo = "AI-1287", CategoriaId = 20, Descripción = "Generador Funciones", Marca = "KI", EstadoId = 1},
            //    new Equipo { Id = 17, Activo = "AI-1286", CategoriaId = 20, Descripción = "Generador Funciones", Marca = "KI", EstadoId = 2},
            //    new Equipo { Id = 18, Activo = "AI-1227", CategoriaId = 20, Descripción = "Generador Funciones", Marca = "KI", EstadoId = 1},
            //    new Equipo { Id = 19, Activo = "AI-1186", CategoriaId = 20, Descripción = "Generador Funciones", Marca = "KI", EstadoId = 2},
            //    new Equipo { Id = 20, Activo = "AI-1231", CategoriaId = 20, Descripción = "Generador Funciones", Marca = "KI", EstadoId = 3},
            //    new Equipo { Id = 21, Activo = "AI-1230", CategoriaId = 20, Descripción = "Generador Funciones", Marca = "KI", EstadoId = 1},
            //    new Equipo { Id = 22, Activo = "AI-1065", CategoriaId = 20, Descripción = "Generador Funciones", Marca = "KI", EstadoId = 1},
            //    new Equipo { Id = 23, Activo = "AI-1232", CategoriaId = 20, Descripción = "Generador Funciones", Marca = "KI", EstadoId = 1},
            //    new Equipo { Id = 24, Activo = "24711", CategoriaId = 20, Descripción = "Generador Funciones", Marca = "GW", EstadoId = 1},
            //    new Equipo { Id = 25, Activo = "3590", CategoriaId = 21, Descripción = "Fuente", EstadoId = 1},
            //    new Equipo { Id = 26, Activo = "3588", CategoriaId = 21, Descripción = "Fuente", EstadoId = 1},
            //    new Equipo { Id = 27, Activo = "37979", CategoriaId = 22, Descripción = "Monitor", Marca = "DELL", EstadoId = 1 },
            //    new Equipo { Id = 28, Activo = "37996", CategoriaId = 22, Descripción = "Monitor", Marca = "DELL", EstadoId = 1},
            //    new Equipo { Id = 29, Activo = "37994", CategoriaId = 22, Descripción = "Monitor", Marca = "DELL", EstadoId = 3}
            // );
        }

        public DbSet<Estado> Estados { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Equipo> Equipo { get; set; }
        public DbSet<Componente> Componentes { get; set; }
    }
}
