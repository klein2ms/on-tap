using Microsoft.EntityFrameworkCore.Migrations;

namespace OnTap.Blazor.Server.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Leagues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leagues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SeasonId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Position = table.Column<int>(nullable: false),
                    TeamShortName = table.Column<string>(nullable: true),
                    ByeWeek = table.Column<int>(nullable: false),
                    Best = table.Column<int>(nullable: false),
                    Worst = table.Column<int>(nullable: false),
                    Adp = table.Column<decimal>(nullable: false),
                    PassingAttempts = table.Column<decimal>(nullable: true),
                    PassingCompletions = table.Column<decimal>(nullable: true),
                    PassingYards = table.Column<decimal>(nullable: true),
                    PassingTouchdowns = table.Column<decimal>(nullable: true),
                    PassingInterceptions = table.Column<decimal>(nullable: true),
                    RushingAttempts = table.Column<decimal>(nullable: true),
                    RushingYards = table.Column<decimal>(nullable: true),
                    RushingTouchdowns = table.Column<decimal>(nullable: true),
                    Receptions = table.Column<decimal>(nullable: true),
                    ReceivingYards = table.Column<decimal>(nullable: true),
                    ReceivingTouchdowns = table.Column<decimal>(nullable: true),
                    FumblesLost = table.Column<decimal>(nullable: true),
                    FieldGoals = table.Column<decimal>(nullable: true),
                    FieldGoalsAttempted = table.Column<decimal>(nullable: true),
                    FieldGoalsMissed = table.Column<decimal>(nullable: true),
                    ExtraPoints = table.Column<decimal>(nullable: true),
                    Sacks = table.Column<decimal>(nullable: true),
                    Interceptions = table.Column<decimal>(nullable: true),
                    FumblesRecovered = table.Column<decimal>(nullable: true),
                    ForcedFumbles = table.Column<decimal>(nullable: true),
                    DefensiveTouchdowns = table.Column<decimal>(nullable: true),
                    Safeties = table.Column<decimal>(nullable: true),
                    PointsAgainst = table.Column<decimal>(nullable: true),
                    YardsAgainst = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Drafts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LeagueId = table.Column<int>(nullable: false),
                    IsComplete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drafts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Drafts_Leagues_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "Leagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeagueSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SeasonId = table.Column<int>(nullable: false),
                    LeagueId = table.Column<int>(nullable: false),
                    NumberOfStartingQuarterbacks = table.Column<int>(nullable: false),
                    NumberOfStartingRunningBacks = table.Column<int>(nullable: false),
                    NumberOfStartingRunningBackOrWideReceivers = table.Column<int>(nullable: false),
                    NumberOfStartingWideReceivers = table.Column<int>(nullable: false),
                    NumberOfStartingWideReceiverOrTightEnds = table.Column<int>(nullable: false),
                    NumberOfStartingTightEnds = table.Column<int>(nullable: false),
                    NumberOfStartingDefenses = table.Column<int>(nullable: false),
                    NumberOfStartingKickers = table.Column<int>(nullable: false),
                    NumberOfBenchPlayers = table.Column<int>(nullable: false),
                    PointsPerPassingYard = table.Column<decimal>(nullable: false),
                    PointsPerPassingCompletion = table.Column<decimal>(nullable: false),
                    PointsPerPassingTouchdown = table.Column<decimal>(nullable: false),
                    PointsPerPassingInterception = table.Column<decimal>(nullable: false),
                    PointsPerPassingTwoPointConversion = table.Column<decimal>(nullable: false),
                    PointsPerRushingYard = table.Column<decimal>(nullable: false),
                    PointsPerRushingTouchdown = table.Column<decimal>(nullable: false),
                    PointsPerRushingTwoPointConversion = table.Column<decimal>(nullable: false),
                    PointsPerReceivingYard = table.Column<decimal>(nullable: false),
                    PointsPerReception = table.Column<decimal>(nullable: false),
                    PointsPerReceivingTouchdown = table.Column<decimal>(nullable: false),
                    PointsPerReceivingTwoPointConversion = table.Column<decimal>(nullable: false),
                    PointsPerPatMade = table.Column<decimal>(nullable: false),
                    PointsPerPatMissed = table.Column<decimal>(nullable: false),
                    PointsPerFieldGoalMadeLessThan40Yards = table.Column<decimal>(nullable: false),
                    PointsPerFieldGoalMade40To49Yards = table.Column<decimal>(nullable: false),
                    PointsPerFieldGoalMadeGreaterThan50Yards = table.Column<decimal>(nullable: false),
                    PointsPerFieldGoalMissedLessThan40Yards = table.Column<decimal>(nullable: false),
                    PointsPer25KickoffReturnYards = table.Column<decimal>(nullable: false),
                    PointsPer10PuntReturnYards = table.Column<decimal>(nullable: false),
                    PointsPerKickoffReturnTouchdown = table.Column<decimal>(nullable: false),
                    PointsPerPuntReturnTouchdown = table.Column<decimal>(nullable: false),
                    PointsPerInterceptionReturnTouchdown = table.Column<decimal>(nullable: false),
                    PointsPerFumbleReturnTouchdown = table.Column<decimal>(nullable: false),
                    PointsPerBlockedKickForTouchdown = table.Column<decimal>(nullable: false),
                    PointsPerSack = table.Column<decimal>(nullable: false),
                    PointsPerBlockedKick = table.Column<decimal>(nullable: false),
                    PointsPerDefensiveInterception = table.Column<decimal>(nullable: false),
                    PointsPerFumbleRecovery = table.Column<decimal>(nullable: false),
                    PointsPerSafety = table.Column<decimal>(nullable: false),
                    PointsPer0PointsAgainst = table.Column<decimal>(nullable: false),
                    PointsPer1To6PointsAgainst = table.Column<decimal>(nullable: false),
                    PointsPer7To13PointsAgainst = table.Column<decimal>(nullable: false),
                    PointsPer14To17PointsAgainst = table.Column<decimal>(nullable: false),
                    PointsPer18To21PointsAgainst = table.Column<decimal>(nullable: false),
                    PointsPer28To34PointsAgainst = table.Column<decimal>(nullable: false),
                    PointsPer35To45PointsAgainst = table.Column<decimal>(nullable: false),
                    PointsPer46OrMorePointsAgainst = table.Column<decimal>(nullable: false),
                    PointsPerTotalFumblesLost = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeagueSettings", x => x.Id);
                    table.UniqueConstraint("AK_LeagueSettings_LeagueId", x => x.LeagueId);
                    table.ForeignKey(
                        name: "FK_LeagueSettings_Leagues_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "Leagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LeagueId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    OwnerName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_Leagues_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "Leagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DraftPicks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DraftId = table.Column<int>(nullable: false),
                    TeamId = table.Column<int>(nullable: false),
                    PlayerId = table.Column<int>(nullable: false),
                    Number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DraftPicks", x => x.Id);
                    table.UniqueConstraint("AK_DraftPicks_DraftId_TeamId_PlayerId", x => new { x.DraftId, x.TeamId, x.PlayerId });
                    table.ForeignKey(
                        name: "FK_DraftPicks_Drafts_DraftId",
                        column: x => x.DraftId,
                        principalTable: "Drafts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DraftPicks_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DraftPicks_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DraftPosition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DraftId = table.Column<int>(nullable: false),
                    TeamId = table.Column<int>(nullable: false),
                    Number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DraftPosition", x => x.Id);
                    table.UniqueConstraint("AK_DraftPosition_DraftId_TeamId", x => new { x.DraftId, x.TeamId });
                    table.ForeignKey(
                        name: "FK_DraftPosition_Drafts_DraftId",
                        column: x => x.DraftId,
                        principalTable: "Drafts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DraftPosition_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DraftPicks_PlayerId",
                table: "DraftPicks",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_DraftPicks_TeamId",
                table: "DraftPicks",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_DraftPosition_TeamId",
                table: "DraftPosition",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Drafts_LeagueId",
                table: "Drafts",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_LeagueId",
                table: "Teams",
                column: "LeagueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DraftPicks");

            migrationBuilder.DropTable(
                name: "DraftPosition");

            migrationBuilder.DropTable(
                name: "LeagueSettings");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Drafts");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Leagues");
        }
    }
}
