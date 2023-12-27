using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonsArchipelago.Utils
{
    public class ArchipelagoXP
    {
        public int Level = 1;
        public float XP = 0.0f;
        public long XPToNext;
        public long MaxLevel;
        public bool Maxed = false;

        public ArchipelagoXP(int Level,  float XP, long XPToNext, long MaxLevel)
        {
            this.Level = Level;
            this.XP = XP;
            this.XPToNext = XPToNext;
            this.MaxLevel = MaxLevel;
            Maxed = Level == MaxLevel;
        }

        public ArchipelagoXP(long XPToNext, long MaxLevel) 
        {
            this.XPToNext = XPToNext;
            this.MaxLevel = MaxLevel;
        }

        public void PassXP(float XP)
        {
            if (Level == MaxLevel)
            {
                return;
            }

            this.XP += XP;
            
            if (this.XP > XPToNext)
            {
                this.XP -= XPToNext;
                Level++;
                BloonsArchipelago.CompleteCheck("Level " + Level);
                
            }
            BloonsArchipelago.session.DataStorage["Level"] = Level;
            BloonsArchipelago.session.DataStorage["XP"] = this.XP;
        }
    }
}
