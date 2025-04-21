using PWC.Common.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalkulatorObrazen.Domain.Events
{
    public class DmgDealtEvent : Event
    {
        public int DmgDealt { get; set; }

        public DmgDealtEvent(int dmgDealt)
        {
            DmgDealt = dmgDealt;
        }
    }
}
