using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ONGAnimaisAPI.Infra.Migrations
{
    /// <inheritdoc />
    public partial class ConfigControllerONGEventos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Eventos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    Descricao = table.Column<string>(type: "VARCHAR(150)", nullable: false),
                    Data = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    ONGId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eventos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Eventos_ONGs_ONGId",
                        column: x => x.ONGId,
                        principalTable: "ONGs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EventoEndereco",
                columns: table => new
                {
                    EventoId = table.Column<int>(type: "int", nullable: false),
                    CEP = table.Column<string>(type: "VARCHAR(8)", nullable: false),
                    Logradouro = table.Column<string>(type: "VARCHAR(150)", nullable: false),
                    Numero = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    Complemento = table.Column<string>(type: "VARCHAR(150)", nullable: true),
                    Bairro = table.Column<string>(type: "VARCHAR(150)", nullable: false),
                    Cidade = table.Column<string>(type: "VARCHAR(150)", nullable: false),
                    UF = table.Column<string>(type: "VARCHAR(2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventoEndereco", x => x.EventoId);
                    table.ForeignKey(
                        name: "FK_EventoEndereco_Eventos_EventoId",
                        column: x => x.EventoId,
                        principalTable: "Eventos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_ONGId",
                table: "Eventos",
                column: "ONGId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventoEndereco");

            migrationBuilder.DropTable(
                name: "Eventos");
        }
    }
}
