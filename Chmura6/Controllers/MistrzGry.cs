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
        private readonly ILogger<MistrzGry> _logger;    

        public MistrzGry(IEventBus eventBus, ILogger<MistrzGry> logger)
        {
            _logger = logger;
            _eventBus = eventBus;
        }
        [HttpPost("TakeCharacter")]
        public async Task<IActionResult> TakeCharacter(int characterId, int enemyId)
        {
            try
            {
                // Walidacja parametrów  
                if (characterId <= 0)
                {

                    _logger.LogError("Invalid characterId: {CharacterId}", characterId);
                    return BadRequest(new { Error = "Invalid characterId. It must be greater than 0." });
                }

                if (enemyId <= 0)
                {
                    _logger.LogError("Invalid enemyId: {EnemyId}", enemyId);
                    return BadRequest(new { Error = "Invalid enemyId. It must be greater than 0." });
                }


                if (characterId == null || enemyId == null)
                {
                    _logger.LogError("characterId or enemyId is null.");
                    return NotFound(new { Error = "characterId and enemyId cannot be null." });
                }
                // Tworzenie zdarzenia  
                var takeCharacterEvent = new TakeCharacterEvent(characterId, enemyId);

                // Publikowanie zdarzenia  
                await _eventBus.Publish(takeCharacterEvent);
                _logger.LogInformation("TakeCharacterEvent published: CharacterId={CharacterId}, EnemyId={EnemyId}", characterId, enemyId);
                return Ok(new { Message = "TakeCharacterEvent published successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Error = ex.Message });
            }
        }


    }
}
