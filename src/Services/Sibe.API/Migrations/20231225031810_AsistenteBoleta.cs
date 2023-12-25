using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sibe.API.Migrations
{
    /// <inheritdoc />
    public partial class AsistenteBoleta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boleta_Usuario_AprobadorId",
                table: "Boleta");

            migrationBuilder.DropForeignKey(
                name: "FK_Boleta_Usuario_AsistenteId",
                table: "Boleta");

            migrationBuilder.DropIndex(
                name: "IX_Usuario_Correo",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_Boleta_AprobadorId",
                table: "Boleta");

            migrationBuilder.DropIndex(
                name: "IX_Boleta_AsistenteId",
                table: "Boleta");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Asistente",
                table: "Asistente");

            migrationBuilder.DropColumn(
                name: "AprobadorId",
                table: "Boleta");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Asistente");

            migrationBuilder.RenameColumn(
                name: "CantidadPrestada",
                table: "BoletaComponente",
                newName: "Cantidad");

            migrationBuilder.RenameColumn(
                name: "Tipo",
                table: "Boleta",
                newName: "TipoSolicitante");

            migrationBuilder.RenameColumn(
                name: "Detalle",
                table: "Boleta",
                newName: "NombreSolicitante");

            migrationBuilder.RenameColumn(
                name: "AsistenteId",
                table: "Boleta",
                newName: "TipoBoleta");

            migrationBuilder.AlterColumn<string>(
                name: "Correo",
                table: "Usuario",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Observaciones",
                table: "BoletaEquipo",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Observaciones",
                table: "BoletaComponente",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CarneAsistente",
                table: "Boleta",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Consecutivo",
                table: "Boleta",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CorreoSolicitante",
                table: "Boleta",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Estado",
                table: "Boleta",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NombreAsistente",
                table: "Boleta",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Asistente",
                table: "Asistente",
                column: "Carne");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Asistente",
                table: "Asistente");

            migrationBuilder.DropColumn(
                name: "Observaciones",
                table: "BoletaEquipo");

            migrationBuilder.DropColumn(
                name: "Observaciones",
                table: "BoletaComponente");

            migrationBuilder.DropColumn(
                name: "CarneAsistente",
                table: "Boleta");

            migrationBuilder.DropColumn(
                name: "Consecutivo",
                table: "Boleta");

            migrationBuilder.DropColumn(
                name: "CorreoSolicitante",
                table: "Boleta");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Boleta");

            migrationBuilder.DropColumn(
                name: "NombreAsistente",
                table: "Boleta");

            migrationBuilder.RenameColumn(
                name: "Cantidad",
                table: "BoletaComponente",
                newName: "CantidadPrestada");

            migrationBuilder.RenameColumn(
                name: "TipoSolicitante",
                table: "Boleta",
                newName: "Tipo");

            migrationBuilder.RenameColumn(
                name: "TipoBoleta",
                table: "Boleta",
                newName: "AsistenteId");

            migrationBuilder.RenameColumn(
                name: "NombreSolicitante",
                table: "Boleta",
                newName: "Detalle");

            migrationBuilder.AlterColumn<string>(
                name: "Correo",
                table: "Usuario",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "AprobadorId",
                table: "Boleta",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Asistente",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Asistente",
                table: "Asistente",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Correo",
                table: "Usuario",
                column: "Correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Boleta_AprobadorId",
                table: "Boleta",
                column: "AprobadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Boleta_AsistenteId",
                table: "Boleta",
                column: "AsistenteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Boleta_Usuario_AprobadorId",
                table: "Boleta",
                column: "AprobadorId",
                principalTable: "Usuario",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Boleta_Usuario_AsistenteId",
                table: "Boleta",
                column: "AsistenteId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
