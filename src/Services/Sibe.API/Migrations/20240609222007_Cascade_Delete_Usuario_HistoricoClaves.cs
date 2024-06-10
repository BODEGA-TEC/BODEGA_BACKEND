using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sibe.API.Migrations
{
    /// <inheritdoc />
    public partial class Cascade_Delete_Usuario_HistoricoClaves : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoricoClave_Usuario_UsuarioId",
                table: "HistoricoClave");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricoClave_Usuario_UsuarioId",
                table: "HistoricoClave",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoricoClave_Usuario_UsuarioId",
                table: "HistoricoClave");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricoClave_Usuario_UsuarioId",
                table: "HistoricoClave",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "Id");
        }
    }
}
