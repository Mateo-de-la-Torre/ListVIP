using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovePortero : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegistrosIngreso_Usuarios_PorteroId",
                table: "RegistrosIngreso");

            migrationBuilder.DropIndex(
                name: "IX_RegistrosIngreso_PorteroId",
                table: "RegistrosIngreso");

            migrationBuilder.DropColumn(
                name: "PorteroId",
                table: "RegistrosIngreso");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PorteroId",
                table: "RegistrosIngreso",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosIngreso_PorteroId",
                table: "RegistrosIngreso",
                column: "PorteroId");

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrosIngreso_Usuarios_PorteroId",
                table: "RegistrosIngreso",
                column: "PorteroId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
