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
    using Newtonsoft.Json.Linq;
    using System.Threading.Tasks;
    public partial class FFXIV_OverlayAPI
    {
        ACTWebSocketCore core;
        public List<uint> partylist = new List<uint>();
        public int partyCount = 0;

        string outD = CombatantData.DamageTypeDataOutgoingDamage;
        string outH = CombatantData.DamageTypeDataOutgoingHealing;

        private string prevEncounterId { get; set; }
        private DateTime prevEndDateTime { get; set; }
        private bool prevEncounterActive { get; set; }

        protected long currentZone = 0L;
        public FFXIV_OverlayAPI(ACTWebSocketCore core)
        {
            Config.SortKey = "encdps";
            Config.SortType = MiniParseSortType.NumericAscending;

            this.core = core;

            outD = CombatantData.DamageTypeDataOutgoingDamage;
            outH = CombatantData.DamageTypeDataOutgoingHealing;

            SetExportVariables();
            AttachACTEvent();
        }

        internal void InstallMessageHandle(ref Dictionary<string, Action<ACTWebSocketCore.WebSocketCommunicateBehavior, JObject>> handle)
        {
            handle["overlayAPI"] = (session, o) =>
            {
                String type = o["msgtype"].ToString();
                if (type == null)
                    return;
                switch (type)
                {
                    case "RequestLastCombat":
                    case "RequestLastCombatData":
                        session.Send(session.id, session.id, SendMessageType.CombatData.ToString(), CreateEncounterJsonData());
                        break;
                    case "RequestEnd":
                    case "RequestEncounterEnd":
                        ActGlobals.oFormActMain.EndCombat(false);
                        break;
                    case "GetFileList":
                        {
                            String arg = ((JToken)o).ToObject<string>();
                            session.Send(session.id, session.id, "FileList", ListToJSON(GetFiles(arg)));
                        }
                        break;
                    case "GetDirectoryList":
                        {
                            String arg = ((JToken)o).ToObject<string>();
                            session.Send(session.id, session.id, "DirectoryList", ListToJSON(GetDirectories(arg)));
                        }
                        break;
                    case "GetDirectoryNoLastSlash":
                        {
                            String arg = ((JToken)o).ToObject<string>();
                            session.Send(session.id, session.id, "GetDirectoryNoLastSlash", GetDirectoryNoLastSlash(arg));
                        }
                        break;
                    case "FileExists":
                        {
                            String arg = ((JToken)o).ToObject<string>();
                            session.Send(session.id, session.id, "FileExists", FileExists(arg));
                        }
                        break;
                    case "DirectoryExists":
                        {
                            String arg = ((JToken)o).ToObject<string>();
                            session.Send(session.id, session.id, "DirectoryExists", DirectoryExists(arg));
                        }
                        break;
                    //case "ReadFile":
                    //    {
                    //        String arg = ((JToken)o).ToObject<string>();
                    //        session.Send(session.id, session.id, "File", ReadFile(arg));
                    //    }
                    //    break;
                    //case "GetImageBase64":
                    //    {
                    //        String arg = ((JToken)o).ToObject<string>();
                    //        session.Send(session.id, session.id, "ImageBase64", GetImageBASE64(arg));
                    //    }
                    //    break;
                };
            };
        }
        
        public JObject ListToJSON(string[] s)
        {
            JObject obj = new JObject();
            foreach(string a in s)
            {
                obj.Add(a);
            }
            return obj;
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
