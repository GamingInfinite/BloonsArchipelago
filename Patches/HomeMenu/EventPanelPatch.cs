using HarmonyLib;
using Il2CppAssets.Scripts.Unity.UI_New.Main.EventPanel;

namespace BloonsArchipelago.Patches.HomeMenu
{
    [HarmonyPatch(typeof(MainMenuEventPanel), nameof(MainMenuEventPanel.Awake))]
    internal class EventPanelPatch
    {
        [HarmonyPostfix]
        private static void Postfix(MainMenuEventPanel __instance)
        {
            if (BloonsArchipelago.sessionHandler.ready)
            {
                __instance.gameObject.SetActive(false);
            }
        }
    }
}
