namespace ACTWebSocket_Plugin
{
    public enum MiniParseSortType
    {
        None,
        StringAscending,
        StringDescending,
        NumericAscending,
        NumericDescending
    }

    public enum MessageType
    {
        LogLine = 0,
        ChangeZone = 1,
        ChangePrimaryPlayer = 2,
        AddCombatant = 3,
        RemoveCombatant = 4,
        AddBuff = 5,
        RemoveBuff = 6,
        FlyingText = 7,
        OutgoingAbility = 8,
        IncomingAbility = 10,
        PartyList = 11,
        PlayerStats = 12,
        CombatantHP = 13,
        NetworkStartsCasting = 20,
        NetworkAbility = 21,
        NetworkAOEAbility = 22,
        NetworkCancelAbility = 23,
        NetworkDoT = 24,
        NetworkDeath = 25,
        NetworkBuff = 26,
        NetworkTargetIcon = 27,
        NetworkRaidMarker = 28,
        NetworkTargetMarker = 29,
        NetworkBuffRemove = 30,
        Debug = 251,
        PacketDump = 252,
        Version = 253,
        Error = 254,
        Timer = 255,
    }

    public enum SwingTypeEnum
    {
        None = 0,
        Autoattack = 1,
        Ability = 2,
        Healing = 10,
        HoT = 11,
        Dispel = 15,
        DoT = 20,
        Buff = 21,
        Debuff = 22,
        PowerDrain = 30,
        PowerHealing = 31,
        TPDrain = 40,
        TPHeal = 41,
        Threat = 50,
    }

    public enum LogLevel : int
    {
        Trace = 0,
        Debug = 1,
        Info = 2,
        Warning = 4,
        Error = 8
    }

    public enum SendMessageType : int
    {
        ChangeZone = 1,
        SendCharName = 2,
        CombatantDataChange = 3,
        CombatantsList = 4,
        NetworkError = 90002,
    }
}
