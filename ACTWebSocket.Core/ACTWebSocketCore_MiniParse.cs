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

                Broadcast("/MiniParse", CreateJsonData());
            }
        }

        private string CreateEventDispatcherScript()
        {
            return "var ActXiv = " + CreateJsonData() + ";\n" +
                   "document.dispatchEvent(new CustomEvent('onOverlayDataUpdate', { detail: ActXiv }));";
        }

        internal string CreateJsonData()
        {
            if (DateTime.Now - updateStringCacheLastUpdate < updateStringCacheExpireInterval)
            {
                return updateStringCache;
            }

            if (!CheckIsActReady())
            {
                return "";
            }

            var allies = ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.GetAllies();
            Dictionary<string, string> encounter = null;
            List<KeyValuePair<CombatantData, Dictionary<string, string>>> combatant = null;

            var encounterTask = Task.Run(() =>
            {
                encounter = overlayAPI.GetEncounterDictionary(allies);
            });
            var combatantTask = Task.Run(() =>
            {
                combatant = overlayAPI.GetCombatantList(allies);
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
                obj["Encounder"] = Encounter;
                JObject Combatant = new JObject();
                foreach (var pair in combatant)
                {
                    JObject o = new JObject();
                    foreach (var pair2 in pair.Value)
                    {
                        o[pair2.Key] = pair2.Value.ReplaceNaN();
                    }
                    Combatant[pair.Key.Name] = o;
                }
                obj["Combatant"] = Combatant;
                obj["isActive"] = ActGlobals.oFormActMain.ActiveZone.ActiveEncounter.Active;
            }

            var result = obj.ToString();
            updateStringCache = result;
            updateStringCacheLastUpdate = DateTime.Now;

            return result;
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
