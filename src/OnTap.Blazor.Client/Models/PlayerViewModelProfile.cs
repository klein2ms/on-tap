using AutoMapper;
using OnTap.Blazor.Shared;

namespace OnTap.Blazor.Server.Models
{
    public class PlayerViewModelProfile : Profile
    {
        public PlayerViewModelProfile()
        {
            _ = CreateMap<PlayerViewModel, PlayerViewModel>();
        }
    }
}