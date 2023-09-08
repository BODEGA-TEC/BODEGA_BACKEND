using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sibe.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipo_Estados_EstadoId",
                table: "Equipo");

            migrationBuilder.AlterColumn<string>(
                name: "Observaciones",
                table: "Equipo",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "EstadoId",
                table: "Equipo",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Observaciones",
                table: "Componentes",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipo_Estados_EstadoId",
                table: "Equipo",
                column: "EstadoId",
                principalTable: "Estados",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipo_Estados_EstadoId",
                table: "Equipo");

            migrationBuilder.UpdateData(
                table: "Equipo",
                keyColumn: "Observaciones",
                keyValue: null,
                column: "Observaciones",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Observaciones",
                table: "Equipo",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "EstadoId",
                table: "Equipo",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Componentes",
                keyColumn: "Observaciones",
                keyValue: null,
                column: "Observaciones",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Observaciones",
                table: "Componentes",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipo_Estados_EstadoId",
                table: "Equipo",
                column: "EstadoId",
                principalTable: "Estados",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
