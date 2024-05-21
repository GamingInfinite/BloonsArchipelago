
using HarmonyLib;
using Il2CppAssets.Scripts.Data;
using Il2CppAssets.Scripts.Unity.UI_New.Main.MapSelect;

namespace BloonsArchipelago.Patches.MapMenu
{
    [HarmonyPatch(typeof(MapSelectScreen), nameof(MapSelectScreen.Open))]
    internal class ReplaceMapList
    {
        [HarmonyPrefix]
        private static void Prefix()
        {
            if (BloonsArchipelago.sessionHandler.ready)
            {
                GameData._instance.mapSet.Maps.items = BloonsArchipelago.sessionHandler.GetMapDetails();
            }
        }
    }
}
