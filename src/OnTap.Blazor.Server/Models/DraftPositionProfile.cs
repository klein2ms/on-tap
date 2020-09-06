using AutoMapper;
using OnTap.Blazor.Shared;

namespace OnTap.Blazor.Server.Models
{
    public class DraftPositionProfile : Profile
    {
        public DraftPositionProfile()
        {
            CreateMap<DraftPosition, DraftPositionViewModel>()
                .ReverseMap();
        }
    }
}