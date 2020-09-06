using AutoMapper;
using OnTap.Blazor.Shared;

namespace OnTap.Blazor.Server.Models
{
    public class DraftProfile : Profile
    {
        public DraftProfile()
        {
            CreateMap<Draft, DraftViewModel>()
                .ReverseMap();
        }
    }
}