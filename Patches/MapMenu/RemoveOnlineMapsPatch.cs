using HarmonyLib;
using Il2CppAssets.Scripts.Unity.UI_New.Main.MapSelect;

namespace BloonsArchipelago.Patches.MapMenu
{
    [HarmonyPatch(typeof(MapSelectScreen), nameof(MapSelectScreen.Open))]
    internal class RemoveOnlineMapsPatch
    {
        [HarmonyPostfix]
        private static void Postfix(MapSelectScreen __instance)
        {
            if (BloonsArchipelago.sessionHandler.ready)
            {
                __instance.communityButton.gameObject.SetActive(false);
            }
        }
    }
}
