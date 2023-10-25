using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ONGAnimaisAPI.Infra.Migrations
{
    /// <inheritdoc />
    public partial class TelefoneNullableUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Numero",
                table: "UsuarioTelefone",
                type: "VARCHAR(9)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(9)");

            migrationBuilder.AlterColumn<string>(
                name: "DDD",
                table: "UsuarioTelefone",
                type: "VARCHAR(2)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Numero",
                table: "UsuarioTelefone",
                type: "VARCHAR(9)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(9)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DDD",
                table: "UsuarioTelefone",
                type: "VARCHAR(2)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(2)",
                oldNullable: true);
        }
    }
}
