using Microsoft.EntityFrameworkCore.Migrations;

namespace ControllerProgrammer.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Leds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Wavelength = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PowerDensities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LedId = table.Column<int>(type: "INTEGER", nullable: false),
                    Current = table.Column<double>(type: "REAL", nullable: false),
                    PowerDenisty = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerDensities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PowerDensities_Leds_LedId",
                        column: x => x.LedId,
                        principalTable: "Leds",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PowerDensities_LedId",
                table: "PowerDensities",
                column: "LedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PowerDensities");

            migrationBuilder.DropTable(
                name: "Leds");
        }
    }
}
