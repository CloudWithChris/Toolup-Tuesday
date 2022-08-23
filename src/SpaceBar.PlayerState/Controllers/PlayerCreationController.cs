using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpaceBar.PlayerState.Services;

namespace SpaceBar.PlayerState.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerCreationController : ControllerBase
    {
        private readonly IPlayerStateService _playerStateService;

        public PlayerCreationController(IPlayerStateService playerStateService)
        {
            _playerStateService = playerStateService;
        }

        /* Create player API */
        [HttpPost]
        public async Task<IActionResult> Post(string firstBarName)
        {
            var playerState = new Models.PlayerState
            {
                Id = Guid.NewGuid(),
                Bars = new Models.Bar[]{
                    new Models.Bar
                    {
                        Id = Guid.NewGuid(),
                        Name = firstBarName,
                        Value = 0,
                        BarType = Models.BarType.Bar
                    }
                },
                Funds = 1000
            };

          //TODO: Figure out how to handle bad saves
          await _playerStateService.Save(playerState);
          return Ok(playerState);
        }
    }
}