using BazaBroni.Domain.Entities;
using PWC.Common.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazaBroni.Domain.Events
{
    public class TakeWeaponEvent : Event
    {
        public Character AttackingCharacter { get; set; }
        public Enemy AttackedEnemy { get; set; }
        public TakeWeaponEvent(Character attackingCharacter, Enemy attackedEnemy)
        {
            AttackingCharacter = attackingCharacter;
            AttackedEnemy = attackedEnemy;
        }
    }
}
