using PWC.Common.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MistrzyGry.Domain.Events
{
    public class TakeCharacterEvent : Event
    {

        public int CharacterId { get; set; }
        public int EnemyId { get; set; }
        public TakeCharacterEvent(int attakingCharacterId, int enemyId)
        {
            CharacterId = attakingCharacterId;
            EnemyId = enemyId;
        }
    }
}
