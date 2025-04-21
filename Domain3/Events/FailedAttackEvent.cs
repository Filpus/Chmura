using GeneratorRzutow.Domain.Entities;
using PWC.Common.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorRzutow.Domain.Events
{
    public class FailedAttackEvent : Event
    {
        public Character AttackingCharacter { get; set; }
        public Enemy AttackedEnemy { get; set; }
        public FailedAttackEvent(Character attackingCharacter, Enemy attackedEnemy)
        {
            AttackingCharacter = attackingCharacter;
            AttackedEnemy = attackedEnemy;
        }
    }

}
