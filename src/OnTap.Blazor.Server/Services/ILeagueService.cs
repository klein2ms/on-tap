using System.Collections.Generic;
using System.Threading.Tasks;
using OnTap.Blazor.Shared;

namespace OnTap.Blazor.Server.Services
{
    public interface ILeagueService
    {
        Task < (bool ok, LeagueViewModel league) > GetLeagueAsync(int leagueId);
        Task < (bool ok, LeagueViewModel league) > AddOrUpdateLeagueAsync(LeagueViewModel league);
        Task < (bool ok, LeagueViewModel league) > RemoveLeagueAsync(int leagueId);
        Task < (bool ok, IEnumerable<TeamViewModel> teams) > GetTeamsAsync(int leagueId);
        Task < (bool ok, IEnumerable<TeamViewModel> teams) > AddOrUpdateTeamsAsync(
            int leagueId,
            IReadOnlyCollection<TeamViewModel> teams);
        Task < (bool ok, IEnumerable<TeamViewModel> teams) > RemoveTeamsAsync(
            int leagueId,
            IReadOnlyCollection<TeamViewModel> teams);
        Task < (bool ok, LeagueSettingsViewModel settings) > GetLeagueSettingsAsync(int leagueId);
        Task < (bool ok, LeagueSettingsViewModel) > AddOrUpdateLeagueSettingsAsync(LeagueSettingsViewModel settings);
    }
}