using BTD_Mod_Helper;
using HarmonyLib;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using System.Linq;

namespace BloonsArchipelago.Patches.InMap
{
    [HarmonyPatch(typeof(InGame), nameof(InGame.RoundEnd))]
    internal class PreviousMedalChecker
    {
        [HarmonyPostfix]
        private static void Postfix(InGame __instance, int completedRound, int highestCompletedRound)
        {
            if (BloonsArchipelago.sessionHandler.ready)
            {
                if (BloonsArchipelago.sessionHandler.currentMode == "Standard")
                {
                    BloonsArchipelago.sessionHandler.currentMode = __instance.SelectedDifficulty;
                }

                ModHelper.Msg<BloonsArchipelago>(BloonsArchipelago.sessionHandler.currentMode + " " + completedRound);
                if (completedRound == 39 && new[] { "Medium", "Hard", "Impoppable", "Clicks" }.Contains(BloonsArchipelago.sessionHandler.currentMode))
                {
                    BloonsArchipelago.sessionHandler.CompleteCheck(BloonsArchipelago.sessionHandler.currentMap + "-Easy");
                }
                else if (completedRound == 59 && new[] { "Hard", "Impoppable", "Clicks" }.Contains(BloonsArchipelago.sessionHandler.currentMode))
                {
                    BloonsArchipelago.sessionHandler.CompleteCheck(BloonsArchipelago.sessionHandler.currentMap + "-Medium");
                }
                else if (completedRound == 79 && new[] { "Impoppable", "Clicks" }.Contains(BloonsArchipelago.sessionHandler.currentMode))
                {
                    BloonsArchipelago.sessionHandler.CompleteCheck(BloonsArchipelago.sessionHandler.currentMap + "-Hard");
                }
                else if (completedRound == 99 && BloonsArchipelago.sessionHandler.currentMode == "Clicks")
                {
                    BloonsArchipelago.sessionHandler.CompleteCheck(BloonsArchipelago.sessionHandler.currentMap + "-Impoppable");
                }
            }
        }
    }
}
