using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sibe.API.Migrations
{
    /// <inheritdoc />
    public partial class BoletaPrestamo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistorialEquipo_Equipo_EquipoId",
                table: "HistorialEquipo");

            migrationBuilder.DropForeignKey(
                name: "FK_HistorialEquipo_Estado_EstadoId",
                table: "HistorialEquipo");

            migrationBuilder.DropForeignKey(
                name: "FK_HistorialEquipo_HistoricoPrestamo_HistorialPrestamoId",
                table: "HistorialEquipo");

            migrationBuilder.DropTable(
                name: "HistoricoPrestamo");

            migrationBuilder.DropTable(
                name: "PrestamoEstudianteComponente");

            migrationBuilder.DropTable(
                name: "PrestamoEstudianteEquipo");

            migrationBuilder.DropTable(
                name: "PrestamoProfesorComponente");

            migrationBuilder.DropTable(
                name: "PrestamoProfesorEquipo");

            migrationBuilder.DropTable(
                name: "PrestamoEstudiante");

            migrationBuilder.DropTable(
                name: "PrestamoProfesor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HistorialEquipo",
                table: "HistorialEquipo");

            migrationBuilder.RenameTable(
                name: "HistorialEquipo",
                newName: "HistoricoEquipo");

            migrationBuilder.RenameColumn(
                name: "HistorialPrestamoId",
                table: "HistoricoEquipo",
                newName: "ComprobanteId");

            migrationBuilder.RenameIndex(
                name: "IX_HistorialEquipo_HistorialPrestamoId",
                table: "HistoricoEquipo",
                newName: "IX_HistoricoEquipo_ComprobanteId");

            migrationBuilder.RenameIndex(
                name: "IX_HistorialEquipo_EstadoId",
                table: "HistoricoEquipo",
                newName: "IX_HistoricoEquipo_EstadoId");

            migrationBuilder.RenameIndex(
                name: "IX_HistorialEquipo_EquipoId",
                table: "HistoricoEquipo",
                newName: "IX_HistoricoEquipo_EquipoId");

            migrationBuilder.AlterColumn<string>(
                name: "ActivoTec",
                table: "Equipo",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ActivoBodega",
                table: "Equipo",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "BoletaPrestamoId",
                table: "Equipo",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ActivoTec",
                table: "Componente",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ActivoBodega",
                table: "Componente",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "BoletaPrestamoId",
                table: "Componente",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_HistoricoEquipo",
                table: "HistoricoEquipo",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BoletaPrestamo",
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
                    table.PrimaryKey("PK_BoletaPrestamo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoletaPrestamo_Profesor_ProfesorAutorizadorId",
                        column: x => x.ProfesorAutorizadorId,
                        principalTable: "Profesor",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BoletaPrestamo_Profesor_ProfesorId",
                        column: x => x.ProfesorId,
                        principalTable: "Profesor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoletaPrestamo_Usuario_AsistenteCarne",
                        column: x => x.AsistenteCarne,
                        principalTable: "Usuario",
                        principalColumn: "Carne",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Departamento",
                keyColumn: "Id",
                keyValue: 3,
                column: "Nombre",
                value: "ESCUELA DE COMPUTADORES");

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
                name: "IX_Equipo_BoletaPrestamoId",
                table: "Equipo",
                column: "BoletaPrestamoId");

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
                name: "IX_Componente_BoletaPrestamoId",
                table: "Componente",
                column: "BoletaPrestamoId");

            migrationBuilder.CreateIndex(
                name: "IX_BoletaPrestamo_AsistenteCarne",
                table: "BoletaPrestamo",
                column: "AsistenteCarne");

            migrationBuilder.CreateIndex(
                name: "IX_BoletaPrestamo_ProfesorAutorizadorId",
                table: "BoletaPrestamo",
                column: "ProfesorAutorizadorId");

            migrationBuilder.CreateIndex(
                name: "IX_BoletaPrestamo_ProfesorId",
                table: "BoletaPrestamo",
                column: "ProfesorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Componente_BoletaPrestamo_BoletaPrestamoId",
                table: "Componente",
                column: "BoletaPrestamoId",
                principalTable: "BoletaPrestamo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipo_BoletaPrestamo_BoletaPrestamoId",
                table: "Equipo",
                column: "BoletaPrestamoId",
                principalTable: "BoletaPrestamo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricoEquipo_BoletaPrestamo_ComprobanteId",
                table: "HistoricoEquipo",
                column: "ComprobanteId",
                principalTable: "BoletaPrestamo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricoEquipo_Equipo_EquipoId",
                table: "HistoricoEquipo",
                column: "EquipoId",
                principalTable: "Equipo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricoEquipo_Estado_EstadoId",
                table: "HistoricoEquipo",
                column: "EstadoId",
                principalTable: "Estado",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Componente_BoletaPrestamo_BoletaPrestamoId",
                table: "Componente");

            migrationBuilder.DropForeignKey(
                name: "FK_Equipo_BoletaPrestamo_BoletaPrestamoId",
                table: "Equipo");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoricoEquipo_BoletaPrestamo_ComprobanteId",
                table: "HistoricoEquipo");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoricoEquipo_Equipo_EquipoId",
                table: "HistoricoEquipo");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoricoEquipo_Estado_EstadoId",
                table: "HistoricoEquipo");

            migrationBuilder.DropTable(
                name: "BoletaPrestamo");

            migrationBuilder.DropIndex(
                name: "IX_Equipo_ActivoBodega",
                table: "Equipo");

            migrationBuilder.DropIndex(
                name: "IX_Equipo_ActivoTec",
                table: "Equipo");

            migrationBuilder.DropIndex(
                name: "IX_Equipo_BoletaPrestamoId",
                table: "Equipo");

            migrationBuilder.DropIndex(
                name: "IX_Componente_ActivoBodega",
                table: "Componente");

            migrationBuilder.DropIndex(
                name: "IX_Componente_ActivoTec",
                table: "Componente");

            migrationBuilder.DropIndex(
                name: "IX_Componente_BoletaPrestamoId",
                table: "Componente");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HistoricoEquipo",
                table: "HistoricoEquipo");

            migrationBuilder.DropColumn(
                name: "BoletaPrestamoId",
                table: "Equipo");

            migrationBuilder.DropColumn(
                name: "BoletaPrestamoId",
                table: "Componente");

            migrationBuilder.RenameTable(
                name: "HistoricoEquipo",
                newName: "HistorialEquipo");

            migrationBuilder.RenameColumn(
                name: "ComprobanteId",
                table: "HistorialEquipo",
                newName: "HistorialPrestamoId");

            migrationBuilder.RenameIndex(
                name: "IX_HistoricoEquipo_EstadoId",
                table: "HistorialEquipo",
                newName: "IX_HistorialEquipo_EstadoId");

            migrationBuilder.RenameIndex(
                name: "IX_HistoricoEquipo_EquipoId",
                table: "HistorialEquipo",
                newName: "IX_HistorialEquipo_EquipoId");

            migrationBuilder.RenameIndex(
                name: "IX_HistoricoEquipo_ComprobanteId",
                table: "HistorialEquipo",
                newName: "IX_HistorialEquipo_HistorialPrestamoId");

            migrationBuilder.AlterColumn<string>(
                name: "ActivoTec",
                table: "Equipo",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ActivoBodega",
                table: "Equipo",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ActivoTec",
                table: "Componente",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ActivoBodega",
                table: "Componente",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HistorialEquipo",
                table: "HistorialEquipo",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PrestamoEstudiante",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AsistenteCarne = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProfesorAutorizadorId = table.Column<int>(type: "int", nullable: true),
                    Descripcion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EstudianteCarne = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false)
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
                    AsistenteCarne = table.Column<string>(type: "varchar(10)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProfesorId = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.UpdateData(
                table: "Departamento",
                keyColumn: "Id",
                keyValue: 3,
                column: "Nombre",
                value: "AREA ACADEMICA DE INGENIERIA EN COMPUTADORES");

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

            migrationBuilder.AddForeignKey(
                name: "FK_HistorialEquipo_Equipo_EquipoId",
                table: "HistorialEquipo",
                column: "EquipoId",
                principalTable: "Equipo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HistorialEquipo_Estado_EstadoId",
                table: "HistorialEquipo",
                column: "EstadoId",
                principalTable: "Estado",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HistorialEquipo_HistoricoPrestamo_HistorialPrestamoId",
                table: "HistorialEquipo",
                column: "HistorialPrestamoId",
                principalTable: "HistoricoPrestamo",
                principalColumn: "Id");
        }
    }
}
