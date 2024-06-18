using HarmonyLib;
using Il2CppAssets.Scripts.Unity.UI_New.Main.MapSelect;
using UnityEngine;

namespace BloonsArchipelago.Patches.MapMenu
{
    [HarmonyPatch(typeof(MapButton), nameof(MapButton.RefreshLockState))]
    internal class MapLocksPatch
    {
        [HarmonyPrefix]
        private static bool Prefix(MapButton __instance)
        {
            if (BloonsArchipelago.sessionHandler.ready)
            {
                if (__instance.mapId == BloonsArchipelago.sessionHandler.VictoryMap && BloonsArchipelago.sessionHandler.MedalRequirement > BloonsArchipelago.sessionHandler.Medals)
                {
                    __instance.isLocked = true;
                    __instance.gameObject.transform.GetChild(6).gameObject.SetActive(true);
                } else
                {
                    __instance.isLocked = false;
                    __instance.gameObject.transform.GetChild(6).gameObject.SetActive(false);
                }
                return false;
            }
            return true;
        }
    }
}
