using Advanced_Combat_Tracker;
using ACTWebSocket_Plugin.Classes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;

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

        public List<KeyValuePair<CombatantData, Dictionary<string, string>>>
            GetCombatantList(List<CombatantData> allies)
        {
            var combatantList = new List<KeyValuePair<CombatantData, Dictionary<string, string>>>();
            Parallel.ForEach(allies, (ally) =>
            //foreach (var ally in allies)
            {
                var valueDict = new Dictionary<string, string>();
                bool FindOverheal = false;
                foreach (var exportValuePair in CombatantData.ExportVariables)
                {
                    try
                    {
                        if (exportValuePair.Key == "NAME")
                        {
                            continue;
                        }

                        if ((exportValuePair.Key == "Last10DPS" ||
                            exportValuePair.Key == "Last30DPS" ||
                            exportValuePair.Key == "Last60DPS" ||
                            exportValuePair.Key == "Last180DPS") && !ally.Items[outD].Items.ContainsKey("All"))
                        {
                            valueDict.Add(exportValuePair.Key, "");
                            continue;
                        }

                        var value = exportValuePair.Value.GetExportString(ally, "");
                        valueDict.Add(exportValuePair.Key, value);
                    }
                    catch (Exception e)
                    {
                        SendErrorJSON(e.Message + "\n" + e.GetBaseException().ToString());
                        Log(LogLevel.Debug, "GetCombatantList: {0}: {1}: {2}", ally.Name, exportValuePair.Key, e);
                        continue;
                    }

                    if (exportValuePair.Key == "overHeal") FindOverheal = true;
                }

                if (!FindOverheal)
                {
                    valueDict.Add("overHeal", "0");
                }

                lock (combatantList)
                {
                    combatantList.Add(new KeyValuePair<CombatantData, Dictionary<string, string>>(ally, valueDict));
                }
            });
            return combatantList;
        }

        public Dictionary<string, string> GetEncounterDictionary(List<CombatantData> allies)
        {
            bool FindOverheal = false;
            var encounterDict = new Dictionary<string, string>();
            foreach (var exportValuePair in EncounterData.ExportVariables)
            {
                try
                {
                    if ((exportValuePair.Key == "Last10DPS" ||
                        exportValuePair.Key == "Last30DPS" ||
                        exportValuePair.Key == "Last60DPS" ||
                        exportValuePair.Key == "Last180DPS") && !allies.All((ally) => ally.Items[outD].Items.ContainsKey("All")))
                    {
                        encounterDict.Add(exportValuePair.Key, "");
                        continue;
                    }

                    var value = exportValuePair.Value.GetExportString(
                        ActGlobals.oFormActMain.ActiveZone.ActiveEncounter,
                        allies,
                        "");
                    encounterDict.Add(exportValuePair.Key, value);
                }
                catch (Exception e)
                {
                    SendErrorJSON(e.Message + "\n" + e.GetBaseException().ToString());
                    Log(LogLevel.Debug, "GetEncounterDictionary: {0}: {1}", exportValuePair.Key, e);
                }
                if (exportValuePair.Key == "overHeal") FindOverheal = true;
            }

            if (!FindOverheal)
            {
                encounterDict.Add("overHeal", "0");
            }
            return encounterDict;
        }

        public async Task Update()
        {
            if (ACTWebSocketCore.CheckIsActReady())
            {
                if (prevEncounterId == ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.EncId &&
                    prevEndDateTime == ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.EndTime &&
                    prevEncounterActive == ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.Active)
                {
                    return;
                }

                prevEncounterId = ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.EncId;
                prevEndDateTime = ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.EndTime;
                prevEncounterActive = ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.Active;

                core.Broadcast("/MiniParse", CreateEncounterJsonData());
            }
        }

        public string CreateEncounterJsonData()
        {

            if (DateTime.Now -
                ACTWebSocketCore.updateStringCacheLastUpdate < ACTWebSocketCore.updateStringCacheExpireInterval)
            {
                return ACTWebSocketCore.updateStringCache;
            }

            if (!ACTWebSocketCore.CheckIsActReady())
            {
                return "";
            }

            var allies = ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.GetAllies();
            Dictionary<string, string> encounter = null;
            List<KeyValuePair<CombatantData, Dictionary<string, string>>> combatant = null;

            var encounterTask = Task.Run(() =>
            {
                encounter = GetEncounterDictionary(allies);
            });
            var combatantTask = Task.Run(() =>
            {
                combatant = GetCombatantList(allies);
                core.SortCombatantList(combatant);
            });
            Task.WaitAll(encounterTask, combatantTask);

            var builder = new StringBuilder();
            builder.Append("{\"typeText\": \"encounter\", \"detail\": {");
            builder.Append("\"Encounter\": {");
            var isFirst1 = true;
            foreach (var pair in encounter)
            {
                if (isFirst1)
                {
                    isFirst1 = false;
                }
                else
                {
                    builder.Append(",");
                }
                var valueString = pair.Value.ReplaceNaN().JSONSafeString();
                builder.AppendFormat("\"{0}\":\"{1}\"", pair.Key.JSONSafeString(), valueString);
            }
            builder.Append("},");
            builder.Append("\"Combatant\": {");
            var isFirst2 = true;
            foreach (var pair in combatant)
            {
                if (isFirst2)
                {
                    isFirst2 = false;
                }
                else
                {
                    builder.Append(",");
                }
                builder.AppendFormat("\"{0}\": {{", pair.Key.Name.JSONSafeString());
                var isFirst3 = true;
                foreach (var pair2 in pair.Value)
                {
                    if (isFirst3)
                    {
                        isFirst3 = false;
                    }
                    else
                    {
                        builder.Append(",");
                    }
                    var valueString = pair2.Value.ReplaceNaN().JSONSafeString();
                    builder.AppendFormat("\"{0}\":\"{1}\"", pair2.Key.JSONSafeString(), valueString);
                }
                builder.Append("}");
            }
            builder.Append("},");
            builder.AppendFormat("\"isActive\": {0}", ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.Active ? "true" : "false");
            builder.Append("}}");


            var result = builder.ToString();
            ACTWebSocketCore.updateStringCache = result;
            ACTWebSocketCore.updateStringCacheLastUpdate = DateTime.Now;

            return result;
        }
    }
}
