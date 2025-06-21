using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Strack.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Platform = table.Column<int>(type: "INTEGER", nullable: false),
                    ExternalId = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    AvatarUrl = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Activity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    BeginUnixTimeSeconds = table.Column<long>(type: "INTEGER", nullable: true),
                    FinishUnixTimeSeconds = table.Column<long>(type: "INTEGER", nullable: true),
                    CaloriesKilocalories = table.Column<double>(type: "REAL", nullable: true),
                    AltitudeAvgMeters = table.Column<double>(type: "REAL", nullable: true),
                    AltitudeMinMeters = table.Column<double>(type: "REAL", nullable: true),
                    AltitudeMaxMeters = table.Column<double>(type: "REAL", nullable: true),
                    CadenceMaxCpm = table.Column<double>(type: "REAL", nullable: true),
                    CadenceAvgCpm = table.Column<double>(type: "REAL", nullable: true),
                    DistanceTotalMeters = table.Column<double>(type: "REAL", nullable: true),
                    DistanceDownslopeMeters = table.Column<double>(type: "REAL", nullable: true),
                    DistanceUpslopeMeters = table.Column<double>(type: "REAL", nullable: true),
                    DistanceFlatMeters = table.Column<double>(type: "REAL", nullable: true),
                    DurationTotalSeconds = table.Column<double>(type: "REAL", nullable: true),
                    DurationMovingSeconds = table.Column<double>(type: "REAL", nullable: true),
                    DurationPauseSeconds = table.Column<double>(type: "REAL", nullable: true),
                    DurationDownslopeSeconds = table.Column<double>(type: "REAL", nullable: true),
                    DurationUpslopeSeconds = table.Column<double>(type: "REAL", nullable: true),
                    DurationFlatSeconds = table.Column<double>(type: "REAL", nullable: true),
                    ElevationAscentMeters = table.Column<double>(type: "REAL", nullable: true),
                    ElevationDescentMeters = table.Column<double>(type: "REAL", nullable: true),
                    HeartrateAvgBpm = table.Column<double>(type: "REAL", nullable: true),
                    HeartrateMinBpm = table.Column<double>(type: "REAL", nullable: true),
                    HeartrateMaxBpm = table.Column<double>(type: "REAL", nullable: true),
                    PowerMaxWatts = table.Column<double>(type: "REAL", nullable: true),
                    PowerAvgWatts = table.Column<double>(type: "REAL", nullable: true),
                    PowerFtpWatts = table.Column<double>(type: "REAL", nullable: true),
                    PowerNpWatts = table.Column<double>(type: "REAL", nullable: true),
                    PowerIf = table.Column<double>(type: "REAL", nullable: true),
                    PowerVi = table.Column<double>(type: "REAL", nullable: true),
                    PowerTss = table.Column<int>(type: "INTEGER", nullable: true),
                    SlopeAvg = table.Column<double>(type: "REAL", nullable: true),
                    SlopeMin = table.Column<double>(type: "REAL", nullable: true),
                    SlopeMax = table.Column<double>(type: "REAL", nullable: true),
                    SlopeAvgUpslope = table.Column<double>(type: "REAL", nullable: true),
                    SlopeAvgDownslope = table.Column<double>(type: "REAL", nullable: true),
                    SlopeMaxUpslope = table.Column<double>(type: "REAL", nullable: true),
                    SlopeMaxDownslope = table.Column<double>(type: "REAL", nullable: true),
                    SpeedAvgKph = table.Column<double>(type: "REAL", nullable: true),
                    SpeedMaxKph = table.Column<double>(type: "REAL", nullable: true),
                    SpeedAvgAscentMph = table.Column<double>(type: "REAL", nullable: true),
                    SpeedMaxAscentMph = table.Column<double>(type: "REAL", nullable: true),
                    SpeedAvgDescentMph = table.Column<double>(type: "REAL", nullable: true),
                    SpeedMaxDescentMph = table.Column<double>(type: "REAL", nullable: true),
                    TemperatureAvgCelsius = table.Column<double>(type: "REAL", nullable: true),
                    TemperatureMinCelsius = table.Column<double>(type: "REAL", nullable: true),
                    TemperatureMaxCelsius = table.Column<double>(type: "REAL", nullable: true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activity_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserCredential",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCredential", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCredential_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UnixTimeSeconds = table.Column<long>(type: "INTEGER", nullable: true),
                    Latitude = table.Column<double>(type: "REAL", nullable: true),
                    Longitude = table.Column<double>(type: "REAL", nullable: true),
                    AltitudeMeters = table.Column<double>(type: "REAL", nullable: true),
                    SpeedBpm = table.Column<double>(type: "REAL", nullable: true),
                    DistanceMeters = table.Column<double>(type: "REAL", nullable: true),
                    TemperatureCelsius = table.Column<double>(type: "REAL", nullable: true),
                    HeartrateBpm = table.Column<int>(type: "INTEGER", nullable: true),
                    PowerWatts = table.Column<int>(type: "INTEGER", nullable: true),
                    ActivityId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityRecord_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivitySource",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Platform = table.Column<int>(type: "INTEGER", nullable: false),
                    ExternalId = table.Column<long>(type: "INTEGER", nullable: false),
                    ImportUnixTimeSeconds = table.Column<long>(type: "INTEGER", nullable: true),
                    ActivityId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivitySource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivitySource_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activity_UserId",
                table: "Activity",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityRecord_ActivityId",
                table: "ActivityRecord",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivitySource_ActivityId",
                table: "ActivitySource",
                column: "ActivityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Platform_ExternalId",
                table: "User",
                columns: new[] { "Platform", "ExternalId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserCredential_UserId",
                table: "UserCredential",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityRecord");

            migrationBuilder.DropTable(
                name: "ActivitySource");

            migrationBuilder.DropTable(
                name: "UserCredential");

            migrationBuilder.DropTable(
                name: "Activity");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
