using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sibe.API.Migrations
{
    /// <inheritdoc />
    public partial class EquipoAddColsCondicionEstante : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Condicion",
                table: "Equipo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Estante",
                table: "Equipo",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Condicion",
                table: "Equipo");

            migrationBuilder.DropColumn(
                name: "Estante",
                table: "Equipo");
        }
    }
}
