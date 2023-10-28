using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ONGAnimaisAPI.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddGeoLocalizacaoEventoUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UF",
                table: "UsuarioEndereco",
                type: "VARCHAR(2)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(2)");

            migrationBuilder.AlterColumn<string>(
                name: "Logradouro",
                table: "UsuarioEndereco",
                type: "VARCHAR(150)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(150)");

            migrationBuilder.AlterColumn<string>(
                name: "Cidade",
                table: "UsuarioEndereco",
                type: "VARCHAR(150)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(150)");

            migrationBuilder.AlterColumn<string>(
                name: "CEP",
                table: "UsuarioEndereco",
                type: "VARCHAR(8)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(8)");

            migrationBuilder.AlterColumn<string>(
                name: "Bairro",
                table: "UsuarioEndereco",
                type: "VARCHAR(150)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(150)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "OngGeoLocalizacao",
                type: "DECIMAL(10,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(9,7)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "OngGeoLocalizacao",
                type: "DECIMAL(10,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(9,7)");

            migrationBuilder.CreateTable(
                name: "EventoGeoLocalizacao",
                columns: table => new
                {
                    EventoId = table.Column<int>(type: "int", nullable: false),
                    Latitude = table.Column<decimal>(type: "DECIMAL(10,8)", nullable: false),
                    Longitude = table.Column<decimal>(type: "DECIMAL(10,8)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventoGeoLocalizacao", x => x.EventoId);
                    table.ForeignKey(
                        name: "FK_EventoGeoLocalizacao_Eventos_EventoId",
                        column: x => x.EventoId,
                        principalTable: "Eventos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioGeoLocalizacao",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Latitude = table.Column<decimal>(type: "DECIMAL(10,8)", nullable: false),
                    Longitude = table.Column<decimal>(type: "DECIMAL(10,8)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioGeoLocalizacao", x => x.UsuarioId);
                    table.ForeignKey(
                        name: "FK_UsuarioGeoLocalizacao_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventoGeoLocalizacao");

            migrationBuilder.DropTable(
                name: "UsuarioGeoLocalizacao");

            migrationBuilder.AlterColumn<string>(
                name: "UF",
                table: "UsuarioEndereco",
                type: "VARCHAR(2)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Logradouro",
                table: "UsuarioEndereco",
                type: "VARCHAR(150)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(150)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Cidade",
                table: "UsuarioEndereco",
                type: "VARCHAR(150)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(150)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CEP",
                table: "UsuarioEndereco",
                type: "VARCHAR(8)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(8)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Bairro",
                table: "UsuarioEndereco",
                type: "VARCHAR(150)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(150)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "OngGeoLocalizacao",
                type: "DECIMAL(9,7)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(10,8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "OngGeoLocalizacao",
                type: "DECIMAL(9,7)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(10,8)");
        }
    }
}
