using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Scripts.Unity.UI_New.Main.MapSelect;
using Il2CppAssets.Scripts.Unity.UI_New.Main.ModeSelect;

namespace BloonsArchipelago.Patches.MapMenu
{
    //The only vague patch.  This is for retrieval of the current map and current mode being played.
    [HarmonyPatch(typeof(ModeButton), nameof(ModeButton.ButtonClicked))]
    internal class GetCurrentMode
    {
        [HarmonyPostfix]
        private static void Postfix(ModeButton __instance)
        {
            BloonsArchipelago.sessionHandler.currentMode = __instance.modeType;
        }
    }

    [HarmonyPatch(typeof(MapButton), nameof(MapButton.OnClick))]
    internal class GetCurrentMap
    {
        [HarmonyPostfix]
        private static void Postfix(MapButton __instance)
        {
            BloonsArchipelago.sessionHandler.currentMap = __instance.mapId;
        }
    }

    //This piece of code is for if continuing a game.  Could be used to gimick but like who is gonna do that.
    [HarmonyPatch(typeof(ContinueGamePanel), nameof(ContinueGamePanel.ContinueClicked))]
    internal class ContinuePatch
    {
        [HarmonyPostfix]
        private static void Postfix(ContinueGamePanel __instance)
        {
            BloonsArchipelago.sessionHandler.currentMode = __instance.saveData.modeName;
        }
    }
}
