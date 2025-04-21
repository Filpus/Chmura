using KalkulatorObrazen.Domain.Entities;
using PWC.Common.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalkulatorObrazen.Domain.Events
{
    public class CalculateDmgEvent : Event
    {
        public Character AttackingCharacter { get; set; }
        public Enemy AttackedEnemy { get; set; }
        public Weapon Weapon { get; set; }
        public CalculateDmgEvent(Character attackingCharacter, Enemy attackedEnemy, Weapon weapon)
        {
            AttackingCharacter = attackingCharacter;
            AttackedEnemy = attackedEnemy;
            Weapon = weapon;
        }
    }
}
