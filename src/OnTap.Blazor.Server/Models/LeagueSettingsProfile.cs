using System.Linq;
using AutoMapper;
using OnTap.Blazor.Shared;

namespace OnTap.Blazor.Server.Models
{
    public class LeagueSettingsProfile : Profile
    {
        public LeagueSettingsProfile()
        {
            CreateMap<LeagueSettings, LeagueSettingsViewModel>()
                .ForMember(d => d.NumberOfTeams, opt => opt.MapFrom(src => src.League.Teams.Count()))
                .ReverseMap();
        }
    }
}