using BazaPotworow.Domain.Entities;
using PWC.Common.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazaPotworow.Domain.Events
{
    public class TakeEnemyEvent : Event
    {
        public Character AttackingCharacter { get; set; }
        public int EnemyId { get; set; }
        public TakeEnemyEvent(Character attackingCharacter, int enemyId)
        {
            AttackingCharacter = attackingCharacter;
            EnemyId = enemyId;
        }
    }
}
