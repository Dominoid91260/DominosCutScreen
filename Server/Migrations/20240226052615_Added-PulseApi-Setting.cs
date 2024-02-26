using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DominosCutScreen.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedPulseApiSetting : Migration
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

            migrationBuilder.AddColumn<string>(
                name: "PulseApiServer",
                table: "Settings",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "SettingsServiceId",
                keyValue: 1,
                column: "PulseApiServer",
                value: "http://pulseapi");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PulseApiServer",
                table: "Settings");

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
