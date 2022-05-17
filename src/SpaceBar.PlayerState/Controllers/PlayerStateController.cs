using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpaceBar.PlayerState.Services;

namespace SpaceBar.PlayerState.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerStateController : ControllerBase
    {
        private readonly IPlayerStateService _playerStateService;

        public PlayerStateController(IPlayerStateService playerStateService)
        {
            _playerStateService = playerStateService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]Guid id)
        {
            return new JsonResult(await _playerStateService.Get(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Models.PlayerState playerState)
        {
            //TODO: Figure out how to handle bad saves
            await _playerStateService.Save(playerState);
            return Ok("Player Updated");
        }
    }
}