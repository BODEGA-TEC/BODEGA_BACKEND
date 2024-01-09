using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sibe.API.Migrations
{
    /// <inheritdoc />
    public partial class PinAsistente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HuellaDigital",
                table: "Asistente");

            migrationBuilder.UpdateData(
                table: "Boleta",
                keyColumn: "CarneSolicitante",
                keyValue: null,
                column: "CarneSolicitante",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "CarneSolicitante",
                table: "Boleta",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Fingerprint",
                table: "Asistente",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<byte[]>(
                name: "PinHash",
                table: "Asistente",
                type: "longblob",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PinSalt",
                table: "Asistente",
                type: "longblob",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fingerprint",
                table: "Asistente");

            migrationBuilder.DropColumn(
                name: "PinHash",
                table: "Asistente");

            migrationBuilder.DropColumn(
                name: "PinSalt",
                table: "Asistente");

            migrationBuilder.AlterColumn<string>(
                name: "CarneSolicitante",
                table: "Boleta",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "HuellaDigital",
                table: "Asistente",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
