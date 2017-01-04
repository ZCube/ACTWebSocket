using Advanced_Combat_Tracker;
using System;
using System.Linq;

namespace ACTWebSocket_Plugin
{
    partial class FFXIV_OverlayAPI
    {

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
                        "Average DPS for last 10 seconds.",
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
                        "Average DPS for last 30 seconds.",
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
                        "Average DPS for last 60 seconds.",
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
                        "Average DPS for last 180 seconds.",
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
                        "Overheal",
                        "Amount of healing that made flood over 100% of health.",
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

            if (!CombatantData.ExportVariables.ContainsKey("damageShield"))
            {
                CombatantData.ExportVariables.Add
                (
                    "damageShield",
                    new CombatantData.TextExportFormatter
                    (
                        "damageShield",
                        "Damage Shield",
                        "Damage blocked by Shield skills of healer.",
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
                        "Healed by Absorbing",
                        "Amount of heal, done by absorbing.",
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
    }
}
