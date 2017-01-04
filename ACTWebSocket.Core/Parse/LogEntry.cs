using System;
using ACTWebSocket_Plugin.Objects;

namespace ACTWebSocket_Plugin.Parse
{
    public class LogEntry
    {
        public Combatant Actor;
        public Combatant Target;
        public DateTime Timestamp;
        public SwingTypeEnum swingtype;
        public Abilities.DamageType DamageTypeEnum;
        public Abilities.ElementType ElementTypeEnum;
        public decimal damagePotencyMultiplier;
        public decimal healPotencyMultiplier;
        public long overhealAmount;
        public long amount;
        public uint ActorID;
        public uint TargetID;
        public int _amount;
        public int TimeInterval;
        public int Key;
        public int SkillID;
        public int Potency;
        public int BuffTimeOffset;
        public int TargetCurrentHP;
        public int TargetMaxHP;
        public int ActorCurrentHP;
        public int ActorMaxHP;
        public int ZoneID;
        public bool IsCritical;
        public bool IsParry;
        public bool IsCancelled;
        public bool IsInterrupted;
        public bool IsImport;
        public string DamageType = "";
        public string Logline;
        public string DispelledDebuff;
        private string _actor;
        private string _target;
        private string _skillname;

        public string skillname
        {
            get { return _skillname ?? ""; }
            set { _skillname = value.ToLower().Trim(); }
        }
    }
}
