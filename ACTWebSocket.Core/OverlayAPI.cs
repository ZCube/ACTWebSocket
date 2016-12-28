using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System;
using System.Runtime.InteropServices;
using System.Text;
using Advanced_Combat_Tracker;
using System.Linq;

namespace ACTWebSocket.Core
{
    using System.Threading;
    public class FFXIV_OverlayAPI
    {
        Dictionary<string, string> Combatants = new Dictionary<string, string>();

        protected long currentZone = 0L;
        public FFXIV_OverlayAPI()
        {
            if (!CombatantData.ExportVariables.ContainsKey("Last10DPS"))
                CombatantData.ExportVariables.Add("Last10DPS",
                    new CombatantData.TextExportFormatter(
                        "Last10DPS",
                        "Last 10 Seconds DPS",
                        "Average DPS for last 10 seconds.",
                        (Data, ExtraFormat) =>
                        (Data.Items[CombatantData.DamageTypeDataOutgoingDamage].Items["All"].Items.ToList().Where
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
                        (Data.Items[CombatantData.DamageTypeDataOutgoingDamage].Items["All"].Items.ToList().Where
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
                        (Data.Items[CombatantData.DamageTypeDataOutgoingDamage].Items["All"].Items.ToList().Where
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
                        (Data.Items[CombatantData.DamageTypeDataOutgoingDamage].Items["All"].Items.ToList().Where
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
                            Data.Items[CombatantData.DamageTypeDataOutgoingHealing].Items["All"].Items.ToList().Sum
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
                                x => x.Items[CombatantData.DamageTypeDataOutgoingDamage].Items["All"].Items.ToList().Where
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
                                x => x.Items[CombatantData.DamageTypeDataOutgoingDamage].Items["All"].Items.ToList().Where
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
                                x => x.Items[CombatantData.DamageTypeDataOutgoingDamage].Items["All"].Items.ToList().Where
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
                                x => x.Items[CombatantData.DamageTypeDataOutgoingDamage].Items["All"].Items.ToList().Where
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
                                x => x.Items[CombatantData.DamageTypeDataOutgoingHealing].Items["All"].Items.ToList().Sum
                                (
                                    y => Convert.ToInt64(y.Tags["overheal"])
                                )
                            )
                        ).ToString()
                    ));

            ActGlobals.oFormActMain.BeforeLogLineRead += (o, e) =>
            {
                string[] data = e.logLine.Split('|');
                MessageType messageType = (MessageType)Convert.ToInt32(data[0]);

                switch(messageType)
                {
                    case MessageType.LogLine:

                        break;
                    case MessageType.ChangeZone:
                        currentZone = Convert.ToInt32(data[2], 16);
                        break;
                    case MessageType.ChangePrimaryPlayer:
                        break;
                    case MessageType.AddCombatant:
                        break;
                    case MessageType.RemoveCombatant:
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

        private void Ability(MessageType type, string[] data)
        {
            LogEntry entry = new LogEntry();
            if (data.Length < 25)
            {
                InvalidLogRecive(data);
            }

            entry.SkillID = Convert.ToInt32(data[4], 16);
            entry.ActorID = Convert.ToUInt32(data[2], 16);
            entry.TargetID = Convert.ToUInt32(data[6], 16);

            string[] ability = new string[16];
            for (int index = 0; index < 16; ++index)
                ability[index] = data[8 + index];

            if (data.Length >= 26)
            {
                entry.TargetCurrentHP = Convert.ToInt32(data[24]);
                entry.TargetMaxHP = Convert.ToInt32(data[25]);
            }

            if (data.Length >= 35)
            {
                entry.ActorCurrentHP = Convert.ToInt32(data[33]);
                entry.ActorMaxHP = Convert.ToInt32(data[34]);
            }
        }

        public void InvalidLogRecive(string[] data)
        {

        }

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

        public void MP3Play(string path)
        {
            MP3 mp3 = new MP3(path);
        }

        public void callTTS(string speach)
        {
            Advanced_Combat_Tracker.ActGlobals.oFormActMain.TTS(speach);
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
    }

    public class LogEntry
    {
        public int SkillID { get; set; }
        public uint ActorID { get; set; }
        public uint TargetID { get; set; }
        public int ActorMaxHP { get; set; }
        public int ActorCurrentHP { get; set; }
        public int TargetMaxHP { get; set; }
        public int TargetCurrentHP { get; set; }
    }

    public static class OverlayStaticAPI
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
}
