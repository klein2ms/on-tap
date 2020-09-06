using OnTap.Blazor.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnTap.Blazor.Server.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OnTap.Blazor.Server.Controllers
{
    [ApiController]
    [Route("leagues")]
    public class LeaguesController : ControllerBase
    {
        private readonly ILogger<LeaguesController> _logger;
        private readonly ILeagueService _service;

        public LeaguesController(
            ILogger<LeaguesController> logger,
            ILeagueService service)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }
 
        [HttpGet("{leagueId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LeagueViewModel>> Get(int leagueId)
        {
            var (ok, league) = await _service.GetLeagueAsync(leagueId);

            if (ok)
                return Ok(league);

            return NotFound();
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LeagueViewModel>> Post([FromBody] LeagueViewModel league)
        {
            var (ok, result) = await _service.AddOrUpdateLeagueAsync(league);

            if (ok)
                return CreatedAtAction(nameof(Get), new { leagueId = result.Id }, league);
            
            return BadRequest();
        }

        [HttpDelete("{leagueId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int leagueId)
        {
            var (ok, _) = await _service.RemoveLeagueAsync(leagueId);

            if (ok)
                return NoContent();
            
            return NotFound();
        }

        [HttpGet("{leagueId}/teams")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<TeamViewModel>>> GetTeams(int leagueId)
        {
            var (ok, teams) = await _service.GetTeamsAsync(leagueId);

            if (ok)
                return Ok(teams);

            return NotFound();
        }

        [HttpPost("{leagueId}/teams")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<TeamViewModel>>> PostTeam(int leagueId, [FromBody] IEnumerable<TeamViewModel> teams)
        {
            var (ok, result) = await _service.AddOrUpdateTeamsAsync(leagueId, teams.ToList());

            if (ok)
                return CreatedAtAction(nameof(GetTeams), new { leagueId }, result);
            
            return BadRequest();
        }

        [HttpDelete("{leagueId}/teams")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteTeam(int leagueId, [FromBody] IEnumerable<TeamViewModel> teams)
        {
            var (ok, _) = await _service.RemoveTeamsAsync(leagueId, teams.ToList());

            if (ok)
                return NoContent();
            
            return NotFound();
        }

        [HttpGet("{leagueId}/settings")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LeagueSettingsViewModel>> GetLeagueSettings(int leagueId)
        {
            var (ok, settings) = await _service.GetLeagueSettingsAsync(leagueId);

            if (ok)
                return Ok(settings);

            return NotFound();
        }

        [HttpPost("{leagueId}/settings")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LeagueSettingsViewModel>> PostLeagueSettings(int leagueId, [FromBody] LeagueSettingsViewModel settings)
        {
            var (ok, result) = await _service.AddOrUpdateLeagueSettingsAsync(settings);
            
            if (ok)
                return CreatedAtAction(nameof(GetLeagueSettings), new { leagueId }, result);
            
            return BadRequest();
        }
    }
}