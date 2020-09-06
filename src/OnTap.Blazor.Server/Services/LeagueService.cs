using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnTap.Blazor.Server.Data;
using OnTap.Blazor.Server.Models;
using OnTap.Blazor.Shared;

namespace OnTap.Blazor.Server.Services
{
    public class LeagueService : ILeagueService
    {
        private readonly ILogger<LeagueService> _logger;
        private readonly DraftContext _context;
        private readonly IMapper _mapper;

        public LeagueService(
            ILogger<LeagueService> logger,
            DraftContext context,
            IMapper mapper)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            _context = context ??
                throw new ArgumentNullException(nameof(context));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public async Task < (bool ok, LeagueViewModel league) > GetLeagueAsync(int leagueId)
        {
            var exists = await _context.Leagues.AnyAsync(l => l.Id == leagueId);

            if (!exists)
                return (false, new LeagueViewModel());

            var league = await _context
                .Leagues
                .Where(l => l.Id == leagueId)
                .ProjectTo<LeagueViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            league.Teams = league.Teams.ToList();

            return (true, league);
        }

        public async Task < (bool ok, LeagueViewModel league) > AddOrUpdateLeagueAsync(LeagueViewModel league)
        {
            if (league == null)
                return (false, new LeagueViewModel());

            var dto = _mapper.Map<League>(league);

            if (_context.Leagues.Any(l => l.Id == dto.Id))
                _context.Leagues.Update(dto);
            else
                _context.Leagues.Add(dto);

            var result = await _context.SaveChangesAsync();
            return (result != 0, _mapper.Map<LeagueViewModel>(dto));
        }

        public async Task < (bool ok, LeagueViewModel league) > RemoveLeagueAsync(int leagueId)
        {
            var exists = await _context.Leagues.AnyAsync(l => l.Id == leagueId);

            if (!exists)
                return (false, new LeagueViewModel());

            var toRemove = await _context
                .Leagues
                .FirstOrDefaultAsync(l => l.Id == leagueId);

            _ = _context.Leagues.Remove(toRemove);
            var result = await _context.SaveChangesAsync();
            return (result != 0, _mapper.Map<LeagueViewModel>(toRemove));
        }

        public async Task < (bool ok, IEnumerable<TeamViewModel> teams) > GetTeamsAsync(int leagueId)
        {
            var exists = await _context.Leagues.AnyAsync(l => l.Id == leagueId);

            if (!exists)
                return (false, new List<TeamViewModel>());

            var results = await _context
                .Teams
                .Where(t => t.LeagueId == leagueId)
                .ProjectTo<TeamViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return (true, results);
        }

        public async Task < (bool ok, IEnumerable<TeamViewModel> teams) > AddOrUpdateTeamsAsync(
            int leagueId,
            IReadOnlyCollection<TeamViewModel> teams)
        {
            if (teams == null)
                return (false, new List<TeamViewModel>());

            if (!await _context.Leagues.AnyAsync(l => l.Id == leagueId))
                return (false, new List<TeamViewModel>());
            
            var league = await _context
                .Leagues
                .Where(l => l.Id == leagueId)
                .ProjectTo<League>(_mapper.ConfigurationProvider)
                .FirstAsync();
            
            teams
                .Select(_mapper.Map<Team>)
                .ToList()
                .ForEach(d => 
                {                    
                    if (league.Teams.Any(t => t.Id == d.Id))
                    {                        
                        var team = league.Teams.First(t => t.Id == d.Id);
                        
                        if (team == d)
                            return;
                       
                        team.Name = d.Name;
                        team.OwnerName = d.OwnerName;
                    }
                    else
                    {
                        league.Teams.Add(d);                            
                    }
                    
                    _context.Leagues.Update(league);                        
                });                                  

            var result = await _context.SaveChangesAsync();
            
            var toReturn = await _context
                .Teams
                .Where(t => t.LeagueId == leagueId)
                .ProjectTo<TeamViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return (result != 0, toReturn);
        }

        public async Task < (bool ok, IEnumerable<TeamViewModel> teams) > RemoveTeamsAsync(
            int leagueId,
            IReadOnlyCollection<TeamViewModel> teams)
        {
            if (teams == null)
                return (false, new List<TeamViewModel>());

            var toRemove = await _context
                .Teams
                .Where(t => t.LeagueId == leagueId && teams.Select(vm => vm.Id).Contains(t.Id))
                .ToListAsync();

            _context.Teams.RemoveRange(toRemove);
            var result = await _context.SaveChangesAsync();
            return (result != 0, toRemove.Select(d => _mapper.Map<TeamViewModel>(d)).ToList());
        }

        public async Task < (bool ok, LeagueSettingsViewModel settings) > GetLeagueSettingsAsync(int leagueId)
        {
            var exists = await _context.LeagueSettings.AnyAsync(l => l.LeagueId == leagueId);

            if (!exists)
                return (false, new LeagueSettingsViewModel());

            var results = await _context
                .LeagueSettings
                .Where(l => l.LeagueId == leagueId)
                .ProjectTo<LeagueSettingsViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return (true, results);
        }

        public async Task < (bool ok, LeagueSettingsViewModel) > AddOrUpdateLeagueSettingsAsync(LeagueSettingsViewModel settings)
        {
            if (settings == null)
                return (false, new LeagueSettingsViewModel());

            var league = await _context
                .Leagues
                .FirstOrDefaultAsync(s => s.Id == settings.LeagueId);

            if (league == null)
                return (false, new LeagueSettingsViewModel());

            var dto = _mapper.Map<LeagueSettings>(settings);

            league.LeagueSettings = dto;
            _ = _context.Leagues.Update(league);

            if (league.LeagueSettings == null)
                _ = _context.LeagueSettings.Add(dto);
            else
                _ = _context.LeagueSettings.Update(dto);

            var results = await _context.SaveChangesAsync();

            var toReturn = await _context
                .LeagueSettings
                .Where(s => s.Id == dto.Id)
                .ProjectTo<LeagueSettingsViewModel>(_mapper.ConfigurationProvider)
                .FirstAsync();

            return (results != 0, toReturn);
        }
    }
}