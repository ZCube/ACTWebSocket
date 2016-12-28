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
        Dictionary<string, CombatData> Combatants = new Dictionary<string, CombatData>();

        public List<uint> partylist = new List<uint>();
        public int partyCount = 0;

        string myID = "00000000";
        string myName = "You";

        string outD = CombatantData.DamageTypeDataOutgoingDamage;
        string outH = CombatantData.DamageTypeDataOutgoingHealing;

        ACTWebSocketCore core;

        protected long currentZone = 0L;
        public FFXIV_OverlayAPI(ACTWebSocketCore core)
        {
            this.core = core;

            outD = CombatantData.DamageTypeDataOutgoingDamage;
            outH = CombatantData.DamageTypeDataOutgoingHealing;

            SetExportVariables();
            AttachACTEvent();
        }

        public void SendJSON(SendMessageType type, string json)
        {
            string sendjson = $"{{typeText:\"updateValue\", detail:{{msgType:\"{type}\", data:{json}}}}}";

            core.Broadcast("/MiniParse", sendjson);
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

            SendJSON(SendMessageType.ChangeZone, $"{{zoneID:\"{currentZone}\"}}");
        }

        // 해루's Request : I want Player real name. don't need 'YOU'
        public void DetectMyName(string[] data)
        {
            myID = data[2];
            myName = data[3];

            SendJSON(SendMessageType.SendCharName, $"{{charID:\"{myID}\", charName:\"{myName.JSONSafeString()}\"}}");
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

                    SendJSON(SendMessageType.CombatantDataChange, $"{{charID:\"{data[2]}\", charName:\"{c.PlayerName.JSONSafeString()}\", charMaxHP:{c.CurrentHP}, charCurrentHP:{c.CurrentHP}, charJob:\"{c.PlayerJob}\"}}");
                }
            }

            if (data.Length >= 35)
            {
                if (Combatants.ContainsKey(data[6]))
                {
                    CombatData c = Combatants[data[6]];
                    c.CurrentHP = Convert.ToInt32(data[33]);
                    c.MaxHP = Convert.ToInt32(data[34]);

                    SendJSON(SendMessageType.CombatantDataChange, $"{{charID:\"{data[6]}\", charName:\"{c.PlayerName.JSONSafeString()}\", charMaxHP:{c.CurrentHP}, charCurrentHP:{c.CurrentHP}, charJob:\"{c.PlayerJob}\"}}");
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

        }

        private void AttachACTEvent()
        {
            ActGlobals.oFormActMain.BeforeLogLineRead += (o, e) =>
            {
                string[] data = e.logLine.Split('|');
                MessageType messageType = (MessageType)Convert.ToInt32(data[0]);

                switch (messageType)
                {
                    case MessageType.LogLine:
                        DetectMyName(data);
                        break;
                    case MessageType.ChangeZone:
                        ChangeZoneEvent(data);
                        break;
                    case MessageType.ChangePrimaryPlayer:
                        DetectMyName(data);
                        break;
                    case MessageType.AddCombatant:
                        if(!Combatants.ContainsKey(data[2]))
                        {
                            CombatData cd = new CombatData();
                            cd.PlayerID = Convert.ToUInt32(data[2], 16);
                            cd.PlayerJob = Convert.ToUInt32(data[4], 16);
                            cd.PlayerName = data[3];
                            cd.MaxHP = cd.CurrentHP = Convert.ToInt64(data[5], 16);
                            cd.MaxMP = cd.CurrentMP = Convert.ToInt64(data[6], 16);
                            if(data[7] != "0")
                            {
                                cd.IsPet = true;
                                cd.OwnerID = Convert.ToUInt32(data[7]);
                            }
                            Combatants.Add(data[2], cd);
                        }
                        break;
                    case MessageType.RemoveCombatant:
                        if(Combatants.ContainsKey(data[2]))
                        {
                            Combatants.Remove(data[2]);
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
            };
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

            if (!CombatantData.ExportVariables.ContainsKey("overHeal"))
                CombatantData.ExportVariables.Add("overHeal",
                    new CombatantData.TextExportFormatter(
                        "overHeal",
                        "overHeal",
                        "overHeal",
                        (Data, Extra) =>
                        (
                            Data.Items[outH].Items["All"].Items.ToList().Sum
                            (
                                x => Convert.ToInt64(x.Tags["overheal"])
                            ).ToString()
                        )
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

            if (!EncounterData.ExportVariables.ContainsKey("overHeal"))
                EncounterData.ExportVariables.Add("overHeal",
                    new EncounterData.TextExportFormatter
                    (
                        "overHeal",
                        "overHeal",
                        "overHeal",
                        (Data, SelectiveAllies, Extra) =>
                        (SelectiveAllies.Sum
                            (
                                x => x.Items[outH].Items["All"].Items.ToList().Sum
                                (
                                    y => Convert.ToInt64(y.Tags["overheal"])
                                )
                            )
                        ).ToString()
                    ));
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
                        else if(exportValuePair.Key == "overHeal" && !ally.Items[outH].Items.ContainsKey("All"))
                        {
                            valueDict.Add(exportValuePair.Key, "");
                            continue;
                        }

                        var value = exportValuePair.Value.GetExportString(ally, "");
                        valueDict.Add(exportValuePair.Key, value);
                    }
                    catch (Exception e)
                    {
                        Log(LogLevel.Debug, "GetCombatantList: {0}: {1}: {2}", ally.Name, exportValuePair.Key, e);
                        continue;
                    }
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

            var encounterDict = new Dictionary<string, string>();
            //Parallel.ForEach(EncounterData.ExportVariables, (exportValuePair) =>
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
                    else if(exportValuePair.Key == "overHeal" && 
                        !allies.All((ally) => ally.Items[outH].Items.ContainsKey("All")))
                    {
                        encounterDict.Add(exportValuePair.Key, "");
                        continue;
                    }

                    var value = exportValuePair.Value.GetExportString(
                        ActGlobals.oFormActMain.ActiveZone.ActiveEncounter,
                        allies,
                        "");
                    //lock (encounterDict)
                    //{
                    encounterDict.Add(exportValuePair.Key, value);
                    //}
                }
                catch (Exception e)
                {
                    Log(LogLevel.Debug, "GetEncounterDictionary: {0}: {1}", exportValuePair.Key, e);
                }
            }
            //);
            return encounterDict;
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
    }
}
