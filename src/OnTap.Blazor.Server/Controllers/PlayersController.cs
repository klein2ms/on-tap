using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnTap.Blazor.Server.Data;
using OnTap.Blazor.Server.Services;
using OnTap.Blazor.Shared;

namespace OnTap.Blazor.Server.Controllers
{
    [ApiController]
    [Route("players")]
    public class PlayersController : ControllerBase
    {
        private readonly ILogger<PlayersController> _logger;
        private readonly IPlayerService _service;

        public PlayersController(
            ILogger<PlayersController> logger,
            IPlayerService service)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            _service = service ??
                throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PlayerViewModel>>> Get()
        {
            var(_, players) = await _service.GetPlayersAsync();
            return Ok(players);
        }

        [HttpGet("{playerId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PlayerViewModel>> Get(int playerId)
        {
            var(ok, player) = await _service.GetPlayerAsync(playerId);

            if (ok)
                return Ok(player);

            return NotFound();
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PlayerViewModel>> Post([FromBody] PlayerViewModel player)
        {
            var(ok, result) = await _service.AddOrUpdatePlayerAsync(player);

            if (ok)
                return CreatedAtAction(nameof(Get), new { playerId = result.Id } , result);

            return BadRequest();
        }

        [HttpDelete("{playerId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int playerId)
        {
            var(ok, _) = await _service.RemovePlayerAsync(playerId);

            if (ok)
                return NoContent();

            return NotFound();
        }
    }
}