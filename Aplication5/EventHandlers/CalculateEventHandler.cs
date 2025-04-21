using PWC.Common.Domain.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KalkulatorObrazen.Domain.Events;
using Microsoft.Extensions.Logging;
using KalkulatorObrazen.Domain.Enums;

namespace KalkulatorObrazen.Infrastructure.EventHandlers
{
    public class CalculateEventHandler : IEventHandler<CalculateDmgEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly ILogger<CalculateEventHandler> _logger;

        public CalculateEventHandler(IEventBus eventBus, ILogger<CalculateEventHandler> logger)
        {
            _eventBus = eventBus;
            _logger = logger;
        }

        public async Task Handle(CalculateDmgEvent @event)
        {
            int characterId = @event.AttackingCharacter.Id;
            int enemyId = @event.AttackedEnemy.Id;

            _logger.LogInformation("Handling CalculateDamageEvent: CharacterId={CharacterId}, EnemyId={EnemyId}", characterId, enemyId);

            try
            {
                var character = @event.AttackingCharacter;
                var enemy = @event.AttackedEnemy;
                var weapon = @event.Weapon;

                _logger.LogInformation("Character details: Name={Name}, Strength={Strength}, WeaponId={WeaponId}", character.Name, character.Strength, character.WeaponId);
                _logger.LogInformation("Enemy details: Name={Name}, AC={AC}, Vulnerability={Vulnerability}, Resistance={Resistance}", enemy.Name, enemy.AC, enemy.Vulnerability, enemy.Resistance);
                if (weapon != null) _logger.LogInformation("Weapon details: Name={Name}, DamageDice={DamageDice}, DamageType={DamageType}", weapon.Name, weapon.DamageDice, weapon.DamageType);
                else _logger.LogInformation("No weapon used.");
                // Rzut na obrażenia
                Random random = new Random();
                int baseDamage = 1;
                if (weapon != null)
                {
                    baseDamage = RollDice(weapon.DamageDice, weapon.DamageDiceCount, random);
                }
                int totalDamage = baseDamage + character.Strength / 2 - 5;

                if (weapon != null)
                {
                    if (enemy.Vulnerability == weapon.DamageType)
                    {
                        totalDamage *= 2;
                        _logger.LogInformation("Enemy is vulnerable to {DamageType}. Damage doubled.", weapon.DamageType);
                    }
                    else if (enemy.Resistance == weapon.DamageType)
                    {
                        totalDamage /= 2;
                        _logger.LogInformation("Enemy is resistant to {DamageType}. Damage halved.", weapon.DamageType);
                    }
                }

                _logger.LogInformation("Calculated damage: {TotalDamage}", totalDamage);

                // Publikacja zdarzenia z wynikiem obrażeń
                //await _eventBus.Publish(new DmgDealtEvent(totalDamage));
                _logger.LogInformation("Published DamageDealtEvent for Character: {Name}, Enemy: {EnemyName}, Damage: {Damage}", character.Name, enemy.Name, totalDamage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while handling CalculateDamageEvent: CharacterId={CharacterId}, EnemyId={EnemyId}", characterId, enemyId);
                throw;
            }
        }

        private int RollDice(DiceType damageDice, int diceNumber, Random random)
        {
            // Przykład damageDice: "D6" (k6)  
            string diceString = damageDice.ToString();
            int diceSides = int.Parse(diceString.Substring(1));

            int total = 0;
            for (int i = 0; i < diceNumber; i++)
            {
                total += random.Next(1, diceSides + 1);
            }

            return total;
        }
    }
}
