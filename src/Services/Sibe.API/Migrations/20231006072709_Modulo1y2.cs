using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Sibe.API.Migrations
{
    /// <inheritdoc />
    public partial class Modulo1y2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Departamento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departamento", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Estado",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estado", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Rol",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Profesor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DepartamentoId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PrimerApellido = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SegundoApellido = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Correo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profesor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profesor_Departamento_DepartamentoId",
                        column: x => x.DepartamentoId,
                        principalTable: "Departamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Componente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    EstadoId = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Descripcion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    ActivoBodega = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ActivoTec = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Observaciones = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Componente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Componente_Categoria_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Componente_Estado_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "Estado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Equipo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    EstadoId = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Descripcion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Marca = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Modelo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ActivoBodega = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ActivoTec = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Serie = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Observaciones = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equipo_Categoria_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Equipo_Estado_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "Estado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Carne = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RolId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaveHash = table.Column<byte[]>(type: "longblob", nullable: false),
                    ClaveSalt = table.Column<byte[]>(type: "longblob", nullable: false),
                    Correo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Carne);
                    table.ForeignKey(
                        name: "FK_Usuario_Rol_RolId",
                        column: x => x.RolId,
                        principalTable: "Rol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Boleta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FechaCreacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AsistenteCarne = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Discriminator = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Carne = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProfesorAutorizadorId = table.Column<int>(type: "int", nullable: true),
                    ProfesorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boleta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Boleta_Profesor_ProfesorAutorizadorId",
                        column: x => x.ProfesorAutorizadorId,
                        principalTable: "Profesor",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Boleta_Profesor_ProfesorId",
                        column: x => x.ProfesorId,
                        principalTable: "Profesor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Boleta_Usuario_AsistenteCarne",
                        column: x => x.AsistenteCarne,
                        principalTable: "Usuario",
                        principalColumn: "Carne",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BoletaComponente",
                columns: table => new
                {
                    BoletaId = table.Column<int>(type: "int", nullable: false),
                    ComponenteId = table.Column<int>(type: "int", nullable: false),
                    CantidadPrestada = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoletaComponente", x => new { x.BoletaId, x.ComponenteId });
                    table.ForeignKey(
                        name: "FK_BoletaComponente_Boleta_BoletaId",
                        column: x => x.BoletaId,
                        principalTable: "Boleta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoletaComponente_Componente_ComponenteId",
                        column: x => x.ComponenteId,
                        principalTable: "Componente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BoletaEquipo",
                columns: table => new
                {
                    BoletaId = table.Column<int>(type: "int", nullable: false),
                    EquipoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoletaEquipo", x => new { x.BoletaId, x.EquipoId });
                    table.ForeignKey(
                        name: "FK_BoletaEquipo_Boleta_BoletaId",
                        column: x => x.BoletaId,
                        principalTable: "Boleta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoletaEquipo_Equipo_EquipoId",
                        column: x => x.EquipoId,
                        principalTable: "Equipo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HistoricoComponente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ComponenteId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Detalle = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CantidadModificada = table.Column<int>(type: "int", nullable: false),
                    CantidadDisponible = table.Column<int>(type: "int", nullable: false),
                    ComprobanteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoComponente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricoComponente_Boleta_ComprobanteId",
                        column: x => x.ComprobanteId,
                        principalTable: "Boleta",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HistoricoComponente_Componente_ComponenteId",
                        column: x => x.ComponenteId,
                        principalTable: "Componente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HistoricoEquipo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EquipoId = table.Column<int>(type: "int", nullable: false),
                    EstadoId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Detalle = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ComprobanteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoEquipo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricoEquipo_Boleta_ComprobanteId",
                        column: x => x.ComprobanteId,
                        principalTable: "Boleta",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HistoricoEquipo_Equipo_EquipoId",
                        column: x => x.EquipoId,
                        principalTable: "Equipo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistoricoEquipo_Estado_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "Estado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Categoria",
                columns: new[] { "Id", "Nombre", "Tipo" },
                values: new object[,]
                {
                    { 1, "TTL", 2 },
                    { 2, "OPERACIONALES", 2 },
                    { 3, "RESISTENCIAS", 2 },
                    { 4, "POTENCIOMETROS", 2 },
                    { 5, "CAPACITORES", 2 },
                    { 6, "PRECISION", 2 },
                    { 7, "CRISTALES", 2 },
                    { 8, "CMOS", 2 },
                    { 9, "BASES", 2 },
                    { 10, "TECLADO HEXADECIMAL", 2 },
                    { 11, "LCD", 2 },
                    { 12, "OSCILOSCOPIO MINI", 2 },
                    { 13, "CABLE WIRE WRAP", 2 },
                    { 14, "PUERTO SERIAL", 2 },
                    { 15, "DISIPADOR", 2 },
                    { 16, "SENSORES", 2 },
                    { 17, "TRANSISTORES/DIODOS", 2 },
                    { 18, "ADC/DAC", 2 },
                    { 19, "MULTIMETRO", 1 },
                    { 20, "GENERADOR FUNCIONES", 1 },
                    { 21, "FUENTE", 1 },
                    { 22, "MONITOR", 1 }
                });

            migrationBuilder.InsertData(
                table: "Departamento",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "ESCUELA DE ELETRONICA" },
                    { 2, "ESCUELA DE MECATRONICA" },
                    { 3, "ESCUELA DE COMPUTADORES" }
                });

            migrationBuilder.InsertData(
                table: "Estado",
                columns: new[] { "Id", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, "El item está disponible en bodega y listo para ser prestado a los solicitantes.", "DISPONIBLE" },
                    { 2, "El item ha sido entregado a un solicitante y está a la espera de ser devuelto.", "PRESTADO" },
                    { 3, "No quedan componentes disponibles en bodega ya que todos han sido prestados.", "AGOTADO" },
                    { 4, "El item está dañado y no puede ser prestado en su estado actual.", "DAÑADO" },
                    { 5, "El item se encuentra en mantenimiento y reparación.", "EN REPARACION" },
                    { 6, "El item ha sido retirado de la bodega, donado o descontinuado su uso.", "RETIRADO" },
                    { 7, "Un funcionario ha apartado el item y está reservado para su préstamo futuro.", "APARTADO" }
                });

            migrationBuilder.InsertData(
                table: "Rol",
                columns: new[] { "Id", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, "Administra usuarios y gestiona información sensible.", "ADMINISTRADOR" },
                    { 2, "Desarrollo y mantenimiento del sistema.", "DESARROLLADOR" },
                    { 3, "Asistencia y tareas administrativas.", "ASISTENTE" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Boleta_AsistenteCarne",
                table: "Boleta",
                column: "AsistenteCarne");

            migrationBuilder.CreateIndex(
                name: "IX_Boleta_ProfesorAutorizadorId",
                table: "Boleta",
                column: "ProfesorAutorizadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Boleta_ProfesorId",
                table: "Boleta",
                column: "ProfesorId");

            migrationBuilder.CreateIndex(
                name: "IX_BoletaComponente_ComponenteId",
                table: "BoletaComponente",
                column: "ComponenteId");

            migrationBuilder.CreateIndex(
                name: "IX_BoletaEquipo_EquipoId",
                table: "BoletaEquipo",
                column: "EquipoId");

            migrationBuilder.CreateIndex(
                name: "IX_Categoria_Nombre",
                table: "Categoria",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Componente_ActivoBodega",
                table: "Componente",
                column: "ActivoBodega",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Componente_ActivoTec",
                table: "Componente",
                column: "ActivoTec",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Componente_CategoriaId",
                table: "Componente",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Componente_EstadoId",
                table: "Componente",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipo_ActivoBodega",
                table: "Equipo",
                column: "ActivoBodega",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Equipo_ActivoTec",
                table: "Equipo",
                column: "ActivoTec",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Equipo_CategoriaId",
                table: "Equipo",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipo_EstadoId",
                table: "Equipo",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoComponente_ComponenteId",
                table: "HistoricoComponente",
                column: "ComponenteId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoComponente_ComprobanteId",
                table: "HistoricoComponente",
                column: "ComprobanteId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoEquipo_ComprobanteId",
                table: "HistoricoEquipo",
                column: "ComprobanteId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoEquipo_EquipoId",
                table: "HistoricoEquipo",
                column: "EquipoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoEquipo_EstadoId",
                table: "HistoricoEquipo",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Profesor_DepartamentoId",
                table: "Profesor",
                column: "DepartamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Rol_Nombre",
                table: "Rol",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_RolId",
                table: "Usuario",
                column: "RolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoletaComponente");

            migrationBuilder.DropTable(
                name: "BoletaEquipo");

            migrationBuilder.DropTable(
                name: "HistoricoComponente");

            migrationBuilder.DropTable(
                name: "HistoricoEquipo");

            migrationBuilder.DropTable(
                name: "Componente");

            migrationBuilder.DropTable(
                name: "Boleta");

            migrationBuilder.DropTable(
                name: "Equipo");

            migrationBuilder.DropTable(
                name: "Profesor");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropTable(
                name: "Estado");

            migrationBuilder.DropTable(
                name: "Departamento");

            migrationBuilder.DropTable(
                name: "Rol");
        }
    }
}
