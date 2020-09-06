using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Components;
using OnTap.Blazor.Shared;

namespace OnTap.Blazor.Client.Pages
{
    public class DraftBoardBase : ComponentBase
    {
        [Inject] HttpClient Http { get; set; }
        [Inject] IMapper Mapper { get; set; }

        [Parameter] public int DraftId { get; set; }

        protected PlayerViewModel[] players;       

        protected DraftPickViewModel[] recommendations
        {
            get
            {
                var remainingPlayers = players
                    .Where(p => p.Adp != 0 && !p.IsDrafted)
                    .Select(p => new PlayerViewModel
                    {
                        FullName = p.FullName,
                        Position = p.Position,
                        TeamShortName = p.TeamShortName
                    })
                    .ToList();
                return draft
                    .DraftPicks
                    .Where(p => p.Player == null)
                    .Take(24)
                    .Select(p =>
                    {
                        var(pick, _) = p
                            .Team
                            .Roster
                            .RecommendDraftPick(remainingPlayers, DraftPickRecommender.AdpComparer);

                        remainingPlayers.Remove(pick);                        
                        var toReturn = Mapper.Map<DraftPickViewModel>(p);
                        toReturn.Player = pick;
                        return toReturn;
                    })
                    .ToArray();
            }
        }

        protected DraftPickViewModel[] draftHistory => 
            draft
                .DraftPicks
                .Skip(CurrentDraftPick.Number - 5 < 0 ? 0 : CurrentDraftPick.Number - 5)
                .Take(25)
                .ToArray();                

        protected DraftPickViewModel[] draftPicks => draft.DraftPicks.ToArray();

        protected DraftViewModel draft;

        public DraftPickViewModel CurrentDraftPick { get; set; } 

        protected void HandleOnDraft(PlayerViewModel player)
        {
            CurrentDraftPick.DraftPlayer(player);                                             
            Console.WriteLine($"Drafting: {player.FullName}");

            CurrentDraftPick = GetCurrentDraftPick();

            CurrentDraftPick
                .Team
                .Roster
                .RecommendRoster(
                    DraftPickRecommender.VbdComparer,
                    players.Where(p => p.Adp != 0 && !p.IsDrafted).ToList());
            StateHasChanged();
        }

        private DraftPickViewModel GetCurrentDraftPick() => draftPicks
                .OrderBy(p => p.Number)
                .FirstOrDefault(p => p.Player == null);

        protected void HandleOnUndo(DraftPickViewModel draftPick)
        {
            draftPick.Player.IsDrafted = false;
            draftPick.Player = null;
            Console.WriteLine($"Undo: {draftPick.Player.FullName}");
        }

        protected override async Task OnInitAsync()
        {
            players = await Http.GetJsonAsync<PlayerViewModel[]>("players");

            var league = await Http.GetJsonAsync<LeagueViewModel>("leagues/1");
            //draft = await Http.GetJsonAsync<DraftViewModel>($"drafts/{DraftId}");
            draft = new DraftViewModel
            {
                League = league,
            };

            draft.DraftPicks = league
                .Teams
                .SelectMany((t, i) =>
                {
                    return DraftCalculator
                        .CalculateDraftPicks(i + 1, league.Teams.Count(), 14)
                        .Select(p => new DraftPickViewModel
                        {
                            DraftId = draft.Id,
                                Number = p,
                                Player = null,
                                Team = t
                        });
                })
                .OrderBy(p => p.Number)
                .ToArray();;

            league
                .Teams
                .ToList()
                .ForEach(t =>
                {
                    t.SeedRoster(league.LeagueSettings);
                });

            players = players
                .Select(p =>
                {
                    p.FantasyPoints = p.CalculateFantasyPoints(league.LeagueSettings);
                    return p;
                })
                .ToArray();

            var top100Baselines = VbdCalculator.GetTop100Baselines(players.ToList());

            players = VbdCalculator.CalculateDraftValue(top100Baselines, players.ToList()).ToArray();

            CurrentDraftPick = draftPicks
            .OrderBy(p => p.Number)
            .FirstOrDefault(p => p.Player == null);

        }
    }
}