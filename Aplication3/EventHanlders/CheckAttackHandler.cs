using GeneratorRzutow.Domain.Entities;
using GeneratorRzutow.Domain.Events;
using Microsoft.Extensions.Logging;
using PWC.Common.Domain.Bus;
using PWC.Infra.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorRzutow.Infrastructure.EventHanlders
{
    public class CheckAttackHandler : IEventHandler<CheckAttackEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly ILogger<CheckAttackHandler> _logger;

        public CheckAttackHandler(IEventBus eventBus, ILogger<CheckAttackHandler> logger)
        {
            _eventBus = eventBus;
            _logger = logger;
        }

        public async Task Handle(CheckAttackEvent @event)
        {
            int characterId = @event.AttackingCharacter.Id;
            int enemyId = @event.AttackedEnemy.Id;

            _logger.LogInformation("Handling CheckAttackEvent: CharacterId={CharacterId}, EnemyId={EnemyId}", characterId, enemyId);

            try
            {
                var enemy = @event.AttackedEnemy;
                var character = @event.AttackingCharacter;

                _logger.LogInformation("Enemy details: Name={Name}, AC={AC}", enemy.Name, enemy.AC);

                // Test ataku  
                Random random = new Random();
                int attackRoll = character.Strength + random.Next(1, 21);

                if (attackRoll > enemy.AC)
                {
                    _logger.LogInformation("Attack successful: AttackRoll={AttackRoll}, EnemyAC={EnemyAC}", attackRoll, enemy.AC);
                    await _eventBus.Publish(new TakeWeaponEvent(character, enemy));
                    _logger.LogInformation("Published TakeWeaponEvent for Character: {Name}, Enemy: {EnemyName}", character.Name, enemy.Name);
                }
                else
                {
                    _logger.LogInformation("Attack failed: AttackRoll={AttackRoll}, EnemyAC={EnemyAC}", attackRoll, enemy.AC);
                    await _eventBus.Publish(new FailedAttackEvent(character, enemy));
                    _logger.LogInformation("Published FailedAttackEvent for Character: {Name}, Enemy: {EnemyName}", character.Name, enemy.Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while handling CheckAttackEvent: CharacterId={CharacterId}, EnemyId={EnemyId}", characterId, enemyId);
                throw;
            }
        }
    }
}

