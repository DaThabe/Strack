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
                name: "Activity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Sport = table.Column<int>(type: "INTEGER", nullable: false),
                    CaloriesKilocalories = table.Column<double>(type: "REAL", nullable: false),
                    BeginTimeUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FinishTimeUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TotalTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    MovingTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    PauseTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    MinTemperatureCelsius = table.Column<double>(type: "REAL", nullable: false),
                    MaxTemperatureCelsius = table.Column<double>(type: "REAL", nullable: false),
                    AvgTemperatureCelsius = table.Column<double>(type: "REAL", nullable: false),
                    AvgAltitudeMeters = table.Column<double>(type: "REAL", nullable: false),
                    MinAltitudeMeters = table.Column<double>(type: "REAL", nullable: false),
                    MaxAltitudeMeters = table.Column<double>(type: "REAL", nullable: false),
                    TotalAscentMeters = table.Column<double>(type: "REAL", nullable: false),
                    TotalDescentMeters = table.Column<double>(type: "REAL", nullable: false),
                    TotalDistanceMeters = table.Column<double>(type: "REAL", nullable: false),
                    AvgSpeedKilometersPerHour = table.Column<double>(type: "REAL", nullable: false),
                    MaxSpeedKilometersPerHour = table.Column<double>(type: "REAL", nullable: false),
                    AvgHeartrateBeatsPerMinute = table.Column<double>(type: "REAL", nullable: false),
                    MaxBeatsPerMinute = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActivityCyclingData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AvgCadenceCyclesPerMinute = table.Column<double>(type: "REAL", nullable: false),
                    MaxCadenceCyclesPerMinute = table.Column<double>(type: "REAL", nullable: false),
                    AvgLeftPowerWatts = table.Column<double>(type: "REAL", nullable: true),
                    AvgRightPowerWatts = table.Column<double>(type: "REAL", nullable: true),
                    MaxLeftPowerWatts = table.Column<double>(type: "REAL", nullable: true),
                    MaxRightPowerWatts = table.Column<double>(type: "REAL", nullable: true),
                    AvgLeftBalancePercent = table.Column<double>(type: "REAL", nullable: true),
                    AvgRightBalancePercent = table.Column<double>(type: "REAL", nullable: true),
                    TotalPowerWatts = table.Column<double>(type: "REAL", nullable: false),
                    MaxPowerWatts = table.Column<double>(type: "REAL", nullable: false),
                    AvgPowerWatts = table.Column<double>(type: "REAL", nullable: false),
                    NormalizedPowerWatts = table.Column<double>(type: "REAL", nullable: false),
                    VariabilityIndex = table.Column<double>(type: "REAL", nullable: false),
                    IntensityFactor = table.Column<double>(type: "REAL", nullable: false),
                    TrainingStressScore = table.Column<double>(type: "REAL", nullable: false),
                    FunctionalThresholdPowerWatts = table.Column<double>(type: "REAL", nullable: false),
                    ActivityId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityCyclingData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityCyclingData_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sampling",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TimestampUTC = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LatitudeDegrees = table.Column<double>(type: "REAL", nullable: false),
                    LongitudeDegrees = table.Column<double>(type: "REAL", nullable: false),
                    AltitudeMeters = table.Column<double>(type: "REAL", nullable: true),
                    SpeedMetersPerSecond = table.Column<double>(type: "REAL", nullable: true),
                    DistanceMeters = table.Column<double>(type: "REAL", nullable: true),
                    TemperatureCelsius = table.Column<double>(type: "REAL", nullable: true),
                    HeartrateBeatPerMinute = table.Column<int>(type: "INTEGER", nullable: true),
                    ActivityId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sampling", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sampling_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Source",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    ActivityId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Source", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Source_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SamplingCyclingData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CadenCyclesPerMinute = table.Column<double>(type: "REAL", nullable: true),
                    PowerWatts = table.Column<double>(type: "REAL", nullable: true),
                    CadenceRpm = table.Column<double>(type: "REAL", nullable: true),
                    LeftPowerPercent = table.Column<double>(type: "REAL", nullable: true),
                    RightPowerPercent = table.Column<double>(type: "REAL", nullable: true),
                    TorqueEfficiencyPercent = table.Column<double>(type: "REAL", nullable: true),
                    PedalSmoothnessPercent = table.Column<double>(type: "REAL", nullable: true),
                    LeftTorqueNm = table.Column<double>(type: "REAL", nullable: true),
                    RightTorqueNm = table.Column<double>(type: "REAL", nullable: true),
                    SamplingId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SamplingCyclingData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SamplingCyclingData_Sampling_SamplingId",
                        column: x => x.SamplingId,
                        principalTable: "Sampling",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SourceData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SourceId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SourceData_Source_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Source",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SourceXingZhe",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<long>(type: "INTEGER", nullable: false),
                    ActivityId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceXingZhe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SourceXingZhe_SourceData_Id",
                        column: x => x.Id,
                        principalTable: "SourceData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityCyclingData_ActivityId",
                table: "ActivityCyclingData",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Sampling_ActivityId",
                table: "Sampling",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_SamplingCyclingData_SamplingId",
                table: "SamplingCyclingData",
                column: "SamplingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Source_ActivityId",
                table: "Source",
                column: "ActivityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SourceData_SourceId",
                table: "SourceData",
                column: "SourceId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityCyclingData");

            migrationBuilder.DropTable(
                name: "SamplingCyclingData");

            migrationBuilder.DropTable(
                name: "SourceXingZhe");

            migrationBuilder.DropTable(
                name: "Sampling");

            migrationBuilder.DropTable(
                name: "SourceData");

            migrationBuilder.DropTable(
                name: "Source");

            migrationBuilder.DropTable(
                name: "Activity");
        }
    }
}
