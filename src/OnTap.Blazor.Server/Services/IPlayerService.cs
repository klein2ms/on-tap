using System.Collections.Generic;
using System.Threading.Tasks;
using OnTap.Blazor.Shared;

namespace OnTap.Blazor.Server.Services
{
    public interface IPlayerService
    {
        Task < (bool ok, IEnumerable<PlayerViewModel> players) > GetPlayersAsync();
        Task < (bool ok, PlayerViewModel player) > GetPlayerAsync(int playerId);
        Task < (bool ok, PlayerViewModel player) > AddOrUpdatePlayerAsync(PlayerViewModel player);
        Task < (bool ok, PlayerViewModel player) > RemovePlayerAsync(int playerId);
    }
}