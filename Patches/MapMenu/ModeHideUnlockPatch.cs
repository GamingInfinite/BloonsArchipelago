using BTD_Mod_Helper;
using HarmonyLib;
using Il2CppAssets.Scripts.Unity.UI_New.Main.ModeSelect;

namespace BloonsArchipelago.Patches.MapMenu
{
    //Meant to be a patch for Locking/Unlocking Difficulty Modes based on randomizer difficulty
    //[HarmonyPatch(typeof(ModeButton), nameof(ModeButton.Initialise))]
    //internal class ModeHideUnlockPatch
    //{
    //    [HarmonyPostfix]
    //    private static void Postfix(ModeButton __instance)
    //    {
    //        if (BloonsArchipelago.sessionHandler.ready)
    //        {
    //            switch (BloonsArchipelago.sessionHandler.Difficulty)
    //            {
    //                case 4:

    //                    break;
    //                case 5:
    //                    break;
    //                case 15:
    //                    if (__instance.currentState != "Unlock")
    //                    {
    //                        __instance.Unlock();
    //                        ModHelper.Msg<BloonsArchipelago>("Unlocking...");
    //                    }
    //                    break;
    //            }
    //        }
    //    }
    //}
}
