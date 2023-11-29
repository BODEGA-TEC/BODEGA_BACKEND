using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sibe.API.Migrations
{
    /// <inheritdoc />
    public partial class ComponentesAddColCantidadDisponible : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Cantidad",
                table: "Componente",
                newName: "CantidadTotal");

            migrationBuilder.AddColumn<int>(
                name: "CantidadDisponible",
                table: "Componente",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 2,
                column: "Nombre",
                value: "PROFESOR");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CantidadDisponible",
                table: "Componente");

            migrationBuilder.RenameColumn(
                name: "CantidadTotal",
                table: "Componente",
                newName: "Cantidad");

            migrationBuilder.UpdateData(
                table: "Rol",
                keyColumn: "Id",
                keyValue: 2,
                column: "Nombre",
                value: "DESARROLLADOR");
        }
    }
}
