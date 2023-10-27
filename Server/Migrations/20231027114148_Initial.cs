using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DominosCutScreen.Server.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    SettingsServiceId = table.Column<int>(type: "INTEGER", nullable: false),
                    MakelineServer = table.Column<string>(type: "TEXT", nullable: false),
                    MakelineCode = table.Column<int>(type: "INTEGER", nullable: false),
                    OvenTime = table.Column<int>(type: "INTEGER", nullable: false),
                    GraceTime = table.Column<int>(type: "INTEGER", nullable: false),
                    AlertInterval = table.Column<int>(type: "INTEGER", nullable: false),
                    FetchInterval = table.Column<int>(type: "INTEGER", nullable: false),
                    QuietTime_IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    QuietTime_Start = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    QuietTime_End = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    TimedOrderAlarm_IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    TimedOrderAlarm_SecondsPerPizza = table.Column<int>(type: "INTEGER", nullable: false),
                    TimedOrderAlarm_MinPizzaThreshold = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.SettingsServiceId);
                });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "SettingsServiceId", "QuietTime_End", "QuietTime_IsEnabled", "QuietTime_Start", "AlertInterval", "FetchInterval", "GraceTime", "MakelineCode", "MakelineServer", "OvenTime", "TimedOrderAlarm_IsEnabled", "TimedOrderAlarm_MinPizzaThreshold", "TimedOrderAlarm_SecondsPerPizza" },
                values: new object[] { 1, new TimeOnly(0, 0, 0), false, new TimeOnly(0, 0, 0), 150, 5, 90, 2, "http://localhost:59108", 300, false, 7, 15 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Settings");
        }
    }
}
