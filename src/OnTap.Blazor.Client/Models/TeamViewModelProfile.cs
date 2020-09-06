using AutoMapper;
using OnTap.Blazor.Shared;

namespace OnTap.Blazor.Server.Models
{
    public class TeamViewModelProfile : Profile
    {
        public TeamViewModelProfile()
        {
            _ = CreateMap<TeamViewModel, TeamViewModel>();
        }
    }
}