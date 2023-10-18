using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ONGAnimaisAPI.Infra.Migrations
{
    /// <inheritdoc />
    public partial class ConfigTabelasRelacionaisONG : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contato");

            migrationBuilder.DropTable(
                name: "Telefone");

            migrationBuilder.CreateTable(
                name: "OngContato",
                columns: table => new
                {
                    ONGId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    URL = table.Column<string>(type: "VARCHAR(500)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OngContato", x => new { x.ONGId, x.Id });
                    table.ForeignKey(
                        name: "FK_OngContato_ONGs_ONGId",
                        column: x => x.ONGId,
                        principalTable: "ONGs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OngTelefone",
                columns: table => new
                {
                    ONGId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DDD = table.Column<string>(type: "VARCHAR(2)", nullable: false),
                    Numero = table.Column<string>(type: "VARCHAR(9)", nullable: false),
                    Tipo = table.Column<string>(type: "VARCHAR(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OngTelefone", x => new { x.ONGId, x.Id });
                    table.ForeignKey(
                        name: "FK_OngTelefone_ONGs_ONGId",
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
                name: "OngContato");

            migrationBuilder.DropTable(
                name: "OngTelefone");

            migrationBuilder.CreateTable(
                name: "Contato",
                columns: table => new
                {
                    ONGId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    URL = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contato", x => new { x.ONGId, x.Id });
                    table.ForeignKey(
                        name: "FK_Contato_ONGs_ONGId",
                        column: x => x.ONGId,
                        principalTable: "ONGs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Telefone",
                columns: table => new
                {
                    ONGId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DDD = table.Column<int>(type: "int", nullable: false),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Telefone", x => new { x.ONGId, x.Id });
                    table.ForeignKey(
                        name: "FK_Telefone_ONGs_ONGId",
                        column: x => x.ONGId,
                        principalTable: "ONGs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
