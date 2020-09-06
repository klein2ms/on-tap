using AutoMapper;
using OnTap.Blazor.Shared;

namespace OnTap.Blazor.Server.Models
{
    public class TeamProfile : Profile
    {
        public TeamProfile()
        {
            CreateMap<Team, TeamViewModel>()
                .ForMember(d => d.Owner, opts => opts.MapFrom(src => new OwnerViewModel { Name = src.OwnerName }))
                .ForMember(d => d.Roster, opts => opts.MapFrom(src => new Roster()))
                .ForMember(d => d.DraftPosition, opts => opts.Ignore())
                .ForMember(d => d.IsDrafting, opts => opts.Ignore())
                .ReverseMap();

            CreateMap<Team, Team>();
        }
    }
}