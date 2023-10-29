using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DominosCutScreen.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedPostBakes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SettingsServiceId",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.CreateTable(
                name: "PostBakes",
                columns: table => new
                {
                    ReceiptCode = table.Column<string>(type: "TEXT", nullable: false),
                    ToppingCode = table.Column<string>(type: "TEXT", nullable: false),
                    ToppingDescription = table.Column<string>(type: "TEXT", nullable: false),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    SettingsServiceId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostBakes", x => x.ReceiptCode);
                    table.ForeignKey(
                        name: "FK_PostBakes_Settings_SettingsServiceId",
                        column: x => x.SettingsServiceId,
                        principalTable: "Settings",
                        principalColumn: "SettingsServiceId");
                });

            migrationBuilder.InsertData(
                table: "PostBakes",
                columns: new[] { "ReceiptCode", "IsEnabled", "SettingsServiceId", "ToppingCode", "ToppingDescription" },
                values: new object[,]
                {
                    { "*", true, 1, "SPO", "Spring Onion" },
                    { "B", true, 1, "BtrChk", "Butter Chicken Sce" },
                    { "CS", true, 1, "CeSpr", "Cheese Sprinkle" },
                    { "F", true, 1, "FRANKS", "Franks Hot Sce" },
                    { "GB", true, 1, "GBUTT", "Garlic Butter" },
                    { "HB", true, 1, "HICBBQ", "Hickory BBQ" },
                    { "HO", true, 1, "HOLLAND", "Hollandaise" },
                    { "M", true, 1, "My", "Mayonnaise" },
                    { "P", true, 1, "PERI", "Peri Peri Sce" },
                    { "PA", true, 1, "PARMSC", "Garlc Parm Sce" },
                    { "T", true, 1, "TOMCAP", "Tom Caps Sce" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostBakes_SettingsServiceId",
                table: "PostBakes",
                column: "SettingsServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostBakes");

            migrationBuilder.AlterColumn<int>(
                name: "SettingsServiceId",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);
        }
    }
}
