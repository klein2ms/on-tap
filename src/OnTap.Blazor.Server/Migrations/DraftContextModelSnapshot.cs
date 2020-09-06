﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OnTap.Blazor.Server.Data;

namespace OnTap.Blazor.Server.Migrations
{
    [DbContext(typeof(DraftContext))]
    partial class DraftContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("OnTap.Blazor.Server.Models.Draft", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsComplete");

                    b.Property<int>("LeagueId");

                    b.HasKey("Id");

                    b.HasIndex("LeagueId");

                    b.ToTable("Drafts");
                });

            modelBuilder.Entity("OnTap.Blazor.Server.Models.DraftPick", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DraftId");

                    b.Property<int>("Number");

                    b.Property<int>("PlayerId");

                    b.Property<int>("TeamId");

                    b.HasKey("Id");

                    b.HasAlternateKey("DraftId", "TeamId", "PlayerId");

                    b.HasIndex("PlayerId");

                    b.HasIndex("TeamId");

                    b.ToTable("DraftPicks");
                });

            modelBuilder.Entity("OnTap.Blazor.Server.Models.DraftPosition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DraftId");

                    b.Property<int>("Number");

                    b.Property<int>("TeamId");

                    b.HasKey("Id");

                    b.HasAlternateKey("DraftId", "TeamId");

                    b.HasIndex("TeamId");

                    b.ToTable("DraftPosition");
                });

            modelBuilder.Entity("OnTap.Blazor.Server.Models.League", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Leagues");
                });

            modelBuilder.Entity("OnTap.Blazor.Server.Models.LeagueSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("LeagueId");

                    b.Property<int>("NumberOfBenchPlayers");

                    b.Property<int>("NumberOfStartingDefenses");

                    b.Property<int>("NumberOfStartingKickers");

                    b.Property<int>("NumberOfStartingQuarterbacks");

                    b.Property<int>("NumberOfStartingRunningBackOrWideReceivers");

                    b.Property<int>("NumberOfStartingRunningBacks");

                    b.Property<int>("NumberOfStartingTightEnds");

                    b.Property<int>("NumberOfStartingWideReceiverOrTightEnds");

                    b.Property<int>("NumberOfStartingWideReceivers");

                    b.Property<decimal>("PointsPer0PointsAgainst");

                    b.Property<decimal>("PointsPer10PuntReturnYards");

                    b.Property<decimal>("PointsPer14To17PointsAgainst");

                    b.Property<decimal>("PointsPer18To21PointsAgainst");

                    b.Property<decimal>("PointsPer1To6PointsAgainst");

                    b.Property<decimal>("PointsPer25KickoffReturnYards");

                    b.Property<decimal>("PointsPer28To34PointsAgainst");

                    b.Property<decimal>("PointsPer35To45PointsAgainst");

                    b.Property<decimal>("PointsPer46OrMorePointsAgainst");

                    b.Property<decimal>("PointsPer7To13PointsAgainst");

                    b.Property<decimal>("PointsPerBlockedKick");

                    b.Property<decimal>("PointsPerBlockedKickForTouchdown");

                    b.Property<decimal>("PointsPerDefensiveInterception");

                    b.Property<decimal>("PointsPerFieldGoalMade40To49Yards");

                    b.Property<decimal>("PointsPerFieldGoalMadeGreaterThan50Yards");

                    b.Property<decimal>("PointsPerFieldGoalMadeLessThan40Yards");

                    b.Property<decimal>("PointsPerFieldGoalMissedLessThan40Yards");

                    b.Property<decimal>("PointsPerFumbleRecovery");

                    b.Property<decimal>("PointsPerFumbleReturnTouchdown");

                    b.Property<decimal>("PointsPerInterceptionReturnTouchdown");

                    b.Property<decimal>("PointsPerKickoffReturnTouchdown");

                    b.Property<decimal>("PointsPerPassingCompletion");

                    b.Property<decimal>("PointsPerPassingInterception");

                    b.Property<decimal>("PointsPerPassingTouchdown");

                    b.Property<decimal>("PointsPerPassingTwoPointConversion");

                    b.Property<decimal>("PointsPerPassingYard");

                    b.Property<decimal>("PointsPerPatMade");

                    b.Property<decimal>("PointsPerPatMissed");

                    b.Property<decimal>("PointsPerPuntReturnTouchdown");

                    b.Property<decimal>("PointsPerReceivingTouchdown");

                    b.Property<decimal>("PointsPerReceivingTwoPointConversion");

                    b.Property<decimal>("PointsPerReceivingYard");

                    b.Property<decimal>("PointsPerReception");

                    b.Property<decimal>("PointsPerRushingTouchdown");

                    b.Property<decimal>("PointsPerRushingTwoPointConversion");

                    b.Property<decimal>("PointsPerRushingYard");

                    b.Property<decimal>("PointsPerSack");

                    b.Property<decimal>("PointsPerSafety");

                    b.Property<decimal>("PointsPerTotalFumblesLost");

                    b.Property<int>("SeasonId");

                    b.HasKey("Id");

                    b.HasAlternateKey("LeagueId");

                    b.ToTable("LeagueSettings");
                });

            modelBuilder.Entity("OnTap.Blazor.Server.Models.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Adp");

                    b.Property<int>("Best");

                    b.Property<int>("ByeWeek");

                    b.Property<decimal?>("DefensiveTouchdowns");

                    b.Property<decimal?>("ExtraPoints");

                    b.Property<decimal?>("FieldGoals");

                    b.Property<decimal?>("FieldGoalsAttempted");

                    b.Property<decimal?>("FieldGoalsMissed");

                    b.Property<decimal?>("ForcedFumbles");

                    b.Property<decimal?>("FumblesLost");

                    b.Property<decimal?>("FumblesRecovered");

                    b.Property<decimal?>("Interceptions");

                    b.Property<string>("Name");

                    b.Property<decimal?>("PassingAttempts");

                    b.Property<decimal?>("PassingCompletions");

                    b.Property<decimal?>("PassingInterceptions");

                    b.Property<decimal?>("PassingTouchdowns");

                    b.Property<decimal?>("PassingYards");

                    b.Property<decimal?>("PointsAgainst");

                    b.Property<int>("Position");

                    b.Property<decimal?>("ReceivingTouchdowns");

                    b.Property<decimal?>("ReceivingYards");

                    b.Property<decimal?>("Receptions");

                    b.Property<decimal?>("RushingAttempts");

                    b.Property<decimal?>("RushingTouchdowns");

                    b.Property<decimal?>("RushingYards");

                    b.Property<decimal?>("Sacks");

                    b.Property<decimal?>("Safeties");

                    b.Property<int>("SeasonId");

                    b.Property<string>("TeamShortName");

                    b.Property<int>("Worst");

                    b.Property<decimal?>("YardsAgainst");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("OnTap.Blazor.Server.Models.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("LeagueId");

                    b.Property<string>("Name");

                    b.Property<string>("OwnerName");

                    b.HasKey("Id");

                    b.HasIndex("LeagueId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("OnTap.Blazor.Server.Models.Draft", b =>
                {
                    b.HasOne("OnTap.Blazor.Server.Models.League", "League")
                        .WithMany("Drafts")
                        .HasForeignKey("LeagueId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("OnTap.Blazor.Server.Models.DraftPick", b =>
                {
                    b.HasOne("OnTap.Blazor.Server.Models.Draft", "Draft")
                        .WithMany("DraftPicks")
                        .HasForeignKey("DraftId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("OnTap.Blazor.Server.Models.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("OnTap.Blazor.Server.Models.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("OnTap.Blazor.Server.Models.DraftPosition", b =>
                {
                    b.HasOne("OnTap.Blazor.Server.Models.Draft", "Draft")
                        .WithMany("DraftPositions")
                        .HasForeignKey("DraftId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("OnTap.Blazor.Server.Models.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("OnTap.Blazor.Server.Models.LeagueSettings", b =>
                {
                    b.HasOne("OnTap.Blazor.Server.Models.League", "League")
                        .WithOne("LeagueSettings")
                        .HasForeignKey("OnTap.Blazor.Server.Models.LeagueSettings", "LeagueId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("OnTap.Blazor.Server.Models.Team", b =>
                {
                    b.HasOne("OnTap.Blazor.Server.Models.League", "League")
                        .WithMany("Teams")
                        .HasForeignKey("LeagueId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
