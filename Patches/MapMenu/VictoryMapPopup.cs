using HarmonyLib;
using Il2CppAssets.Scripts.Unity.UI_New.Popups;

namespace BloonsArchipelago.Patches.MapMenu
{
    [HarmonyPatch(typeof(Popup), nameof(Popup.SetTitle))]
    internal class VictoryMapPopupTitle
    {
        [HarmonyPrefix]
        private static void Prefix(Popup __instance, ref string title)
        {
            if (BloonsArchipelago.sessionHandler.ready && title == "Unlocking Maps")
            {
                title = "Victory Map";
            }
        }
    }

    [HarmonyPatch(typeof(Popup), nameof(Popup.SetBody), new[] { typeof(string) })]
    internal class VictoryMapPopupBody
    {
        [HarmonyPrefix]
        private static bool Prefix(Popup __instance)
        {
            if (BloonsArchipelago.sessionHandler.ready && __instance.title.text == "Victory Map")
            {
                __instance.body.text = "Sorry Chief! I'm only supposed to let you in once you have " + BloonsArchipelago.sessionHandler.MedalRequirement + " Medals!";
                return false;
            }
            return true;
        }
    }
}
