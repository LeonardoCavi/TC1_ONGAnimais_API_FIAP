using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ONGAnimaisAPI.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AlteraRequerimentoNumeroEnderecoEvento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_ONGs_ONGId",
                table: "Eventos");

            migrationBuilder.RenameColumn(
                name: "ONGId",
                table: "Eventos",
                newName: "OngId");

            migrationBuilder.RenameIndex(
                name: "IX_Eventos_ONGId",
                table: "Eventos",
                newName: "IX_Eventos_OngId");

            migrationBuilder.AlterColumn<int>(
                name: "OngId",
                table: "Eventos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Numero",
                table: "EventoEndereco",
                type: "VARCHAR(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)");

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_ONGs_OngId",
                table: "Eventos",
                column: "OngId",
                principalTable: "ONGs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_ONGs_OngId",
                table: "Eventos");

            migrationBuilder.RenameColumn(
                name: "OngId",
                table: "Eventos",
                newName: "ONGId");

            migrationBuilder.RenameIndex(
                name: "IX_Eventos_OngId",
                table: "Eventos",
                newName: "IX_Eventos_ONGId");

            migrationBuilder.AlterColumn<int>(
                name: "ONGId",
                table: "Eventos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Numero",
                table: "EventoEndereco",
                type: "VARCHAR(20)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(20)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_ONGs_ONGId",
                table: "Eventos",
                column: "ONGId",
                principalTable: "ONGs",
                principalColumn: "Id");
        }
    }
}
