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
    public class PlayerService : IPlayerService
    {
        private readonly ILogger<PlayerService> _logger;
        private readonly DraftContext _context;
        private readonly IMapper _mapper;

        public PlayerService(
            ILogger<PlayerService> logger,
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

        public async Task<(bool ok, PlayerViewModel player)> AddOrUpdatePlayerAsync(PlayerViewModel player)
        {
            if (player == null)
                return (false, new PlayerViewModel());
            
            var dto = _mapper.Map<Player>(player);

            if (await _context.Players.AnyAsync(p => p.Id == dto.Id))
                _context.Players.Update(dto);
            else
                _context.Players.Add(dto);
            
            var result = await _context.SaveChangesAsync();
            return (result != 0, _mapper.Map<PlayerViewModel>(dto));
        }

        public async Task<(bool ok, PlayerViewModel player)> GetPlayerAsync(int playerId)
        {
            if (!await _context.Players.AnyAsync(p => p.Id == playerId))
                return (false, new PlayerViewModel());
            
            var player = await _context
                .Players
                .Where(p => p.Id == playerId)
                .ProjectTo<PlayerViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            
            return (true, player);
        }

        public async Task<(bool ok, IEnumerable<PlayerViewModel> players)> GetPlayersAsync()
        {
            var players = await _context
                .Players
                .ProjectTo<PlayerViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
            
            return (true, players);
        }        

        public async Task<(bool ok, PlayerViewModel player)> RemovePlayerAsync(int playerId)
        {
            if (!await _context.Players.AnyAsync(p => p.Id == playerId))
                return (false, new PlayerViewModel());
            
            var toRemove = await _context
                .Players
                .FirstAsync(p => p.Id == playerId);
            
            _ = _context.Players.Remove(toRemove);
            var result = await _context.SaveChangesAsync();
            return (result != 0, _mapper.Map<PlayerViewModel>(toRemove));
        }
    }
}