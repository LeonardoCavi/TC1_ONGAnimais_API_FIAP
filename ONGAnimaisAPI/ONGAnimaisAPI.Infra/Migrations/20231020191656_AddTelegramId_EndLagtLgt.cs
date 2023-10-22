using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ONGAnimaisAPI.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddTelegramId_EndLagtLgt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TelegramId",
                table: "Usuarios",
                type: "VARCHAR(16)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                table: "UsuarioEndereco",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "UsuarioEndereco",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                table: "OngEndereco",
                type: "DECIMAL(9,7)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "OngEndereco",
                type: "DECIMAL(9,7)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                table: "EventoEndereco",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "EventoEndereco",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TelegramId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "UsuarioEndereco");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "UsuarioEndereco");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "OngEndereco");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "OngEndereco");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "EventoEndereco");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "EventoEndereco");
        }
    }
}
