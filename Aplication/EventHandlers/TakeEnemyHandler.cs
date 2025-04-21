using Microsoft.Extensions.Logging;
using PWC.Common.Domain.Bus;
using PWC.Infra.Bus;
using BazaPotworow.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BazaPotworow.Domain.Entities;

namespace BazaPotworow.Infrastructure.EventHandlers
{
    public class TakeEnemyHandler: IEventHandler<TakeEnemyEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly SupabaseService<Enemy> _enemyService;
        private readonly ILogger<TakeEnemyHandler> _logger;


        public TakeEnemyHandler(IEventBus eventBus, SupabaseService<Enemy> enemyService, ILogger<TakeEnemyHandler> logger)
        {
            _eventBus = eventBus;
            _enemyService = enemyService;
            _logger = logger;
        }

        public async Task Handle(TakeEnemyEvent @event)
        {
            int characterId = @event.AttackingCharacter.Id;
            int enemyId = @event.EnemyId;

            _logger.LogInformation("Handling TakeEnemyEvent: CharacterId={CharacterId}, EnemyId={EnemyId}", characterId, enemyId);

            try
            {
                // Pobierz dane postaci z bazy danych
                var enemy = await _enemyService.GetByIdAsync(enemyId);

                if (enemy != null)
                {
                    _logger.LogInformation("Enemy found: Name={Name}, AC={AC}", enemy.Name, enemy.AC);

                    // Publikuj nowe zdarzenie TakeEnemyEvent
                    await _eventBus.Publish(new CheckAttackEvent(@event.AttackingCharacter, enemy));
                    _logger.LogInformation("Published CheckAttackEvent for Character: {Name}, EnemyId: {EnemyId}", @event.AttackingCharacter.Name, enemy.Name);
                }
                else
                {
                    _logger.LogWarning("Enemy with ID {enemyId} not found.", enemyId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while handling TakeEnemyEvent: CharacterId={CharacterId}, EnemyId={EnemyId}", characterId, enemyId);
                throw; // Opcjonalnie możesz ponownie rzucić wyjątek
            }
        }
    }
    
}
