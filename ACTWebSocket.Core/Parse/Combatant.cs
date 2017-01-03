using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACTWebSocket_Plugin.Parse
{
    public class Combatant
    {
        private decimal?[] outPotencyHistory = new decimal?[1000];
        public string name;
        public string nameabbrev1;
        public string nameabbrev2;
        public uint id;
        public uint owner_id;
        public byte jobid;
        public byte level;
        public int max_hp;
        public int cur_hp;
        public int max_mp;
        public int cur_mp;
        public int max_tp = 1000;
        public int cur_tp;
        private decimal outDamageCritPercent;
        private int outDamageCritHistoryCount;
        private decimal outHealPotencyMultiplier;
        private int outHealPotencyHistoryCount;
        private decimal outHealCritPercent;
        private int outHealCritHistoryCount;
        private decimal outClericHealPotencyMultiplier;
        private int outClericHealPotencyHistoryCount;
        private decimal outClericDamagePotencyMultiplier;
        private int outClericDamagePotencyHistoryCount;
        private const int potencyHistoryMax = 1000;
        private const int potencyWindow = 15;
        private int outPotencyHistoryIndex;

        public void ResetPotency()
        {
            outHealPotencyHistoryCount = 0;
            outHealPotencyMultiplier = new decimal();
            outClericHealPotencyMultiplier = new decimal();
            outClericHealPotencyHistoryCount = 0;
            outClericDamagePotencyMultiplier = new decimal();
            outClericDamagePotencyHistoryCount = 0;
            outPotencyHistoryIndex = 0;
            for (int index = 0; index < outPotencyHistory.Length; ++index)
                outPotencyHistory[index] = new decimal?();
            outDamageCritHistoryCount = 0;
            outDamageCritPercent = new decimal();
            outHealCritHistoryCount = 0;
            outHealCritPercent = new decimal();
        }

        private Dictionary<int, int> BuffList = new Dictionary<int, int>();

        public void AddBuff(int id, int time)
        {
            if (!BuffList.ContainsKey(id))
                BuffList.Add(id, time);
            else
                BuffList[id] = time;

            if (time > 0)
                Task.Run(() => { System.Threading.Thread.Sleep(time); if (BuffList.ContainsKey(id)) BuffList.Remove(id); });
        }

        public void RemoveBuff(int id)
        {
            if (BuffList.ContainsKey(id))
            {
                BuffList.Remove(id);
            }
        }
    }
}
