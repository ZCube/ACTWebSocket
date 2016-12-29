using Advanced_Combat_Tracker;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ACTWebSocket_Plugin
{
    using System.Threading;
    using System.Threading.Tasks;
    public class FFXIV_OverlayAPI
    {
        ACTWebSocketCore core;
        Dictionary<string, CombatData> Combatants = new Dictionary<string, CombatData>();

        public List<uint> partylist = new List<uint>();
        public int partyCount = 0;

        string myID = "00000000";
        string myName = "You";
        string outD = CombatantData.DamageTypeDataOutgoingDamage;
        string outH = CombatantData.DamageTypeDataOutgoingHealing;

        private string prevEncounterId { get; set; }
        private DateTime prevEndDateTime { get; set; }
        private bool prevEncounterActive { get; set; }

        protected long currentZone = 0L;
        public FFXIV_OverlayAPI(ACTWebSocketCore core)
        {
            this.core = core;

            outD = CombatantData.DamageTypeDataOutgoingDamage;
            outH = CombatantData.DamageTypeDataOutgoingHealing;

            SetExportVariables();
            AttachACTEvent();
        }

        public void ProcPrivateMsg(string id, WebSocketSharp.Server.WebSocketSessionManager Session, string data)
        {
            if(data != ".")
            {
                switch(data)
                {
                    case "RequestLastCombat":
                        SendPrivMessage(id, CreateEncounterJsonData());
                        break;

                    // DBM?
                }
            }
        }

        // DBM?...
        public void ReadFFxivEcho(string r)
        {
            if(r.ToLower().StartsWith("dbm"))
            {
                string[] data = r.SplitStr(" ", StringSplitOptions.RemoveEmptyEntries);

                // TODO...
            }
        }

        public void SendPrivMessage(string id, string text)
        {
            foreach(var v in core.httpServer.WebSocketServices.Hosts)
            {
                v.Sessions.SendTo(text, id);
            }
        }
        
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


        #region FileIO
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public string[] GetFiles(string dir)
        {
            if (Directory.Exists(dir))
                return Directory.GetFiles(dir);
            else
                return new string[] { };
        }

        public string[] GetDirectories(string dir)
        {
            if (Directory.Exists(dir))
                return Directory.GetDirectories(dir);
            else
                return new string[] { };
        }

        public string ReadFile(string path)
        {
            if (File.Exists(path))
                return File.ReadAllText(path);
            else
                return string.Empty;
        }

        public string GetImageBASE64(string path)
        {
            if (File.Exists(path))
            {
                Image image = Image.FromFile(path);
                ImageFormat format = ImageFormat.Png;
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, format);
                    byte[] imageBytes = ms.ToArray();
                    return Convert.ToBase64String(imageBytes);
                }
            }
            else
                return string.Empty;
        }

        public string GetDirectoryNoLastSlash(string dir)
        {
            return string.Join("\\", dir.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries));
        }
        #endregion

        #region SoundPlay
        public void MP3Play(string path)
        {
            MP3 mp3 = new MP3(path);
        }

        public void callTTS(string speach)
        {
            ActGlobals.oFormActMain.TTS(speach);
        }
        #endregion

        #region ACT Events
        public void ChangeZoneEvent(string[] data)
        {
            currentZone = Convert.ToInt32(data[2], 16);

            SendJSON(SendMessageType.ChangeZone, $"{{\"zoneID\":\"{currentZone}\"}}");
        }

        // 해루's Request : I want Player real name. don't need 'YOU'
        public void DetectMyName(string[] data)
        {
            myID = data[2];
            myName = data[3];

            SendJSON(SendMessageType.SendCharName, $"{{\"charID\":\"{myID}\", \"charName\":\"{myName.JSONSafeString()}\"}}");
        }

        private void Ability(MessageType type, string[] data)
        {
            if (data.Length < 25)
            {
                InvalidLogRecive(data);
            }

            // SkillID = Convert.ToInt32(data[4], 16);

            string[] ability = new string[16];
            for (int index = 0; index < 16; ++index)
                ability[index] = data[8 + index];

            if (data.Length >= 26)
            {
                if (Combatants.ContainsKey(data[2]))
                {
                    CombatData c = Combatants[data[2]];
                    c.CurrentHP = Convert.ToInt32(data[24]);
                    c.MaxHP = Convert.ToInt32(data[25]);

                    SendJSON(SendMessageType.CombatantDataChange, $"{{\"charID\":\"{data[2]}\", \"charName\":\"{c.PlayerName.JSONSafeString()}\", \"charMaxHP\":{c.CurrentHP}, \"charCurrentHP\":{c.CurrentHP}, \"charJob\":\"{c.PlayerJob}\"}}");
                }
            }

            if (data.Length >= 35)
            {
                if (Combatants.ContainsKey(data[6]))
                {
                    CombatData c = Combatants[data[6]];
                    c.CurrentHP = Convert.ToInt32(data[33]);
                    c.MaxHP = Convert.ToInt32(data[34]);

                    SendJSON(SendMessageType.CombatantDataChange, $"{{\"charID\":\"{data[6]}\", \"charName\":\"{c.PlayerName.JSONSafeString()}\", \"charMaxHP\":{c.CurrentHP}, \"charCurrentHP\":{c.CurrentHP}, \"charJob\":\"{c.PlayerJob}\"}}");
                }
            }
        }

        private void UpdatePartyList(string[] data)
        {
            partylist = new List<uint>();
            partyCount = Convert.ToInt32(data[2]);
            for (int i = 3; i < data.Length; ++i)
            {
                uint id = Convert.ToUInt32(data[i], 16);
                if (id < 0 && id != 0xE0000000)
                {
                    partylist.Add(id);
                }
            }
        }

        public void InvalidLogRecive(string[] data)
        {
            Log(LogLevel.Error, "Invalid Log Recived : <br>" + string.Join(":", data));
        }

        public void SendCombatantList()
        {
            string jsonformat = "{{ \"playerID\" : {0}, \"playerName\" : \"{1}\", \"playerJob\" : \"{2}\", \"maxHP\" : {3}, \"maxMP\" : {4} }},";

            StringBuilder sb = new StringBuilder();
            foreach(KeyValuePair<string, CombatData> kv in Combatants)
            {
                sb.Append(
                    string.Format(jsonformat,
                    kv.Value.PlayerID,
                    kv.Value.PlayerName.JSONSafeString(),
                    kv.Value.PlayerJob,
                    kv.Value.MaxHP,
                    kv.Value.MaxMP));
            }

            string result = "[" + sb.ToString() + "]";

            SendJSON(SendMessageType.CombatantsList, $"{{ \"combatantList\" : {result.Replace(",]", "]")} }}");
        }

        private void ACTExtension(bool isImport, LogLineEventArgs e)
        {
            string[] data = e.logLine.Split('|');
            MessageType messageType = (MessageType)Convert.ToInt32(data[0]);

            switch (messageType)
            {
                case MessageType.LogLine:
                    if(Convert.ToInt32(data[2], 16) == 56)
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

        public void AttachACTEvent()
        {
            ActGlobals.oFormActMain.BeforeLogLineRead += ACTExtension;
        }

        public void DetachACTEvent()
        {
            ActGlobals.oFormActMain.BeforeLogLineRead -= ACTExtension;
        }

        private void SetExportVariables()
        {
            if (!CombatantData.ExportVariables.ContainsKey("Last10DPS"))
                CombatantData.ExportVariables.Add("Last10DPS",
                    new CombatantData.TextExportFormatter(
                        "Last10DPS",
                        "Last 10 Seconds DPS",
                        "Average DPS for last 10 seconds.",
                        (Data, ExtraFormat) =>
                        (Data.Items[outD].Items["All"].Items.ToList().Where
                            (
                                x => x.Time >= ActGlobals.oFormActMain.LastKnownTime.Subtract(new TimeSpan(0, 0, 10))
                            ).Sum
                            (
                                x => x.Damage.Number
                            ) / (Data.Duration.TotalSeconds < 10.0 ? Data.Duration.TotalSeconds : 10.0)
                        ).ToString("0.00")
                    ));

            if (!CombatantData.ExportVariables.ContainsKey("Last30DPS"))
                CombatantData.ExportVariables.Add("Last30DPS",
                    new CombatantData.TextExportFormatter(
                        "Last30DPS",
                        "Last 30 Seconds DPS",
                        "Average DPS for last 30 seconds.",
                        (Data, ExtraFormat) =>
                        (Data.Items[outD].Items["All"].Items.ToList().Where
                            (
                                x => x.Time >= ActGlobals.oFormActMain.LastKnownTime.Subtract(new TimeSpan(0, 0, 30))
                            ).Sum
                            (
                                x => x.Damage.Number
                            ) / (Data.Duration.TotalSeconds < 30.0 ? Data.Duration.TotalSeconds : 30.0)
                        ).ToString("0.00")
                    ));

            if (!CombatantData.ExportVariables.ContainsKey("Last60DPS"))
                CombatantData.ExportVariables.Add("Last60DPS",
                    new CombatantData.TextExportFormatter(
                        "Last60DPS",
                        "Last 60 Seconds DPS",
                        "Average DPS for last 60 seconds.",
                        (Data, ExtraFormat) =>
                        (Data.Items[outD].Items["All"].Items.ToList().Where
                            (
                                x => x.Time >= ActGlobals.oFormActMain.LastKnownTime.Subtract(new TimeSpan(0, 0, 60))
                            ).Sum
                            (
                                x => x.Damage.Number
                            ) / (Data.Duration.TotalSeconds < 60.0 ? Data.Duration.TotalSeconds : 60.0)
                        ).ToString("0.00")
                    ));

            if (!CombatantData.ExportVariables.ContainsKey("Last180DPS"))
                CombatantData.ExportVariables.Add("Last180DPS",
                    new CombatantData.TextExportFormatter(
                        "Last180DPS",
                        "Last 180 Seconds DPS",
                        "Average DPS for last 180 seconds.",
                        (Data, ExtraFormat) =>
                        (Data.Items[outD].Items["All"].Items.ToList().Where
                            (
                                x => x.Time >= ActGlobals.oFormActMain.LastKnownTime.Subtract(new TimeSpan(0, 0, 180))
                            ).Sum
                            (
                                x => x.Damage.Number
                            ) / (Data.Duration.TotalSeconds < 180.0 ? Data.Duration.TotalSeconds : 180.0)
                        ).ToString("0.00")
                    ));

            if (!EncounterData.ExportVariables.ContainsKey("Last10DPS"))
                EncounterData.ExportVariables.Add("Last10DPS",
                    new EncounterData.TextExportFormatter
                    (
                        "Last10DPS",
                        "Last 10 Seconds DPS",
                        "Average DPS for last 10 seconds",
                        (Data, SelectiveAllies, Extra) =>
                        (SelectiveAllies.Sum
                            (
                                x => x.Items[outD].Items["All"].Items.ToList().Where
                                (
                                    y => y.Time >= Data.EndTime.Subtract(new TimeSpan(0, 0, 10))
                                ).Sum
                                (
                                    y => y.Damage.Number
                                )
                            ) / (Data.Duration.TotalSeconds < 10.0 ? Data.Duration.TotalSeconds : 10.0)
                        ).ToString("0.00")
                    ));

            if (!EncounterData.ExportVariables.ContainsKey("Last30DPS"))
                EncounterData.ExportVariables.Add("Last30DPS",
                    new EncounterData.TextExportFormatter
                    (
                        "Last30DPS",
                        "Last 30 Seconds DPS",
                        "Average DPS for last 30 seconds",
                        (Data, SelectiveAllies, Extra) =>
                        (SelectiveAllies.Sum
                            (
                                x => x.Items[outD].Items["All"].Items.ToList().Where
                                (
                                    y => y.Time >= Data.EndTime.Subtract(new TimeSpan(0, 0, 30))
                                ).Sum
                                (
                                    y => y.Damage.Number
                                )
                            ) / (Data.Duration.TotalSeconds < 30.0 ? Data.Duration.TotalSeconds : 30.0)
                        ).ToString("0.00")
                    ));

            if (!EncounterData.ExportVariables.ContainsKey("Last60DPS"))
                EncounterData.ExportVariables.Add("Last60DPS",
                    new EncounterData.TextExportFormatter
                    (
                        "Last60DPS",
                        "Last 60 Seconds DPS",
                        "Average DPS for last 60 seconds",
                        (Data, SelectiveAllies, Extra) =>
                        (SelectiveAllies.Sum
                            (
                                x => x.Items[outD].Items["All"].Items.ToList().Where
                                (
                                    y => y.Time >= Data.EndTime.Subtract(new TimeSpan(0, 0, 60))
                                ).Sum
                                (
                                    y => y.Damage.Number
                                )
                            ) / (Data.Duration.TotalSeconds < 60.0 ? Data.Duration.TotalSeconds : 60.0)
                        ).ToString("0.00")
                    ));

            if (!EncounterData.ExportVariables.ContainsKey("Last180DPS"))
                EncounterData.ExportVariables.Add("Last180DPS",
                    new EncounterData.TextExportFormatter
                    (
                        "Last180DPS",
                        "Last 180 Seconds DPS",
                        "Average DPS for last 180 seconds",
                        (Data, SelectiveAllies, Extra) =>
                        (SelectiveAllies.Sum
                            (
                                x => x.Items[outD].Items["All"].Items.ToList().Where
                                (
                                    y => y.Time >= Data.EndTime.Subtract(new TimeSpan(0, 0, 180))
                                ).Sum
                                (
                                    y => y.Damage.Number
                                )
                            ) / (Data.Duration.TotalSeconds < 180.0 ? Data.Duration.TotalSeconds : 180.0)
                        ).ToString("0.00")
                    ));

            if (!CombatantData.ExportVariables.ContainsKey("overHeal"))
            {
                CombatantData.ExportVariables.Add
                (
                    "overHeal",
                    new CombatantData.TextExportFormatter
                    (
                        "overHeal",
                        "overHeal",
                        "overHeal",
                        (Data, ExtraFormat) =>
                        (
                            (
                                // Data.Items[outD].Items["All"].Items.ToList().Where
                                Data.Items[outH].Items.ToList().Where
                                (
                                    x => x.Key == "All"
                                ).Sum
                                (
                                    x => x.Value.Items.ToList().Where
                                    (
                                        y => y.Tags.ContainsKey("overheal")
                                    ).Sum
                                    (
                                        y => Convert.ToInt64(y.Tags["overheal"])
                                    )
                                )
                            ).ToString()
                        )
                    )
                );
            }

            if(!CombatantData.ExportVariables.ContainsKey("damageShield"))
            {
                CombatantData.ExportVariables.Add
                (
                    "damageShield",
                    new CombatantData.TextExportFormatter
                    (
                        "damageShield",
                        "damageShield",
                        "Healers DamageShield Skill Total Value",
                        (Data, ExtraFormat) =>
                        (
                            (
                                Data.Items[outH].Items.ToList().Where
                                (
                                    x => x.Key == "All"
                                ).Sum
                                (
                                    x => x.Value.Items.Where
                                    (
                                        y =>
                                        {
                                            if (y.DamageType == "DamageShield")
                                                return true;
                                            else
                                                return false;
                                        }
                                    ).Sum
                                    (
                                        y => Convert.ToInt64(y.Damage)
                                    )
                                )
                            ).ToString()
                        )
                    )
                );
            }

            if (!CombatantData.ExportVariables.ContainsKey("absorbHeal"))
            {
                CombatantData.ExportVariables.Add
                (
                    "absorbHeal",
                    new CombatantData.TextExportFormatter
                    (
                        "absorbHeal",
                        "absorbHeal",
                        "absorbHeal",
                        (Data, ExtraFormat) =>
                        (
                            (
                                Data.Items[outH].Items.ToList().Where
                                (
                                    x => x.Key == "All"
                                ).Sum
                                (
                                    x => x.Value.Items.Where
                                    (
                                        y => y.DamageType == "Absorb"
                                    ).Sum
                                    (
                                        y => Convert.ToInt64(y.Damage)
                                    )
                                )
                            ).ToString()
                        )
                    )
                );
            }
        }
        #endregion

        #region For JSON
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

                if(!FindOverheal)
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

        public void Log(LogLevel level, string format, params object[] args)
        {
            Log(level, string.Format(format, args));
        }

        public void Log(LogLevel level, string text)
        {
            string sendJSONData = $"{{typeText:\"Log\", detail:{{logLevel:\"{level}\",text:\"{text.JSONSafeString()}\"}}}}";

            // TODO : Require UI Server <-> this... LogStream
        }
        #endregion
    }

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
    
    public class MP3 : IDisposable
    {
        private string cmd, myHandle;
        private bool isOpen;
        private long ErrorNo = 0, length;

        public MP3(string path)
        {
            myHandle = DateTime.Now.ToString("yyyyMMddHHmmss");
            if (File.Exists(path))
                MP3Open(path);
            else
                return;

            Thread trd = new Thread(new ThreadStart(autoDispose));
            trd.IsBackground = true;
            trd.Start();
        }

        private void autoDispose()
        {
            Thread.Sleep((int)(length) + 1000);
            MP3Close();
            Dispose();
        }

        [DllImport("winmm.dll")]
        private static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);

        public void MP3Close()
        {
            cmd = "close " + myHandle;
            mciSendString(cmd, null, 0, IntPtr.Zero);
            isOpen = false;
        }

        public void MP3Open(string sFileName)
        {
            cmd = $"open \"{sFileName}\" type mpegvideo alias " + myHandle;
            if ((ErrorNo = mciSendString(cmd, null, 0, IntPtr.Zero)) != 0) return;

            cmd = $"set {myHandle} time format milliseconds";
            if ((ErrorNo = mciSendString(cmd, null, 0, IntPtr.Zero)) != 0) return;

            cmd = $"set {myHandle} seek exactly on";
            if ((ErrorNo = mciSendString(cmd, null, 0, IntPtr.Zero)) != 0) return;

            StringBuilder str = new StringBuilder(128);
            cmd = $"status {myHandle} length";
            if ((ErrorNo = mciSendString(cmd, str, 128, IntPtr.Zero)) != 0) return;
            length = Convert.ToInt64(str.ToString());

            isOpen = true;
        }

        public void MP3Play(bool loop)
        {
            if (isOpen)
            {
                cmd = "play " + myHandle;
                if (loop)
                    cmd += " REPEAT";
                mciSendString(cmd, null, 0, IntPtr.Zero);
            }
        }
        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리되는 상태(관리되는 개체)를 삭제합니다.
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
    
    public static class FFXIV_OverlayStaticAPI
    {
        public static string JSONSafeString(this string s)
        {
            return s.Replace("\\", "\\\\").Replace("\r", "\\r").Replace("\n", "\\n").Replace("\t", "\\t").Replace("'", "\\'").Replace("\"", "\\\"");
        }

        public static string ReplaceNaN(this string str, string replace = "---")
        {
            return str.Replace(double.NaN.ToString(), replace);
        }

        public static string[] SplitStr(this string str, string needle, StringSplitOptions option = StringSplitOptions.None)
        {
            return str.Split(new string[] { str }, option);
        }
    }
}
