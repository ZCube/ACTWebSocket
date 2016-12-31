using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACTWebSocket_Plugin.Classes
{

    public class CombatData
    {
        public uint PlayerID { get; set; }
        public uint PlayerJob { get; set; }
        public uint Level { get; set; }
        public uint OwnerID { get; set; }
        public string PlayerName { get; set; }
        public long MaxHP { get; set; }
        public long CurrentHP { get; set; }
        public long MaxMP { get; set; }
        public long CurrentMP { get; set; }
        public bool IsPet { get; set; }
    }
}
