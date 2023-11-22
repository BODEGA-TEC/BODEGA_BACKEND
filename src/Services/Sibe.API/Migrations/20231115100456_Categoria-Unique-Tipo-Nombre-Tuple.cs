using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sibe.API.Migrations
{
    /// <inheritdoc />
    public partial class CategoriaUniqueTipoNombreTuple : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Equipo_ActivoTec",
                table: "Equipo");

            migrationBuilder.DropIndex(
                name: "IX_Componente_ActivoTec",
                table: "Componente");

            migrationBuilder.DropIndex(
                name: "IX_Categoria_Nombre",
                table: "Categoria");

            migrationBuilder.DropColumn(
                name: "ActivoTec",
                table: "Componente");

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

            migrationBuilder.CreateIndex(
                name: "IX_Categoria_Tipo_Nombre",
                table: "Categoria",
                columns: new[] { "Tipo", "Nombre" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categoria_Tipo_Nombre",
                table: "Categoria");

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

            migrationBuilder.AddColumn<string>(
                name: "ActivoTec",
                table: "Componente",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Equipo_ActivoTec",
                table: "Equipo",
                column: "ActivoTec",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Componente_ActivoTec",
                table: "Componente",
                column: "ActivoTec",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categoria_Nombre",
                table: "Categoria",
                column: "Nombre",
                unique: true);
        }
    }
}
