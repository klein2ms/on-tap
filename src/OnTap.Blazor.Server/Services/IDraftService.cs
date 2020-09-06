using System.Threading.Tasks;
using OnTap.Blazor.Shared;

namespace OnTap.Blazor.Server.Services
{
    public interface IDraftService
    {
        Task < (bool ok, DraftViewModel draft) > GetDraftAsync(int draftId);
        Task < (bool ok, DraftViewModel) > AddOrUpdateDraftAsync(DraftViewModel draft);
        Task < (bool ok, DraftViewModel draft) > RemoveDraftAsync(int draftId);
        Task < (bool ok, DraftPickViewModel pick) > AddDraftPickAsync(DraftPickViewModel draftPick);
        Task < (bool ok, DraftPickViewModel pick) > RemoveDraftPickAsync(int draftPickId);
    }
}