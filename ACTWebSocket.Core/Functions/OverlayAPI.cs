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
    public partial class FFXIV_OverlayAPI
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
                    // Send Last Combat Data
                    case "RequestLastCombat":
                    case "RequestLastCombatData":
                        SendPrivMessage(id, CreateEncounterJsonData());
                        break;
                    // E END
                    case "RequestEnd":
                    case "RequestEncounterEnd":
                        ActGlobals.oFormActMain.EndCombat(false);
                        break;
                    // DBM?
                }
            }
        }

        // DBM?...
        public void ReadFFxivEcho(string r)
        {
            string[] data = r.SplitStr(" ", StringSplitOptions.RemoveEmptyEntries);

            // something like TShock... hmmm
            switch(data[0])
            {
                case "server":
                    if (data.Length < 2) return;
                    switch(data[1])
                    {
                        case "켜기":
                        case "on":
                            // TODO

                            break;
                        case "끄기":
                        case "off":
                            // TODO

                            break;
                        case "모두닫기":
                        case "closeall":
                            // TODO

                            break;
                        case "열기":
                        case "open":
                            if (data.Length < 3) return;

                            string url = data[2];
                            // TODO
                            break;
                        case "크기변경":
                        case "resize":
                            if (data.Length < 4) return;
                            int w = Convert.ToInt32(data[2]);
                            int h = Convert.ToInt32(data[3]);

                            break;
                        case "위치변경":
                        case "repos":
                            if (data.Length < 4) return;
                            int x = Convert.ToInt32(data[2]);
                            int y = Convert.ToInt32(data[3]);

                            break;
                        case "클릭통과":
                        case "clickthru":
                            if (data.Length < 3) return;
                            bool clickthru = Convert.ToBoolean(data[2]);

                            break;
                        case "이동가능":
                        case "dragable":
                            if (data.Length < 3) return;
                            bool dragable = Convert.ToBoolean(data[2]);

                            break;
                        case "강조가능":
                        case "focusable":
                            if (data.Length < 3) return;
                            bool focusable = Convert.ToBoolean(data[2]);

                            break;
                        case "영역선택가능":
                        case "contentselectable":
                            if (data.Length < 3) return;
                            bool contentselectable = Convert.ToBoolean(data[2]);

                            break;
                    }
                    break;
                case "dbm":
                    if (data.Length < 2) return;

                    // TODO...
                    break;
            }
        }

        public void SendPrivMessage(string id, string text)
        {
            foreach(var v in core.httpServer.WebSocketServices.Hosts)
            {
                v.Sessions.SendTo(text, id);
            }
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
            if (str.Contains(needle))
                return str.Split(new string[] { str }, option);
            else
                return new string[] { str };
        }
    }
}
