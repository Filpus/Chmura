using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain7
{
    public class ZapisPotworaEvent
    {
        public Guid EventId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Data { get; set; }
    }
}
