using AutoMapper;
using OnTap.Blazor.Shared;

namespace OnTap.Blazor.Server.Models
{
    public class DraftPickProfile : Profile
    {
        public DraftPickProfile()
        {
            CreateMap<DraftPick, DraftPickViewModel>()
                .ReverseMap();
            
            CreateMap<DraftPick, DraftPick>();
        }
    }
}