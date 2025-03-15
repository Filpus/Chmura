using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain4
{
    public class ZapisPostaciEvent
    {
        public Guid EventId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Data { get; set; }
    }
}
