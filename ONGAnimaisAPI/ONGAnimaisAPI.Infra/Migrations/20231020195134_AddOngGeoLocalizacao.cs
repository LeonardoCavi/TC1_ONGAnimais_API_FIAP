using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ONGAnimaisAPI.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddOngGeoLocalizacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "OngGeoLocalizacao",
                columns: table => new
                {
                    ONGId = table.Column<int>(type: "int", nullable: false),
                    Latitude = table.Column<decimal>(type: "DECIMAL(9,7)", nullable: false),
                    Longitude = table.Column<decimal>(type: "DECIMAL(9,7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OngGeoLocalizacao", x => x.ONGId);
                    table.ForeignKey(
                        name: "FK_OngGeoLocalizacao_ONGs_ONGId",
                        column: x => x.ONGId,
                        principalTable: "ONGs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OngGeoLocalizacao");

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
    }
}
