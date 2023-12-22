using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Sibe.API.Migrations
{
    /// <inheritdoc />
    public partial class Modulo2refactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Asistente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Carne = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Correo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaRegistro = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    HuellaDigital = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asistente", x => x.Id);
                })
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
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Rol = table.Column<int>(type: "int", nullable: false),
                    Correo = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaveTemporal = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaRegistro = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
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
                    CantidadTotal = table.Column<int>(type: "int", nullable: false),
                    CantidadDisponible = table.Column<int>(type: "int", nullable: false),
                    Condicion = table.Column<int>(type: "int", nullable: false),
                    Estante = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ActivoBodega = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NoParte = table.Column<string>(type: "longtext", nullable: true)
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
                    Condicion = table.Column<int>(type: "int", nullable: false),
                    Estante = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Marca = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Modelo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ActivoBodega = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ActivoTec = table.Column<string>(type: "longtext", nullable: true)
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
                name: "Boleta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FechaEmision = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Detalle = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AsistenteId = table.Column<int>(type: "int", nullable: false),
                    CarneSolicitante = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AprobadorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boleta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Boleta_Usuario_AprobadorId",
                        column: x => x.AprobadorId,
                        principalTable: "Usuario",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Boleta_Usuario_AsistenteId",
                        column: x => x.AsistenteId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HistoricoClave",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FechaRegistro = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ClaveHash = table.Column<byte[]>(type: "longblob", nullable: false),
                    ClaveSalt = table.Column<byte[]>(type: "longblob", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoClave", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricoClave_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HistoricoRefreshToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    AccessToken = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RefreshToken = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaCreacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaExpiracion = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoRefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricoRefreshToken_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
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

            migrationBuilder.CreateIndex(
                name: "IX_Boleta_AprobadorId",
                table: "Boleta",
                column: "AprobadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Boleta_AsistenteId",
                table: "Boleta",
                column: "AsistenteId");

            migrationBuilder.CreateIndex(
                name: "IX_BoletaComponente_ComponenteId",
                table: "BoletaComponente",
                column: "ComponenteId");

            migrationBuilder.CreateIndex(
                name: "IX_BoletaEquipo_EquipoId",
                table: "BoletaEquipo",
                column: "EquipoId");

            migrationBuilder.CreateIndex(
                name: "IX_Categoria_Tipo_Nombre",
                table: "Categoria",
                columns: new[] { "Tipo", "Nombre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Componente_ActivoBodega",
                table: "Componente",
                column: "ActivoBodega",
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
                name: "IX_Equipo_CategoriaId",
                table: "Equipo",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipo_EstadoId",
                table: "Equipo",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoClave_UsuarioId",
                table: "HistoricoClave",
                column: "UsuarioId");

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
                name: "IX_HistoricoRefreshToken_UsuarioId",
                table: "HistoricoRefreshToken",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Correo",
                table: "Usuario",
                column: "Correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Username",
                table: "Usuario",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Asistente");

            migrationBuilder.DropTable(
                name: "BoletaComponente");

            migrationBuilder.DropTable(
                name: "BoletaEquipo");

            migrationBuilder.DropTable(
                name: "HistoricoClave");

            migrationBuilder.DropTable(
                name: "HistoricoComponente");

            migrationBuilder.DropTable(
                name: "HistoricoEquipo");

            migrationBuilder.DropTable(
                name: "HistoricoRefreshToken");

            migrationBuilder.DropTable(
                name: "Componente");

            migrationBuilder.DropTable(
                name: "Boleta");

            migrationBuilder.DropTable(
                name: "Equipo");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropTable(
                name: "Estado");
        }
    }
}
