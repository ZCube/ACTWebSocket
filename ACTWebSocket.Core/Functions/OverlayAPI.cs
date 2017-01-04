using Advanced_Combat_Tracker;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using ACTWebSocket_Plugin.Classes;

namespace ACTWebSocket_Plugin
{
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
                string[] arguments = data.SplitStr(" ", StringSplitOptions.RemoveEmptyEntries);
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
                    case "GetFileList":
                        if (arguments.Length < 2) return;
                        SendPrivMessage(id, ListToJSON(GetFiles(arguments[1])));
                        break;
                    case "GetDirectoryList":
                        if (arguments.Length < 2) return;
                        SendPrivMessage(id, ListToJSON(GetDirectories(arguments[1])));
                        break;
                    case "ReadFile":
                        if (arguments.Length < 2) return;
                        SendPrivMessage(id, "{returndata:\""+ReadFile(arguments[1])+"\"}");
                        break;
                    case "GetImageBase64":
                        if (arguments.Length < 2) return;
                        SendPrivMessage(id, "{returndata:\"" + GetImageBASE64(arguments[1]) + "\"}");
                        break;
                    case "GetDirectoryNoLastSlash":
                        if (arguments.Length < 2) return;
                        SendPrivMessage(id, "{returndata:\"" + GetDirectoryNoLastSlash(arguments[1]) + "\"}");
                        break;
                    case "FileExists":
                        SendPrivMessage(id, "{returndata:" + FileExists(arguments[1]) + "}");
                        break;
                    case "DirectoryExists":
                        SendPrivMessage(id, "{returndata:" + DirectoryExists(arguments[1]) + "}");
                        break;
                }
            }
        }

        public string ListToJSON(string[] s)
        {
            string sr = string.Format("[\"{0}\"]", string.Join("\",\"", s));
            sr = "{returndata:" + sr.JSONSafeString() + "}";
            return sr;
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
                        case "draggable":
                            if (data.Length < 3) return;
                            bool draggable = Convert.ToBoolean(data[2]);

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
        
        public void ChangeZoneEvent(string[] data)
        {
            currentZone = Convert.ToInt32(data[2], 16);

            SendJSON(SendMessageType.ChangeZone, $"{{\"zoneID\":\"{currentZone}\"}}");
        }

        // 해루's Request : I need realname of Player, not 'YOU'.
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

        public void Log(LogLevel level, string format, params object[] args)
        {
            Log(level, string.Format(format, args));
        }

        public void Log(LogLevel level, string text)
        {
            string sendJSONData = $"{{typeText:\"Log\", detail:{{logLevel:\"{level}\",text:\"{text.JSONSafeString()}\"}}}}";

            // TODO : Require UI Server <-> this... LogStream
        }
    }
}
