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
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Sport = table.Column<int>(type: "INTEGER", nullable: false),
                    Source = table.Column<int>(type: "INTEGER", nullable: false),
                    BeginUnixTimeSeconds = table.Column<long>(type: "INTEGER", nullable: true),
                    FinishUnixTimeSeconds = table.Column<long>(type: "INTEGER", nullable: true),
                    DurationSeconds = table.Column<double>(type: "REAL", nullable: true),
                    TotalDistanceMeters = table.Column<double>(type: "REAL", nullable: true),
                    CaloriesKilocalories = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActivityCadence",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    MaxCpm = table.Column<double>(type: "REAL", nullable: true),
                    AvgCpm = table.Column<double>(type: "REAL", nullable: true),
                    ActivityId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityCadence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityCadence_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityElevation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AvgAltitudeMeters = table.Column<double>(type: "REAL", nullable: true),
                    MinAltitudeMeters = table.Column<double>(type: "REAL", nullable: true),
                    MaxAltitudeMeters = table.Column<double>(type: "REAL", nullable: true),
                    AvgGrade = table.Column<double>(type: "REAL", nullable: true),
                    MinGrade = table.Column<double>(type: "REAL", nullable: true),
                    MaxGrade = table.Column<double>(type: "REAL", nullable: true),
                    AvgUpslopeGrade = table.Column<double>(type: "REAL", nullable: true),
                    AvgDownslopeGrade = table.Column<double>(type: "REAL", nullable: true),
                    MaxUpslopeGrade = table.Column<double>(type: "REAL", nullable: true),
                    MaxDownslopeGrade = table.Column<double>(type: "REAL", nullable: true),
                    DownslopeDistanceMeters = table.Column<double>(type: "REAL", nullable: true),
                    UpslopeDistanceMeters = table.Column<double>(type: "REAL", nullable: true),
                    FlatDistanceMeters = table.Column<double>(type: "REAL", nullable: true),
                    DescentHeightMeters = table.Column<double>(type: "REAL", nullable: true),
                    AscentHeightMeters = table.Column<double>(type: "REAL", nullable: true),
                    AvgAscentSpeed = table.Column<double>(type: "REAL", nullable: true),
                    MaxAscentSpeed = table.Column<double>(type: "REAL", nullable: true),
                    AvgDescentSpeed = table.Column<double>(type: "REAL", nullable: true),
                    MaxDescentSpeed = table.Column<double>(type: "REAL", nullable: true),
                    DownslopeDurationSeconds = table.Column<double>(type: "REAL", nullable: true),
                    UpslopeDurationSeconds = table.Column<double>(type: "REAL", nullable: true),
                    FlatDurationSeconds = table.Column<double>(type: "REAL", nullable: true),
                    ActivityId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityElevation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityElevation_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityHeartrate",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AvgBpm = table.Column<double>(type: "REAL", nullable: true),
                    MinBpm = table.Column<double>(type: "REAL", nullable: true),
                    MaxBpm = table.Column<double>(type: "REAL", nullable: true),
                    ActivityId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityHeartrate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityHeartrate_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityPower",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    MaxWatts = table.Column<double>(type: "REAL", nullable: true),
                    AvgWatts = table.Column<double>(type: "REAL", nullable: true),
                    FtpWatts = table.Column<double>(type: "REAL", nullable: true),
                    NpWatts = table.Column<double>(type: "REAL", nullable: true),
                    If = table.Column<double>(type: "REAL", nullable: true),
                    Vi = table.Column<double>(type: "REAL", nullable: true),
                    Tss = table.Column<int>(type: "INTEGER", nullable: true),
                    ActivityId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityPower", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityPower_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivitySpeed",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AvgKph = table.Column<double>(type: "REAL", nullable: true),
                    MaxKph = table.Column<double>(type: "REAL", nullable: true),
                    ActivityId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivitySpeed", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivitySpeed_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityTemperature",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AvgCelsius = table.Column<double>(type: "REAL", nullable: true),
                    MinCelsius = table.Column<double>(type: "REAL", nullable: true),
                    MaxCelsius = table.Column<double>(type: "REAL", nullable: true),
                    ActivityId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityTemperature", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityTemperature_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityTime",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    MovingDurationSeconds = table.Column<double>(type: "REAL", nullable: true),
                    PauseDurationSeconds = table.Column<double>(type: "REAL", nullable: true),
                    ActivityId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityTime", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityTime_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Record",
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
                    table.PrimaryKey("PK_Record", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Record_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityCadence_ActivityId",
                table: "ActivityCadence",
                column: "ActivityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityElevation_ActivityId",
                table: "ActivityElevation",
                column: "ActivityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityHeartrate_ActivityId",
                table: "ActivityHeartrate",
                column: "ActivityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityPower_ActivityId",
                table: "ActivityPower",
                column: "ActivityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivitySpeed_ActivityId",
                table: "ActivitySpeed",
                column: "ActivityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityTemperature_ActivityId",
                table: "ActivityTemperature",
                column: "ActivityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivityTime_ActivityId",
                table: "ActivityTime",
                column: "ActivityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Record_ActivityId",
                table: "Record",
                column: "ActivityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityCadence");

            migrationBuilder.DropTable(
                name: "ActivityElevation");

            migrationBuilder.DropTable(
                name: "ActivityHeartrate");

            migrationBuilder.DropTable(
                name: "ActivityPower");

            migrationBuilder.DropTable(
                name: "ActivitySpeed");

            migrationBuilder.DropTable(
                name: "ActivityTemperature");

            migrationBuilder.DropTable(
                name: "ActivityTime");

            migrationBuilder.DropTable(
                name: "Record");

            migrationBuilder.DropTable(
                name: "Activity");
        }
    }
}
