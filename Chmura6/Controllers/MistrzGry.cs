using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MistrzyGry.Domain.Events;
using PWC.Common.Domain.Bus;
using PWC.Common.Domain.Events;
using PWC.Infra.Bus;

namespace MistrzGry.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MistrzGry : ControllerBase
    {
        private readonly IEventBus _eventBus;

        public MistrzGry(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }
        [HttpPost("TakeCharacter")]
        public async Task<IActionResult> TakeCharacter(int characterId, int enemyId)
        {
            try
            {
                // Tworzenie zdarzenia
                var takeCharacterEvent = new TakeCharacterEvent(characterId, enemyId);

                // Publikowanie zdarzenia
                await _eventBus.Publish(takeCharacterEvent);

                return Ok(new { Message = "TakeCharacterEvent published successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
            }
        }


    }
}
