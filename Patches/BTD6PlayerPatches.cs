using HarmonyLib;
using Il2CppAssets.Scripts.Unity.Player;

namespace BloonsArchipelago.Patches
{
    [HarmonyPatch(typeof(Btd6Player), nameof(Btd6Player.GainPlayerXP))]
    internal class XPTrack
    {
        [HarmonyPrefix]
        private static void Prefix(float amount)
        {
            if (BloonsArchipelago.sessionReady)
            {
                BloonsArchipelago.XPTracker.PassXP(amount);
            }
        }
    }

    [HarmonyPatch(typeof(Btd6Player), nameof(Btd6Player.CanUnlockMap))]
    class Btd6Player_CanUnlockMap
    {
        public static bool Prefix(string map)
        {
            if (map == BloonsArchipelago.VictoryMap) return false;
            return true;
        }
    }
}
