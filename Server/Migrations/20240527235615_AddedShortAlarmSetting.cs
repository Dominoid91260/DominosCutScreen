using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DominosCutScreen.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedShortAlarmSetting : Migration
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

            migrationBuilder.AddColumn<bool>(
                name: "UseShortAlarm",
                table: "Settings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "SettingsServiceId",
                keyValue: 1,
                column: "UseShortAlarm",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UseShortAlarm",
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
