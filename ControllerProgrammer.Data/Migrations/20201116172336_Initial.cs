using Microsoft.EntityFrameworkCore.Migrations;

namespace ControllerProgrammer.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "controlDb");

            migrationBuilder.CreateTable(
                name: "Leds",
                schema: "controlDb",
                columns: table => new
                {
                    LedId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Wavelength = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leds", x => x.LedId);
                });

            migrationBuilder.CreateTable(
                name: "PowerDensities",
                schema: "controlDb",
                columns: table => new
                {
                    PowerDensityId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LedId = table.Column<int>(type: "INTEGER", nullable: false),
                    Current = table.Column<double>(type: "REAL", nullable: false),
                    PowerDenisty = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerDensities", x => x.PowerDensityId);
                    table.ForeignKey(
                        name: "FK_PowerDensities_Leds_LedId",
                        column: x => x.LedId,
                        principalSchema: "controlDb",
                        principalTable: "Leds",
                        principalColumn: "LedId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PowerDensities_LedId",
                schema: "controlDb",
                table: "PowerDensities",
                column: "LedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PowerDensities",
                schema: "controlDb");

            migrationBuilder.DropTable(
                name: "Leds",
                schema: "controlDb");
        }
    }
}
