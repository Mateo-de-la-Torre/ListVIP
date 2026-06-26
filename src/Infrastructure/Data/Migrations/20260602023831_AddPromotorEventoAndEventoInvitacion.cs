using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPromotorEventoAndEventoInvitacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Already applied by previous partial migration run
            // migrationBuilder.DropColumn(name: "Active", table: "PromotorEventos");
            // migrationBuilder.AlterColumn<decimal>(name: "Commission", ...);
            // migrationBuilder.AddColumn<int>(name: "EventoId", table: "Invitaciones", ...);
            // migrationBuilder.CreateIndex(name: "IX_Invitaciones_EventoId", ...);

            migrationBuilder.AddForeignKey(
                name: "FK_Invitaciones_Eventos_EventoId",
                table: "Invitaciones",
                column: "EventoId",
                principalTable: "Eventos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitaciones_Eventos_EventoId",
                table: "Invitaciones");

            migrationBuilder.DropIndex(
                name: "IX_Invitaciones_EventoId",
                table: "Invitaciones");

            migrationBuilder.DropColumn(
                name: "EventoId",
                table: "Invitaciones");

            migrationBuilder.AlterColumn<decimal>(
                name: "Commission",
                table: "PromotorEventos",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "PromotorEventos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
