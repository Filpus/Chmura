using BazaBroni.Domain.Entities;
using BazaBroni.Domain.Events;
using Microsoft.Extensions.Logging;
using PWC.Common.Domain.Bus;
using PWC.Infra.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazaBroni.Infrastructure.EventHandlers
{
    public class TakeWeaponHandler : IEventHandler<TakeWeaponEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly SupabaseService<Weapon> _weaponService;
        private readonly ILogger<TakeWeaponHandler> _logger;

        public TakeWeaponHandler(IEventBus eventBus, SupabaseService<Weapon> weaponService, ILogger<TakeWeaponHandler> logger)
        {
            _eventBus = eventBus;
            _weaponService = weaponService;
            _logger = logger;
        }

        public async Task Handle(TakeWeaponEvent @event)
        {
            int characterId = @event.AttackingCharacter.Id;
            int? weaponId = @event.AttackingCharacter.WeaponId;

            _logger.LogInformation("Handling TakeWeaponEvent: CharacterId={CharacterId}, WeaponId={WeaponId}", characterId, weaponId);

            try
            {
                if (weaponId.HasValue)
                {
                    var weapon = await _weaponService.GetByIdAsync(weaponId.Value);

                    if (weapon != null)
                    {
                        _logger.LogInformation("Weapon found: Name={Name}, Damage={Damage}", weapon.Name, weapon.DamageDice);

                        await _eventBus.Publish(new CalculateDmgEvent(@event.AttackingCharacter, @event.AttackedEnemy, weapon));
                        _logger.LogInformation("Published CheckWeaponEvent for Character: {Name}, Weapon: {WeaponName}", @event.AttackingCharacter.Name, weapon.Name);
                    }
                    else
                    {
                        _logger.LogWarning("Weapon with ID {WeaponId} not found. Character will attack without a weapon.", weaponId);

                        await _eventBus.Publish(new CalculateDmgEvent(@event.AttackingCharacter, @event.AttackedEnemy, null));
                        _logger.LogInformation("Published CheckWeaponEvent for Character: {Name} without a weapon.", @event.AttackingCharacter.Name);
                    }
                }
                else
                {
                    _logger.LogWarning("Character {CharacterId} has no weapon equipped. Attacking without a weapon.", characterId);

                    await _eventBus.Publish(new CalculateDmgEvent(@event.AttackingCharacter, @event.AttackedEnemy, null));
                    _logger.LogInformation("Published CheckWeaponEvent for Character: {Name} without a weapon.", @event.AttackingCharacter.Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while handling TakeWeaponEvent: CharacterId={CharacterId}, WeaponId={WeaponId}", characterId, weaponId);
                throw;
            }
        }
    }
}
