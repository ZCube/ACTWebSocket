using Advanced_Combat_Tracker;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ACTWebSocket_Plugin
{
    public partial class ACTWebSocketCore
    {
        #region MiniParse

        private string prevEncounterId { get; set; }
        private DateTime prevEndDateTime { get; set; }
        private bool prevEncounterActive { get; set; }
        public string pluginDirectory { get; internal set; }
        public string overlaySkinDirectory { get; set; }

        public struct ConfigStruct
        {
            public MiniParseSortType SortType;
            public string SortKey;
        }

        public ConfigStruct Config;// { get; set; }

        public static string updateStringCache = "";
        public static DateTime updateStringCacheLastUpdate;
        public static readonly TimeSpan updateStringCacheExpireInterval = new TimeSpan(0, 0, 0, 0, 500); // 500 msec

        public void Reload()
        {
            prevEncounterId = null;
            prevEndDateTime = DateTime.MinValue;
        }
        
        protected async Task Update()
        {
            if (CheckIsActReady())
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

                Broadcast("/MiniParse", SendMessageType.CombatData.ToString(), overlayAPI.CreateEncounterJsonData());
            }
        }

        public static bool CheckIsActReady()
        {
            if (ActGlobals.oFormActMain != null &&
                ActGlobals.oFormActMain.ActiveZone != null &&
                ActGlobals.oFormActMain.ActiveZone.ActiveEncounter != null &&
                EncounterData.ExportVariables != null &&
                CombatantData.ExportVariables != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        protected void Log(LogLevel level, string format, params object[] args)
        {
            //Log(level, string.Format(format, args));
        }

        #endregion
    }
}
