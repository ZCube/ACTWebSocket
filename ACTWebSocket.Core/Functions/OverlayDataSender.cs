using Advanced_Combat_Tracker;
using ACTWebSocket_Plugin.Classes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using ACTWebSocket_Plugin.Parse;
using static ACTWebSocket_Plugin.ACTWebSocketCore;

#pragma warning disable 0168 // variable declared but not used. 

namespace ACTWebSocket_Plugin
{
    partial class FFXIV_OverlayAPI
    {
        public struct ConfigStruct
        {
            public MiniParseSortType SortType;
            public string SortKey;
        }

        public ConfigStruct Config;// { get; set; }
        
        public static JObject updateStringCache = null;
        public static DateTime updateStringCacheLastUpdate;
        public static readonly TimeSpan updateStringCacheExpireInterval = new TimeSpan(0, 0, 0, 0, 500); // 500 msec

        /// <summary>
        /// 클라이언트(들)로 메세지를 전송합니다.
        /// </summary>
        /// <param name="type">전송될 Enum 타입입니다.</param>
        /// <param name="json">전송될 JSON 입니다. JSON을 감싸는 중괄호를 포함해 작성합니다.</param>
        public void SendJSON(SendMessageType type, JToken json)
        {
            core.Broadcast("/MiniParse", type.ToString(), json);
        }

        public void OnOpen(string id, WebSocketCommunicateBehavior session)
        {
            try
            {
                // 마지막 전투 전송
                if (updateStringCache != null)
                {
                    // isActive Update
                    updateStringCache["isActive"] = ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.Active;

                    session.Send("broadcast", null, session.id, SendMessageType.CombatData.ToString(),
                        //session.Broadcast(SendMessageType.CombatData.ToString(),
                        updateStringCache
                        );
                }

                // 플레이어 아이디 전송
                if (CurrentPlayerID != 0)
                {
                    session.Send("broadcast", null, session.id, SendMessageType.SendCharName.ToString(),
                        //session.Broadcast(SendMessageType.SendCharName.ToString(),
                        JObject.FromObject(new
                        {
                            charID = CurrentPlayerID,
                            charName = CurrentPlayerName
                        }));
                }

                // 지역 전송
                if (CurrentZoneID != 0)
                {
                    session.Send("broadcast", null, session.id, SendMessageType.ChangeZone.ToString(),
                        //session.Broadcast(SendMessageType.ChangeZone.ToString(),
                        JObject.FromObject(new
                        {
                            zoneID = CurrentZoneID
                        }));
                }

                // 오브젝트 목록 전송
                if (Combatants.Count > 0)
                {
                    foreach (KeyValuePair<uint, Combatant> c in Combatants)
                    {
                        Combatant combatant = c.Value;
                        session.Send("broadcast", null, session.id, SendMessageType.AddCombatant.ToString(),
                            JObject.FromObject(new
                            {
                                id = combatant.id,
                                name = combatant.name,
                                job = combatant.jobid,
                                level = combatant.level,
                                max_hp = combatant.max_hp,
                                max_mp = combatant.max_mp,
                                owner_id = combatant.owner_id,
                            }));
                    }
                }
            }
            catch(Exception e)
            {
                // TODO : how?
            }
        }

        /// <summary>
        /// 클라이언트(들)로 오류 메세지를 전송합니다.
        /// </summary>
        /// <param name="json">전송될 message입니다.</param>
        public void SendErrorJSON(JToken json)
        {
            core.Broadcast("/MiniParse", "Error", json);
        }

        public void SendLastCombat()
        {
            core.Broadcast("/MiniParse", SendMessageType.CombatData.ToString(), CreateEncounterJsonData());
        }

        private void ACTExtension(bool isImport, LogLineEventArgs e)
        {
            string[] data = e.logLine.Split('|');
            try
            {
                MessageType messageType = (MessageType)Convert.ToInt32(data[0]);
                ParseData(data, isImport);
            }
            catch(Exception err)
            {
                // 예외처리 필요.
                // TODO : exception....
                // Log(LogLevel.Debug, err.ToString());
                //SendErrorJSON(err.ToString());
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
                        if (exportValuePair.Key.StartsWith("NAME"))
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

        public void Update()
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

                core.Broadcast("/MiniParse", SendMessageType.CombatData.ToString(), CreateEncounterJsonData());
            }
        }

        internal JObject CreateEncounterJsonData()
        {
            if (DateTime.Now - updateStringCacheLastUpdate < updateStringCacheExpireInterval)
            {
                return updateStringCache;
            }

            if (!ACTWebSocketCore.CheckIsActReady())
            {
                return new JObject();
            }

            object o0 = ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.GetCombatant("YOU");
            object o1 = null;
            if (CurrentPlayerID != 0)
            {
                o1 = ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.GetCombatant(CurrentPlayerName);
            }

            // CharName Adjust
            if (o0 != null)
            {
                ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.CharName = "YOU";
            }
            else if (o1 != null)
            {
                ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.CharName = CurrentPlayerName;
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
                SortCombatantList(combatant);
            });
            Task.WaitAll(encounterTask, combatantTask);

            JObject obj = new JObject();
            {
                JObject Encounter = new JObject();
                foreach (var pair in encounter)
                {
                    Encounter[pair.Key] = pair.Value.ReplaceNaN();
                }
                obj["Encounter"] = Encounter;
                JObject Combatant = new JObject();
                foreach (var pair in combatant)
                {
                    JObject o = new JObject();
                    foreach (var pair2 in pair.Value)
                    {
                        o[pair2.Key] = pair2.Value.ReplaceNaN();
                    }
                    if (CurrentPlayerID != 0 && pair.Key.Name == CurrentPlayerName)
                    {
                        // CharName Adjust
                        Combatant["YOU"] = o;
                        Combatant["YOU"]["name"] = "YOU";
                    }
                    else
                    {
                        Combatant[pair.Key.Name] = o;
                    }
                }
                obj["Combatant"] = Combatant;
                obj["isActive"] = ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.Active;
            }

            updateStringCache = obj;
            updateStringCacheLastUpdate = DateTime.Now;

            return obj;
        }

        public void SortCombatantList(List<KeyValuePair<CombatantData, Dictionary<string, string>>> combatant)
        {
            if (Config.SortType == MiniParseSortType.NumericAscending ||
                Config.SortType == MiniParseSortType.NumericDescending)
            {
                combatant.Sort((x, y) =>
                {
                    int result = 0;
                    if (x.Value.ContainsKey(Config.SortKey) &&
                        y.Value.ContainsKey(Config.SortKey))
                    {
                        double xValue, yValue;
                        double.TryParse(x.Value[Config.SortKey].Replace("%", ""), out xValue);
                        double.TryParse(y.Value[Config.SortKey].Replace("%", ""), out yValue);

                        result = xValue.CompareTo(yValue);

                        if (Config.SortType == MiniParseSortType.NumericDescending)
                        {
                            result *= -1;
                        }
                    }

                    return result;
                });
            }
            else if (
                Config.SortType == MiniParseSortType.StringAscending ||
                Config.SortType == MiniParseSortType.StringDescending)
            {
                combatant.Sort((x, y) =>
                {
                    int result = 0;
                    if (x.Value.ContainsKey(Config.SortKey) &&
                        y.Value.ContainsKey(Config.SortKey))
                    {
                        result = x.Value[Config.SortKey].CompareTo(y.Value[Config.SortKey]);

                        if (Config.SortType == MiniParseSortType.StringDescending)
                        {
                            result *= -1;
                        }
                    }

                    return result;
                });
            }
        }
    }
}
