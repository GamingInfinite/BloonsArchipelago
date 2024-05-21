using BTD_Mod_Helper;
using HarmonyLib;
using Il2CppAssets.Scripts.Unity.UI_New.GameOver;

namespace BloonsArchipelago.Patches
{
    [HarmonyPatch(typeof(VictoryScreen), nameof(VictoryScreen.Open))]
    internal class MapCompletePatch
    {
        [HarmonyPostfix]
        private static void Postfix(VictoryScreen __instance)
        {
            if (BloonsArchipelago.sessionHandler.currentMode == "Standard")
            {
                BloonsArchipelago.sessionHandler.currentMode = __instance.difficulty.text.Substring(__instance.difficulty.text.IndexOf(":") + 2);
            }

            string checkstring = BloonsArchipelago.sessionHandler.currentMap + "-" + BloonsArchipelago.sessionHandler.currentMode;
            ModHelper.Msg<BloonsArchipelago>("Checked: " + checkstring);
            if (BloonsArchipelago.sessionHandler.ready)
            {
                if (BloonsArchipelago.sessionHandler.currentMap == BloonsArchipelago.sessionHandler.VictoryMap)
                {
                    switch (BloonsArchipelago.sessionHandler.Difficulty)
                    {
                        case 4:
                            if (BloonsArchipelago.sessionHandler.currentMode == "Impoppable")
                            {
                                BloonsArchipelago.sessionHandler.CompleteRando();
                                __instance.difficulty.text = "You have just beaten the Randomizer! Congragulations!";
                            }
                            break;
                        case 5:
                            if (BloonsArchipelago.sessionHandler.currentMode == "Clicks")
                            {
                                BloonsArchipelago.sessionHandler.CompleteRando();
                                __instance.difficulty.text = "You have just beaten the Randomizer! Congragulations!";
                            }
                            break;
                        case 14:
                            if (BloonsArchipelago.sessionHandler.currentMode == "Clicks")
                            {
                                BloonsArchipelago.sessionHandler.CompleteRando();
                                __instance.difficulty.text = "You have just beaten the Randomizer! Congragulations!";
                            }
                            break;
                        default:
                            break;
                    }
                    return;
                }
                BloonsArchipelago.sessionHandler.CompleteCheck(checkstring);
            }
        }
    }
}
