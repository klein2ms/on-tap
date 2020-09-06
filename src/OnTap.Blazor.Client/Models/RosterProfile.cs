using AutoMapper;
using OnTap.Blazor.Shared;

namespace OnTap.Blazor.Server.Models
{
    public class RosterSpotProfile : Profile
    {
        public RosterSpotProfile()
        {
            _ = CreateMap<RosterSpot, RosterSpot>();
        }
    }
}