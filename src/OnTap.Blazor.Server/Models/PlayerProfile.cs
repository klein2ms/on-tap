using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using OnTap.Blazor.Shared;

namespace OnTap.Blazor.Server.Models
{
    public class PlayerProfile : Profile
    {
        public PlayerProfile()
        {
            _ = CreateMap<Player, PlayerViewModel>()
                .ForMember(d => d.FullName, opts => opts.MapFrom(src => src.Name))
                .ForMember(d => d.FantasyPoints, opts => opts.Ignore())
                .ForMember(d => d.VbdScore, opts => opts.Ignore())
                .ForMember(d => d.VbdOverallRank, opts => opts.Ignore())
                .ForMember(d => d.VbdPositionRank, opts => opts.Ignore())
                .ForMember(d => d.ProjectedStats, opts => opts.MapFrom(src => MapStatisticsToVm(src)))
                .ForMember(d => d.IsDrafted, opts => opts.Ignore())
                .ReverseMap()
                .ForMember(d => d.PassingAttempts, opts => opts.MapFrom(src => MapStatisticFromVm(src, Statistic.PassingAttempts)))
                .ForMember(d => d.PassingCompletions, opts => opts.MapFrom(src => MapStatisticFromVm(src, Statistic.PassingCompletions)))
                .ForMember(d => d.PassingYards, opts => opts.MapFrom(src => MapStatisticFromVm(src, Statistic.PassingYards)))
                .ForMember(d => d.PassingTouchdowns, opts => opts.MapFrom(src => MapStatisticFromVm(src, Statistic.PassingTouchdowns)))
                .ForMember(d => d.PassingInterceptions, opts => opts.MapFrom(src => MapStatisticFromVm(src, Statistic.PassingInterceptions)))
                .ForMember(d => d.RushingAttempts, opts => opts.MapFrom(src => MapStatisticFromVm(src, Statistic.RushingAttempts)))
                .ForMember(d => d.RushingYards, opts => opts.MapFrom(src => MapStatisticFromVm(src, Statistic.RushingYards)))
                .ForMember(d => d.RushingTouchdowns, opts => opts.MapFrom(src => MapStatisticFromVm(src, Statistic.RushingTouchdowns)))
                .ForMember(d => d.Receptions, opts => opts.MapFrom(src => MapStatisticFromVm(src, Statistic.Receptions)))
                .ForMember(d => d.ReceivingYards, opts => opts.MapFrom(src => MapStatisticFromVm(src, Statistic.ReceivingYards)))
                .ForMember(d => d.ReceivingTouchdowns, opts => opts.MapFrom(src => MapStatisticFromVm(src, Statistic.ReceivingTouchdowns)))
                .ForMember(d => d.FumblesLost, opts => opts.MapFrom(src => MapStatisticFromVm(src, Statistic.FumblesLost)))
                .ForMember(d => d.FieldGoals, opts => opts.MapFrom(src => MapStatisticFromVm(src, Statistic.FieldGoals)))
                .ForMember(d => d.FieldGoalsAttempted, opts => opts.MapFrom(src => MapStatisticFromVm(src, Statistic.FieldGoalsAttempted)))
                .ForMember(d => d.FieldGoalsMissed, opts => opts.MapFrom(src => MapStatisticFromVm(src, Statistic.FieldGoalsMissed)))
                .ForMember(d => d.ExtraPoints, opts => opts.MapFrom(src => MapStatisticFromVm(src, Statistic.ExtraPoints)))
                .ForMember(d => d.Sacks, opts => opts.MapFrom(src => MapStatisticFromVm(src, Statistic.Sacks)))
                .ForMember(d => d.Interceptions, opts => opts.MapFrom(src => MapStatisticFromVm(src, Statistic.Interceptions)))
                .ForMember(d => d.FumblesRecovered, opts => opts.MapFrom(src => MapStatisticFromVm(src, Statistic.FumblesRecovered)))
                .ForMember(d => d.ForcedFumbles, opts => opts.MapFrom(src => MapStatisticFromVm(src, Statistic.ForcedFumbles)))
                .ForMember(d => d.DefensiveTouchdowns, opts => opts.MapFrom(src => MapStatisticFromVm(src, Statistic.DefensiveTouchdowns)))
                .ForMember(d => d.Safeties, opts => opts.MapFrom(src => MapStatisticFromVm(src, Statistic.Safeties)))
                .ForMember(d => d.PointsAgainst, opts => opts.MapFrom(src => MapStatisticFromVm(src, Statistic.PointsAgainst)))
                .ForMember(d => d.YardsAgainst, opts => opts.MapFrom(src => MapStatisticFromVm(src, Statistic.YardsAgainst)));                
            
            _ = CreateMap<PlayerViewModel, PlayerViewModel>();
        }

        private decimal? MapStatisticFromVm(PlayerViewModel vm, Statistic stat)
        {
            var toReturn = vm.ProjectedStats.FirstOrDefault(s => s.Stat == stat);

            if (toReturn == default)
                return null;

            return toReturn.Value;
        }

        private IEnumerable<StatisticViewModel> MapStatisticsToVm(Player player) =>
            new List<(Statistic stat, decimal? value)>
            {
                (Statistic.PassingAttempts, player.PassingAttempts),
                (Statistic.PassingCompletions, player.PassingCompletions),
                (Statistic.PassingYards, player.PassingYards),
                (Statistic.PassingTouchdowns, player.PassingTouchdowns),
                (Statistic.PassingInterceptions, player.PassingInterceptions),
                (Statistic.RushingAttempts, player.RushingAttempts),
                (Statistic.RushingYards, player.RushingYards),
                (Statistic.RushingTouchdowns, player.RushingTouchdowns),
                (Statistic.Receptions, player.Receptions),
                (Statistic.ReceivingYards, player.ReceivingYards),
                (Statistic.ReceivingTouchdowns, player.ReceivingTouchdowns),
                (Statistic.FumblesLost, player.FumblesLost),
                (Statistic.FieldGoals, player.FieldGoals),
                (Statistic.FieldGoalsAttempted, player.FieldGoalsAttempted),
                (Statistic.FieldGoalsMissed, player.FieldGoalsMissed),
                (Statistic.ExtraPoints, player.ExtraPoints),
                (Statistic.Sacks, player.Sacks),
                (Statistic.Interceptions, player.Interceptions),
                (Statistic.FumblesRecovered, player.FumblesRecovered),
                (Statistic.ForcedFumbles, player.ForcedFumbles),
                (Statistic.DefensiveTouchdowns, player.DefensiveTouchdowns),
                (Statistic.Safeties, player.Safeties),
                (Statistic.PointsAgainst, player.PointsAgainst),
                (Statistic.YardsAgainst, player.YardsAgainst),
            }
            .Where(s => s.value.HasValue)
            .Select(s => new StatisticViewModel{ Stat = s.stat, Value = s.value.Value })
            .ToList();
    }
}