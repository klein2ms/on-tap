using AutoMapper;
using OnTap.Blazor.Shared;

namespace OnTap.Blazor.Server.Models
{
    public class LeagueProfile : Profile
    {
        public LeagueProfile()
        {
            CreateMap<League, LeagueViewModel>()
                .ReverseMap();
            
            CreateMap<League, League>();            
        }
    }
}