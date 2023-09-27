using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Sibe.API.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
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
                    ActivoBodega = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    ActivoTec = table.Column<string>(type: "longtext", nullable: true)
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
                    ActivoBodega = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Marca = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Modelo = table.Column<string>(type: "longtext", nullable: true)
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
                name: "PrestamoEstudiante",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AsistenteCarne = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EstudianteCarne = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProfesorAutorizadorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrestamoEstudiante", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrestamoEstudiante_Profesor_ProfesorAutorizadorId",
                        column: x => x.ProfesorAutorizadorId,
                        principalTable: "Profesor",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PrestamoEstudiante_Usuario_AsistenteCarne",
                        column: x => x.AsistenteCarne,
                        principalTable: "Usuario",
                        principalColumn: "Carne",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PrestamoProfesor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AsistenteCarne = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProfesorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrestamoProfesor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrestamoProfesor_Profesor_ProfesorId",
                        column: x => x.ProfesorId,
                        principalTable: "Profesor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrestamoProfesor_Usuario_AsistenteCarne",
                        column: x => x.AsistenteCarne,
                        principalTable: "Usuario",
                        principalColumn: "Carne",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PrestamoEstudianteComponente",
                columns: table => new
                {
                    PrestamoEstudianteId = table.Column<int>(type: "int", nullable: false),
                    ComponenteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrestamoEstudianteComponente", x => new { x.PrestamoEstudianteId, x.ComponenteId });
                    table.ForeignKey(
                        name: "FK_PrestamoEstudianteComponente_Componente_ComponenteId",
                        column: x => x.ComponenteId,
                        principalTable: "Componente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrestamoEstudianteComponente_PrestamoEstudiante_PrestamoEstu~",
                        column: x => x.PrestamoEstudianteId,
                        principalTable: "PrestamoEstudiante",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PrestamoEstudianteEquipo",
                columns: table => new
                {
                    PrestamoEstudianteId = table.Column<int>(type: "int", nullable: false),
                    EquipoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrestamoEstudianteEquipo", x => new { x.PrestamoEstudianteId, x.EquipoId });
                    table.ForeignKey(
                        name: "FK_PrestamoEstudianteEquipo_Equipo_EquipoId",
                        column: x => x.EquipoId,
                        principalTable: "Equipo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrestamoEstudianteEquipo_PrestamoEstudiante_PrestamoEstudian~",
                        column: x => x.PrestamoEstudianteId,
                        principalTable: "PrestamoEstudiante",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HistoricoPrestamo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PrestamoEstudianteId = table.Column<int>(type: "int", nullable: true),
                    PrestamoProfesorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoPrestamo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricoPrestamo_PrestamoEstudiante_PrestamoEstudianteId",
                        column: x => x.PrestamoEstudianteId,
                        principalTable: "PrestamoEstudiante",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HistoricoPrestamo_PrestamoProfesor_PrestamoProfesorId",
                        column: x => x.PrestamoProfesorId,
                        principalTable: "PrestamoProfesor",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PrestamoProfesorComponente",
                columns: table => new
                {
                    PrestamoProfesorId = table.Column<int>(type: "int", nullable: false),
                    ComponenteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrestamoProfesorComponente", x => new { x.PrestamoProfesorId, x.ComponenteId });
                    table.ForeignKey(
                        name: "FK_PrestamoProfesorComponente_Componente_ComponenteId",
                        column: x => x.ComponenteId,
                        principalTable: "Componente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrestamoProfesorComponente_PrestamoProfesor_PrestamoProfesor~",
                        column: x => x.PrestamoProfesorId,
                        principalTable: "PrestamoProfesor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PrestamoProfesorEquipo",
                columns: table => new
                {
                    PrestamoProfesorId = table.Column<int>(type: "int", nullable: false),
                    EquipoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrestamoProfesorEquipo", x => new { x.PrestamoProfesorId, x.EquipoId });
                    table.ForeignKey(
                        name: "FK_PrestamoProfesorEquipo_Equipo_EquipoId",
                        column: x => x.EquipoId,
                        principalTable: "Equipo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrestamoProfesorEquipo_PrestamoProfesor_PrestamoProfesorId",
                        column: x => x.PrestamoProfesorId,
                        principalTable: "PrestamoProfesor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HistorialEquipo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EquipoId = table.Column<int>(type: "int", nullable: false),
                    EstadoId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Detalle = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HistorialPrestamoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialEquipo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistorialEquipo_Equipo_EquipoId",
                        column: x => x.EquipoId,
                        principalTable: "Equipo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistorialEquipo_Estado_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "Estado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistorialEquipo_HistoricoPrestamo_HistorialPrestamoId",
                        column: x => x.HistorialPrestamoId,
                        principalTable: "HistoricoPrestamo",
                        principalColumn: "Id");
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
                    { 3, "AREA ACADEMICA DE INGENIERIA EN COMPUTADORES" }
                });

            migrationBuilder.InsertData(
                table: "Estado",
                columns: new[] { "Id", "Descripcion" },
                values: new object[,]
                {
                    { 1, "DISPONIBLE" },
                    { 2, "PRESTADO" },
                    { 3, "AGOTADO" },
                    { 4, "DAÑADO" },
                    { 5, "EN REPARACION" },
                    { 6, "RETIRADO" },
                    { 7, "APARTADO" }
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
                name: "IX_Categoria_Nombre",
                table: "Categoria",
                column: "Nombre",
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
                name: "IX_Equipo_CategoriaId",
                table: "Equipo",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipo_EstadoId",
                table: "Equipo",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialEquipo_EquipoId",
                table: "HistorialEquipo",
                column: "EquipoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialEquipo_EstadoId",
                table: "HistorialEquipo",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialEquipo_HistorialPrestamoId",
                table: "HistorialEquipo",
                column: "HistorialPrestamoId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoPrestamo_PrestamoEstudianteId",
                table: "HistoricoPrestamo",
                column: "PrestamoEstudianteId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoPrestamo_PrestamoProfesorId",
                table: "HistoricoPrestamo",
                column: "PrestamoProfesorId");

            migrationBuilder.CreateIndex(
                name: "IX_PrestamoEstudiante_AsistenteCarne",
                table: "PrestamoEstudiante",
                column: "AsistenteCarne");

            migrationBuilder.CreateIndex(
                name: "IX_PrestamoEstudiante_ProfesorAutorizadorId",
                table: "PrestamoEstudiante",
                column: "ProfesorAutorizadorId");

            migrationBuilder.CreateIndex(
                name: "IX_PrestamoEstudianteComponente_ComponenteId",
                table: "PrestamoEstudianteComponente",
                column: "ComponenteId");

            migrationBuilder.CreateIndex(
                name: "IX_PrestamoEstudianteEquipo_EquipoId",
                table: "PrestamoEstudianteEquipo",
                column: "EquipoId");

            migrationBuilder.CreateIndex(
                name: "IX_PrestamoProfesor_AsistenteCarne",
                table: "PrestamoProfesor",
                column: "AsistenteCarne");

            migrationBuilder.CreateIndex(
                name: "IX_PrestamoProfesor_ProfesorId",
                table: "PrestamoProfesor",
                column: "ProfesorId");

            migrationBuilder.CreateIndex(
                name: "IX_PrestamoProfesorComponente_ComponenteId",
                table: "PrestamoProfesorComponente",
                column: "ComponenteId");

            migrationBuilder.CreateIndex(
                name: "IX_PrestamoProfesorEquipo_EquipoId",
                table: "PrestamoProfesorEquipo",
                column: "EquipoId");

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
                name: "HistorialEquipo");

            migrationBuilder.DropTable(
                name: "PrestamoEstudianteComponente");

            migrationBuilder.DropTable(
                name: "PrestamoEstudianteEquipo");

            migrationBuilder.DropTable(
                name: "PrestamoProfesorComponente");

            migrationBuilder.DropTable(
                name: "PrestamoProfesorEquipo");

            migrationBuilder.DropTable(
                name: "HistoricoPrestamo");

            migrationBuilder.DropTable(
                name: "Componente");

            migrationBuilder.DropTable(
                name: "Equipo");

            migrationBuilder.DropTable(
                name: "PrestamoEstudiante");

            migrationBuilder.DropTable(
                name: "PrestamoProfesor");

            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropTable(
                name: "Estado");

            migrationBuilder.DropTable(
                name: "Profesor");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Departamento");

            migrationBuilder.DropTable(
                name: "Rol");
        }
    }
}
