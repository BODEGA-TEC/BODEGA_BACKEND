using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sibe.API.Migrations
{
    /// <inheritdoc />
    public partial class ComponentesAddColModeloCondicionEstante : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Condicion",
                table: "Componente",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Estante",
                table: "Componente",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Modelo",
                table: "Componente",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Condicion",
                table: "Componente");

            migrationBuilder.DropColumn(
                name: "Estante",
                table: "Componente");

            migrationBuilder.DropColumn(
                name: "Modelo",
                table: "Componente");
        }
    }
}
