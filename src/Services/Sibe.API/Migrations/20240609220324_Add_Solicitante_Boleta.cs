using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sibe.API.Migrations
{
    /// <inheritdoc />
    public partial class Add_Solicitante_Boleta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarneSolicitante",
                table: "Boleta");

            migrationBuilder.DropColumn(
                name: "CorreoSolicitante",
                table: "Boleta");

            migrationBuilder.DropColumn(
                name: "NombreSolicitante",
                table: "Boleta");

            migrationBuilder.DropColumn(
                name: "TipoSolicitante",
                table: "Boleta");

            migrationBuilder.AlterColumn<string>(
                name: "Correo",
                table: "Usuario",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SolicitanteCarne",
                table: "Boleta",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Solicitante",
                columns: table => new
                {
                    Carne = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Correo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Carrera = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tipo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solicitante", x => x.Carne);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Correo",
                table: "Usuario",
                column: "Correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Boleta_SolicitanteCarne",
                table: "Boleta",
                column: "SolicitanteCarne");

            migrationBuilder.AddForeignKey(
                name: "FK_Boleta_Solicitante_SolicitanteCarne",
                table: "Boleta",
                column: "SolicitanteCarne",
                principalTable: "Solicitante",
                principalColumn: "Carne",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boleta_Solicitante_SolicitanteCarne",
                table: "Boleta");

            migrationBuilder.DropTable(
                name: "Solicitante");

            migrationBuilder.DropIndex(
                name: "IX_Usuario_Correo",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_Boleta_SolicitanteCarne",
                table: "Boleta");

            migrationBuilder.DropColumn(
                name: "SolicitanteCarne",
                table: "Boleta");

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
                name: "CarneSolicitante",
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

            migrationBuilder.AddColumn<string>(
                name: "NombreSolicitante",
                table: "Boleta",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "TipoSolicitante",
                table: "Boleta",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
