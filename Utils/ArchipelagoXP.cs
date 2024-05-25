using Il2CppAssets.Scripts.Data;
using Il2CppAssets.Scripts.Data.Global;
using System;

namespace BloonsArchipelago.Utils
{
    public class ArchipelagoXP
    {
        public int Level = 1;
        public float XP = 0.0f;
        public long XPToNext;
        public long MaxLevel;
        public bool Maxed = false; //potentially not needed
        public bool Curved = false;

        public ArchipelagoXP(int Level, float XP, long XPToNext, long MaxLevel, bool Curved)
        {
            this.Level = Level;
            this.XP = XP;
            this.XPToNext = XPToNext;
            this.MaxLevel = MaxLevel;
            this.Curved = Curved;
            Maxed = Level == MaxLevel;

            if (this.Curved)
            {
                this.XPToNext = GameData._instance.rankInfo.GetXpDiffForRankFromPrev(Level);
            }
        }

        public ArchipelagoXP(long XPToNext, long MaxLevel, bool Curved)
        {
            this.XPToNext = XPToNext;
            this.MaxLevel = MaxLevel;
            this.Curved = Curved;

            if (this.Curved)
            {
                this.XPToNext = GameData._instance.rankInfo.GetXpDiffForRankFromPrev(1);
            }
        }

        public void PassXP(float XP)
        {
            this.XP += (float)Math.Round(XP);

            while (this.XP > XPToNext && Level < MaxLevel)
            {
                this.XP -= XPToNext;
                Level++;
                BloonsArchipelago.sessionHandler.CompleteCheck("Level " + Level);
                if (Curved)
                {
                    XPToNext = GameData._instance.rankInfo.GetXpDiffForRankFromPrev(Level);
                }
            }

            if (Level == MaxLevel)
            {
                Maxed = true;
            }

            BloonsArchipelago.sessionHandler.session.DataStorage["Level-" + BloonsArchipelago.sessionHandler.PlayerSlotName()] = Level;
            BloonsArchipelago.sessionHandler.session.DataStorage["XP-" + BloonsArchipelago.sessionHandler.PlayerSlotName()] = this.XP;
        }
    }
}
