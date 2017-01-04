using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACTWebSocket_Plugin.Objects
{
    public class Abilities
    {
        public enum DamageType
        {
            Unknown = 0,
            Slashing = 1,
            Piercing = 2,
            Blunt = 3,
            Magic = 5,
            Darkness = 6,
            Physical = 7,
            LimitBreak = 8,
        }

        public enum ElementType
        {
            Unknown,
            Fire,
            Ice,
            Air,
            Earth,
            Lightning,
            Water,
            Unaspected,
        }
    }
}
