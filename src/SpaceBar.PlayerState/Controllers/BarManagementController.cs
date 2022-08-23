using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpaceBar.PlayerState.Services;

namespace SpaceBar.PlayerState.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BarManagementController : ControllerBase
    {
        private readonly IPlayerStateService _playerStateService;

        public BarManagementController(IPlayerStateService playerStateService)
        {
            _playerStateService = playerStateService;
        }

        /* Change Bar Type */
        [HttpPut("{playerId}/{barId}")]
        public async Task<IActionResult> Put(Guid playerId, Guid barId,
            [FromQuery]Models.BarType newBarType)
        {
            var playerState = await _playerStateService.Get(playerId);
            if(playerState == null)
            {
                return NotFound();
            }
            var bar = playerState.Bars?.FirstOrDefault(b => b.Id == barId);
            if (bar == null)
            {
                return NotFound();
            }
            bar.BarType = newBarType;
            playerState.Funds -= (int)(playerState.Funds * 0.1);
            await _playerStateService.Save(playerState);
            return Ok();
        }
    }
}