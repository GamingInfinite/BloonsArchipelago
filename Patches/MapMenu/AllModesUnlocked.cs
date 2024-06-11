using BTD_Mod_Helper;
using HarmonyLib;
using Il2CppAssets.Scripts.Unity.UI_New.Main.ModeSelect;
using System.Collections.Generic;

namespace BloonsArchipelago.Patches.MapMenu
{
    [HarmonyPatch(typeof(ModeButton), nameof(ModeButton.Update))]
    internal class AllModesUnlocked
    {
        [HarmonyPrefix]
        public static bool Prefix(ModeButton __instance)
        {
            if (BloonsArchipelago.sessionHandler.ready)
            {
                List<string> unlockedModes = new()
                {
                    "Standard"
                };
                if (BloonsArchipelago.sessionHandler.Difficulty >= 4)
                {
                    unlockedModes.Add("Impoppable");
                }
                if (BloonsArchipelago.sessionHandler.Difficulty >= 5)
                {
                    unlockedModes.Add("Clicks");
                }
                if (BloonsArchipelago.sessionHandler.Difficulty > 5)
                {
                    unlockedModes.Add(__instance.name);
                }

                if (unlockedModes.Contains(__instance.name))
                {
                    if (__instance.currentState != "Unlock")
                    {
                        __instance.Unlock();
                        __instance.DisplayUnlockAnimation();
                        __instance.currentState = "Unlock";
                    }
                } else
                {
                    if (__instance.currentState != "Lock")
                    {
                        __instance.Lock();
                        __instance.currentState = "Lock";
                    }
                }

                return false;
            }
            return true;
        }
    }
}
