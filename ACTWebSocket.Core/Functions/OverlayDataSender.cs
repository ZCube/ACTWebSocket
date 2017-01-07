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
        /// <summary>
        /// 클라이언트(들)로 메세지를 전송합니다.
        /// </summary>
        /// <param name="type">전송될 Enum 타입입니다.</param>
        /// <param name="json">전송될 JSON 입니다. JSON을 감싸는 중괄호를 포함해 작성합니다.</param>
        public void SendJSON(SendMessageType type, string json)
        {
            string sendjson = $"{{\"type\":\"{type}\", \"detail\":{json}}}";
            core.Broadcast("/MiniParse", sendjson);
        }

        public void SendPrivMessage(string id, string text)
        {
            foreach (var v in core.httpServer.WebSocketServices.Hosts)
            {
                v.Sessions.SendTo(text, id);
            }
        }

        /// <summary>
        /// 클라이언트(들)로 오류 메세지를 전송합니다.
        /// </summary>
        /// <param name="json">전송될 message입니다.</param>
        public void SendErrorJSON(string json)
        {
            SendJSON(SendMessageType.NetworkError, $"{{\"message\":\"{json.JSONSafeString()}\"}}");
        }

        public void SendLastCombat()
        {
            core.Broadcast("/MiniParse", CreateEncounterJsonData());
        }

        private void ACTExtension(bool isImport, LogLineEventArgs e)
        {
            string[] data = e.logLine.Split('|');
            MessageType messageType = (MessageType)Convert.ToInt32(data[0]);
            ParseData(data, isImport);
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
            builder.Append("{\"type\": \"encounter\", \"detail\": {");
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
