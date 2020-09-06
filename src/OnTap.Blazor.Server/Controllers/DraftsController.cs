using System;
using System.Collections.Generic;
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
    [Route("drafts")]
    public class DraftsController : ControllerBase
    {
        private readonly ILogger<DraftsController> _logger;
        private readonly IDraftService _service;

        public DraftsController(
            ILogger<DraftsController> logger,
            IDraftService service)
        {
            _logger = logger
                ??
                throw new ArgumentNullException(nameof(logger));
            _service = service
                ??
                throw new ArgumentNullException(nameof(service));
        }

        [HttpGet("{draftId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DraftViewModel>> Get(int draftId)
        {
            var(ok, draft) = await _service.GetDraftAsync(draftId);

            if (ok)
                return Ok(draft);

            return NotFound();
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DraftViewModel>> Post([FromBody] DraftViewModel draft)
        {
            var(ok, result) = await _service.AddOrUpdateDraftAsync(draft);

            if (ok)
                return CreatedAtAction(nameof(Get), new { draftId = result.Id }, result);

            return BadRequest();
        }

        [HttpDelete("{draftId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int draftId)
        {
            var(ok, _) = await _service.RemoveDraftAsync(draftId);

            if (ok)
                return NoContent();

            return NotFound();
        }

        [HttpPost("{draftId}/draft-picks")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DraftPickViewModel>> PostDraftPick(int draftId, [FromBody] DraftPickViewModel draftPick)
        {
            var(ok, result) = await _service.AddDraftPickAsync(draftPick);

            if (ok)
                return CreatedAtAction(nameof(Get), new { draftId = result.DraftId }, result);

            return BadRequest();
        }

        [HttpDelete("{draftId}/draft-picks/{draftPickId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int draftId, int draftPickId)
        {
            var(ok, _) = await _service.RemoveDraftPickAsync(draftPickId);

            if (ok)
                return NoContent();

            return NotFound();
        }
    }
}