using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Advanced_Combat_Tracker;
using ACTWebSocket_Plugin.Parse;
using Newtonsoft.Json.Linq;

namespace ACTWebSocket_Plugin
{
    partial class FFXIV_OverlayAPI
    {
        public static Dictionary<uint, Combatant> Combatants = new Dictionary<uint, Combatant>();
        public static List<uint> PartyList = new List<uint>();
        public static int CurrentZoneID = 0;
        public static uint CurrentPlayerID = 0;
        public static string CurrentPlayerName = "YOU";
        
        public void ParseData(string[] data, bool isImport)
        {
            if (data.Length < 2 || data == null)
            {
                // Data Error
                return;
            }
            else
            {
                MessageType lineType = (MessageType)Convert.ToInt32(data[0]);

                LogEntry oLogEntry = new LogEntry();
                oLogEntry.Timestamp = DateTime.Parse(data[1]);
                oLogEntry.IsImport = isImport;
                oLogEntry.ZoneID = CurrentZoneID;
                
                switch(lineType)
                {
                    case MessageType.LogLine:
                        if (data.Length < 5)
                        {
                            InvaildLogLine(data);
                            break;
                        }

                        if (Convert.ToInt32(data[2], 16) == 56)
                        {
                            ReadFFxivEcho(data[4]);
                        }
                        break;
                    case MessageType.ChangeZone:
                        if (data.Length < 3)
                        {
                            InvaildLogLine(data);
                            break;
                        }
                        CurrentZoneID = oLogEntry.ZoneID = Convert.ToInt32(data[2], 16);
                        SendJSON(SendMessageType.ChangeZone,
                            JObject.FromObject(new
                            {
                                zoneID = CurrentZoneID
                            }));
                        break;
                    case MessageType.ChangePrimaryPlayer:
                        if (data.Length < 4)
                        {
                            InvaildLogLine(data);
                            break;
                        }
                        CurrentPlayerID = Convert.ToUInt32(data[2], 16);
                        CurrentPlayerName = data[3];
                        SendJSON(SendMessageType.SendCharName,
                            JObject.FromObject(new
                            {
                                charID = CurrentPlayerID,
                                charName = CurrentPlayerName
                            }));
                        break;
                    case MessageType.AddCombatant:
                        if (data.Length < 7)
                        {
                            InvaildLogLine(data);
                            break;
                        }
                        Combatant combatant = new Combatant();
                        combatant.id = Convert.ToUInt32(data[2], 16);
                        combatant.name = data[3];
                        combatant.jobid = Convert.ToByte(data[4], 16);
                        combatant.level = Convert.ToByte(data[5], 16);
                        combatant.max_hp = Convert.ToInt32(data[6], 16);
                        combatant.max_mp = Convert.ToInt32(data[7], 16);
                        combatant.owner_id = Convert.ToUInt32(data[8], 16);

                        if (!Combatants.ContainsKey(combatant.id))
                            Combatants.Add(combatant.id, combatant);
                        SendJSON(SendMessageType.AddCombatant,
                            JObject.FromObject(new
                            {
                                id = combatant.id,
                                name = data[3],
                                job = combatant.jobid,
                                level = combatant.level,
                                max_hp = combatant.max_hp,
                                max_mp = combatant.max_mp,
                                owner_id = combatant.owner_id,
                            }));
                        break;
                    case MessageType.RemoveCombatant:
                        Combatants.Remove(Convert.ToUInt32(data[2], 16));
                        SendJSON(SendMessageType.RemoveCombatant,
                            JObject.FromObject(new
                            {
                                id = Convert.ToUInt32(data[2], 16),
                            }));
                        break;
                    case MessageType.AddBuff:
                        Combatants[Convert.ToUInt32(data[7], 16)].AddBuff(Convert.ToInt32(data[2], 16), Convert.ToInt32(data[4]));
                        break;
                    case MessageType.RemoveBuff:
                        Combatants[Convert.ToUInt32(data[7], 16)].RemoveBuff(Convert.ToInt32(data[2], 16));
                        break;
                    case MessageType.OutgoingAbility:
                        // 메모리?
                        break;
                    case MessageType.IncomingAbility:
                        // 메모리?
                        break;
                    case MessageType.NetworkDoT:
                        if (data.Length < 13)
                        {
                            InvaildLogLine(data);
                            break;
                        }

                        SendJSON(SendMessageType.DoTHoT,
                            JObject.FromObject(new
                            {
                                // Only Target Data...
                                id = Convert.ToUInt32(data[2], 16),
                                name = data[3],
                                cur_hp = Convert.ToInt32(data[7]),
                                max_hp = Convert.ToInt32(data[8]),
                                cur_mp = Convert.ToInt32(data[9]),
                                max_mp = Convert.ToInt32(data[10]),
                                cur_tp = Convert.ToInt32(data[11]),
                                max_tp = Convert.ToInt32(data[12]),
                            }));
                        break;
                    case MessageType.NetworkAbility:
                    case MessageType.NetworkAOEAbility:
                        uint target = Convert.ToUInt32(data[6], 16);
                        uint actor = Convert.ToUInt32(data[2], 16);
                        /* 
                         * 00|
                         * 01|
                         * 02|ACTORID
                         * 03|ACTORNAME
                         * 04|SKILLID
                         * 05|SKILLNAME
                         * 06|TARGETID
                         * 07|TARGETNAME
                         * --------ABILITY
                         * 08|1412 DamageAmount?
                         * 09|55
                         * 10|1D
                         * 11|25 SkillID?
                         * 12|0
                         * 13|0
                         * 14|0
                         * 15|0
                         * 16|2F070103 // flag4. str1
                         * 17|800418 // flag5 str2
                         * XXXX XXXX str1
                         * XXXX XXXX str2
                         * 2F07 0103
                         * 0080 0418
                         * 
                         * 01030080
                         * 04182F07
                         * 
                         * 03 (01 '03' 0080) 
                         * 1, 2, 3, 5, 7, 8, 9 : 어빌리티
                         * 4, 6 : 힐
                         * 10 : 디스펠
                         * 5, 6 : critical = true;
                         * 7 : block = true; 받아넘기기
                         * 8 : parry = true; 방패막기
                         * 
                         * 18|1E
                         * 19|800000
                         * 20|0
                         * 21|0
                         * 22|0
                         * 23|0
                         * --------ABILITY
                         * --------TARGETSTATUS
                         * 24|TargetCurrentHP
                         * 25|TargetMaxHP
                         * 26|TargetCurrentMP
                         * 27|TargetMaxHP
                         * 28|TargetCurrentTP
                         * 29|TargetMaxTP (1000...)
                         * 30|45.79224
                         * 31|-82.01727
                         * 32|56.48605
                         * --------TARGETSTATUS
                         * --------ACTORSTATUS
                         * 33|ActorCurrentHP
                         * 34|ActorMaxHP
                         * 35|ActorCurrentMP
                         * 36|ActorMaxMP
                         * 37|ActorCurrentTP
                         * 38|ActorMaxTP (1000...)
                         * 39|47.55971
                         * 40|-81.14174
                         * 41|54.5
                         * --------ACTORSTATUS
                        */
                        if (data.Length >= 32 && Combatants.ContainsKey(target))
                        {
                            Combatants[target].cur_hp = Convert.ToInt32(data[24]);
                            Combatants[target].max_hp = Convert.ToInt32(data[25]);

                            Combatants[target].cur_mp = Convert.ToInt32(data[26]);
                            Combatants[target].max_mp = Convert.ToInt32(data[27]);

                            Combatants[target].cur_tp = Convert.ToInt32(data[28]);
                            Combatants[target].max_tp = Convert.ToInt32(data[29]);
                            SendJSON(SendMessageType.AbilityUse,
                                JObject.FromObject(new
                                {
                                    id = target,
                                    cur_hp = Convert.ToInt32(data[24]),
                                    max_hp = Convert.ToInt32(data[25]),
                                    cur_mp = Convert.ToInt32(data[26]),
                                    max_mp = Convert.ToInt32(data[27]),
                                    cur_tp = Convert.ToInt32(data[28]),
                                    max_tp = Convert.ToInt32(data[29]),
                                }));
                        }

                        if (data.Length >= 41 &&  Combatants.ContainsKey(actor))
                        {
                            Combatants[actor].cur_hp = Convert.ToInt32(data[33]);
                            Combatants[actor].max_hp = Convert.ToInt32(data[34]);

                            Combatants[actor].cur_mp = Convert.ToInt32(data[35]);
                            Combatants[actor].max_mp = Convert.ToInt32(data[36]);

                            Combatants[actor].cur_tp = Convert.ToInt32(data[37]);
                            Combatants[actor].max_tp = Convert.ToInt32(data[38]);
                            SendJSON(SendMessageType.AbilityUse,
                                JObject.FromObject(new
                                {
                                    id = actor,
                                    cur_hp = Convert.ToInt32(data[33]),
                                    max_hp = Convert.ToInt32(data[34]),
                                    cur_mp = Convert.ToInt32(data[35]),
                                    max_mp = Convert.ToInt32(data[36]),
                                    cur_tp = Convert.ToInt32(data[37]),
                                    max_tp = Convert.ToInt32(data[38]),
                                }));
                        }
                        break;
                    case MessageType.PartyList:
                        PartyList = new List<uint>();
                        int var_1 = Convert.ToInt32(data[2]);
                        for (int idx=3; idx < data.Length; ++idx)
                        {
                            uint var_2 = Convert.ToUInt32(data[idx], 16);
                            if (var_2 > 0 && var_2 != 0xE0000000)
                                PartyList.Add(var_2);
                        }
                        break;
                    case MessageType.FlyingText:
                        break;
                    // unused
                    case MessageType.CombatantHP:
                    case MessageType.Error:
                    case MessageType.Timer:
                        break;
                }
            }
        }

        public void InvaildLogLine(string[] data)
        {

        }
    }
}
