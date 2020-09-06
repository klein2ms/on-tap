using System;
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
    public class DraftService : IDraftService
    {
        private readonly ILogger<DraftService> _logger;
        private readonly DraftContext _context;
        private readonly IMapper _mapper;
        public DraftService(
            ILogger<DraftService> logger,
            DraftContext context,
            IMapper mapper)
        {
            _logger = logger
                ??
                throw new ArgumentNullException(nameof(logger));
            _context = context
                ??
                throw new ArgumentNullException(nameof(context));
            _mapper = mapper
                ??
                throw new ArgumentNullException(nameof(mapper));
        }

        public async Task < (bool ok, DraftViewModel draft) > GetDraftAsync(int draftId)
        {
            if (!await _context.Drafts.AnyAsync(d => d.Id == draftId))
                return (false, new DraftViewModel());

            var draft = await _context
                .Drafts
                .Where(d => d.Id == draftId)
                .ProjectTo<DraftViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            draft.DraftPositions = draft.DraftPositions.ToList();
            draft.DraftPicks = draft.DraftPicks.ToList();
            draft.League.Teams = draft.League.Teams.ToList();

            return (true, draft);
        }

        public async Task < (bool ok, DraftViewModel) > AddOrUpdateDraftAsync(DraftViewModel draft)
        {
            if (draft == null || draft.League == null)
                return (false, new DraftViewModel());

            if (!await _context.Leagues.AnyAsync(l => l.Id == draft.League.Id))
                return (false, new DraftViewModel());

            var dto = _mapper.Map<Draft>(draft);

            var league = await _context
                .Leagues
                .Where(l => l.Id == dto.LeagueId)
                .FirstAsync();

            dto.League = league;

            if (_context.Drafts.Any(d => d.Id == dto.Id))
                _context.Drafts.Update(dto);
            else
                _context.Drafts.Add(dto);

            dto.DraftPositions = dto.DraftPositions.ToList();
            dto.DraftPicks = dto.DraftPicks.ToList();

            var result = await _context.SaveChangesAsync();

            var draftViewModel = _mapper.Map<DraftViewModel>(dto);
            var leagueViewModel = await _context
                .Leagues
                .Where(l => l.Id == league.Id)
                .ProjectTo<LeagueViewModel>(_mapper.ConfigurationProvider)
                .FirstAsync();

            leagueViewModel.Teams = leagueViewModel.Teams.ToList();
            draftViewModel.League = leagueViewModel;

            return (result != 0, draftViewModel);
        }

        public async Task < (bool ok, DraftViewModel draft) > RemoveDraftAsync(int draftId)
        {
            if (!await _context.Drafts.AnyAsync(d => d.Id == draftId))
                return (false, new DraftViewModel());

            var toRemove = await _context
                .Drafts
                .FirstOrDefaultAsync(d => d.Id == draftId);

            _ = _context.Drafts.Remove(toRemove);
            var result = await _context.SaveChangesAsync();
            return (result != 0, _mapper.Map<DraftViewModel>(toRemove));
        }

        public async Task < (bool ok, DraftPickViewModel pick) > AddDraftPickAsync(DraftPickViewModel draftPick)
        {
            if (draftPick == null || draftPick.Team == null || draftPick.Player == null)
                return (false, new DraftPickViewModel());

            if (!await _context.Drafts.AnyAsync(d => d.Id == draftPick.DraftId))
                return (false, new DraftPickViewModel());

            if (!await _context.Teams.AnyAsync(t => t.Id == draftPick.Team.Id))
                return (false, new DraftPickViewModel());

            if (!await _context.Players.AnyAsync(p => p.Id == draftPick.Player.Id))
                return (false, new DraftPickViewModel());

            var dto = _mapper.Map<DraftPick>(draftPick);

            var draft = await _context
                .Drafts
                .Where(d => d.Id == draftPick.DraftId)
                .FirstAsync();

            var team = await _context
                .Teams
                .Where(t => t.Id == dto.TeamId)
                .FirstAsync();

            var player = await _context
                .Players
                .Where(p => p.Id == dto.PlayerId)
                .FirstAsync();

            dto.Draft = draft;
            dto.Team = team;
            dto.Player = player;

            _ = _context.DraftPicks.Add(dto);
            var result = await _context.SaveChangesAsync();

            var draftPickViewModel = await _context
                .DraftPicks
                .Where(d => d.Id == dto.Id)
                .ProjectTo<DraftPickViewModel>(_mapper.ConfigurationProvider)
                .FirstAsync();

            return (result != 0, draftPickViewModel);
        }

        public async Task < (bool ok, DraftPickViewModel pick) > RemoveDraftPickAsync(int draftPickId)
        {
            if (!await _context.DraftPicks.AnyAsync(d => d.Id == draftPickId))
                return (false, new DraftPickViewModel());

            var toRemove = await _context
                .DraftPicks
                .ProjectTo<DraftPick>(_mapper.ConfigurationProvider)
                .FirstAsync(d => d.Id == draftPickId);

            _ = _context.DraftPicks.Remove(toRemove);
            var result = await _context.SaveChangesAsync();
            return (result != 0, _mapper.Map<DraftPickViewModel>(toRemove));
        }
    }
}