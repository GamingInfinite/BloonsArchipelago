using System;

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
            this.XP += (float)Math.Round(XP);
            
            while (this.XP > XPToNext && Level < MaxLevel)
            {
                this.XP -= XPToNext;
                Level++;
                BloonsArchipelago.CompleteCheck("Level " + Level);
            }

            if (Level == MaxLevel)
            {
                Maxed = true;
            }

            BloonsArchipelago.session.DataStorage["Level"] = Level;
            BloonsArchipelago.session.DataStorage["XP"] = this.XP;
        }
    }
}
