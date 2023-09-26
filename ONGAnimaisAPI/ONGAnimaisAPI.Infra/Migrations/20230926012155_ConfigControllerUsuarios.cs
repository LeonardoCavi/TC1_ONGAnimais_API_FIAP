using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ONGAnimaisAPI.Infra.Migrations
{
    /// <inheritdoc />
    public partial class ConfigControllerUsuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "VARCHAR(150)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventoUsuario",
                columns: table => new
                {
                    EventosSeguidosId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventoUsuario", x => new { x.EventosSeguidosId, x.UsuarioId });
                    table.ForeignKey(
                        name: "FK_EventoUsuario_Eventos_EventosSeguidosId",
                        column: x => x.EventosSeguidosId,
                        principalTable: "Eventos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventoUsuario_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ONGUsuario",
                columns: table => new
                {
                    ONGsSeguidasId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ONGUsuario", x => new { x.ONGsSeguidasId, x.UsuarioId });
                    table.ForeignKey(
                        name: "FK_ONGUsuario_ONGs_ONGsSeguidasId",
                        column: x => x.ONGsSeguidasId,
                        principalTable: "ONGs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ONGUsuario_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioEndereco",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_UsuarioEndereco", x => x.UsuarioId);
                    table.ForeignKey(
                        name: "FK_UsuarioEndereco_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioTelefone",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    DDD = table.Column<string>(type: "VARCHAR(2)", nullable: false),
                    Numero = table.Column<string>(type: "VARCHAR(9)", nullable: false),
                    Tipo = table.Column<string>(type: "VARCHAR(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioTelefone", x => x.UsuarioId);
                    table.ForeignKey(
                        name: "FK_UsuarioTelefone_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventoUsuario_UsuarioId",
                table: "EventoUsuario",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ONGUsuario_UsuarioId",
                table: "ONGUsuario",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventoUsuario");

            migrationBuilder.DropTable(
                name: "ONGUsuario");

            migrationBuilder.DropTable(
                name: "UsuarioEndereco");

            migrationBuilder.DropTable(
                name: "UsuarioTelefone");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
