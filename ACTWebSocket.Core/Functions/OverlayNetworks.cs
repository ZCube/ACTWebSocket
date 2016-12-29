using Advanced_Combat_Tracker;
using System;

namespace ACTWebSocket_Plugin
{
    partial class FFXIV_OverlayAPI
    {
        public void GetMessage(WebSocketSharp.MessageEventArgs e)
        {
            core.Broadcast("/MiniParse", $"{{\"typeText\":\"onMessage\", \"data\":\"{e.Data.JSONSafeString()}\"}}");
        }

        public void SendJSON(SendMessageType type, string json)
        {
            string sendjson = $"{{\"typeText\":\"update\", \"detail\":{{\"msgType\":\"{type}\", \"data\":{json}}}}}";

            core.Broadcast("/MiniParse", sendjson);
        }

        public void SendErrorJSON(string json)
        {
            string sendjson = $"{{\"typeText\":\"error\", \"detail\":{{\"msgType\":\"{SendMessageType.NetworkError}\", \"data\":\"{json.JSONSafeString()}\"}}}}";
            core.Broadcast("/MiniParse", sendjson);
        }

        public void SendLastCombat()
        {
            core.Broadcast("/MiniParse", CreateEncounterJsonData());
        }

        private void ACTExtension(bool isImport, LogLineEventArgs e)
        {
            string[] data = e.logLine.Split('|');
            MessageType messageType = (MessageType)Convert.ToInt32(data[0]);

            switch (messageType)
            {
                case MessageType.LogLine:
                    if (Convert.ToInt32(data[2], 16) == 56)
                    {
                        ReadFFxivEcho(data[4]);
                    }
                    break;
                case MessageType.ChangeZone:
                    ChangeZoneEvent(data);
                    break;
                case MessageType.ChangePrimaryPlayer:
                    DetectMyName(data);
                    break;
                case MessageType.AddCombatant:
                    if (!Combatants.ContainsKey(data[2]))
                    {
                        CombatData cd = new CombatData();
                        cd.PlayerID = Convert.ToUInt32(data[2], 16);
                        cd.PlayerJob = Convert.ToUInt32(data[4], 16);
                        cd.PlayerName = data[3];
                        cd.MaxHP = cd.CurrentHP = Convert.ToInt64(data[5], 16);
                        cd.MaxMP = cd.CurrentMP = Convert.ToInt64(data[6], 16);
                        if (data[8] != "0")
                        {
                            cd.IsPet = true;
                            cd.OwnerID = Convert.ToUInt32(data[8]);
                        }
                        Combatants.Add(data[2], cd);
                        SendCombatantList();
                    }
                    break;
                case MessageType.RemoveCombatant:
                    if (Combatants.ContainsKey(data[2]))
                    {
                        Combatants.Remove(data[2]);
                        SendCombatantList();
                    }
                    break;
                case MessageType.PartyList:
                    UpdatePartyList(data);
                    break;
                case MessageType.NetworkStartsCasting:
                case MessageType.NetworkCancelAbility:
                case MessageType.NetworkDoT:
                case MessageType.NetworkDeath:
                case MessageType.NetworkBuff:
                case MessageType.NetworkTargetIcon:
                case MessageType.NetworkRaidMarker:
                case MessageType.NetworkTargetMarker:
                case MessageType.NetworkBuffRemove:
                    break;
                case MessageType.NetworkAbility:
                case MessageType.NetworkAOEAbility:
                    Ability(messageType, data);
                    break;
            }
        }
    }
}
