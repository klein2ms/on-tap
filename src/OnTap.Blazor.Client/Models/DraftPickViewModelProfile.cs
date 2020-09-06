using AutoMapper;
using OnTap.Blazor.Shared;

namespace OnTap.Blazor.Server.Models
{
    public class DraftPickViewModelProfile : Profile
    {
        public DraftPickViewModelProfile()
        {
            _ = CreateMap<DraftPickViewModel, DraftPickViewModel>();
        }
    }
}