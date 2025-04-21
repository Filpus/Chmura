using PWC.Common.Domain.Bus;
using System;
using BazaPostaci.Domain.Events; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PWC.Infra.Bus;
using BazaPostaci.Domain.Entities;
using Supabase;
using Microsoft.Extensions.Logging;

namespace BazaPostaci.Infrastructure.EventHandler
{
    public class TakeCharacterHandler : IEventHandler<TakeCharacterEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly SupabaseService<Character> _characterService;
        private readonly ILogger<TakeCharacterHandler> _logger;


        public TakeCharacterHandler(IEventBus eventBus, SupabaseService<Character> characterService, ILogger<TakeCharacterHandler> logger)
        {
            _eventBus = eventBus;
            _characterService = characterService;
            _logger = logger;
        }

        public async Task Handle(TakeCharacterEvent @event)
        {
            int characterId = @event.CharacterId;
            int enemyId = @event.EnemyId;
            Console.WriteLine($"Handling TakeCharacterEvent: CharacterId={characterId}, EnemyId={enemyId}");
            _logger.LogInformation("Handling TakeCharacterEvent: CharacterId={CharacterId}, EnemyId={EnemyId}", characterId, enemyId);

            try
            {
                // Pobierz dane postaci z bazy danych
                var character = await _characterService.GetByIdAsync(characterId);

                if (character != null)
                {
                    _logger.LogInformation("Character found: Name={Name}, Strength={Strength}", character.Name, character.Strength);

                    // Publikuj nowe zdarzenie TakeEnemyEvent
                    await _eventBus.Publish(new TakeEnemyEvent(character, enemyId));
                    _logger.LogInformation("Published TakeEnemyEvent for Character: {Name}, EnemyId: {EnemyId}", character.Name, enemyId);
                }
                else
                {
                    _logger.LogWarning("Character with ID {CharacterId} not found.", characterId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while handling TakeCharacterEvent: CharacterId={CharacterId}, EnemyId={EnemyId}", characterId, enemyId);
                throw; // Opcjonalnie możesz ponownie rzucić wyjątek
            }
        }
    }
    
}

