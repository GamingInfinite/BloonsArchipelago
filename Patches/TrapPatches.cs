using BTD_Mod_Helper;
using HarmonyLib;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonsArchipelago.Patches
{
    public class TrapPatches
    {
        public bool EffectTrapActive = false;

        public void ActivateTrap(string trap)
        {
            switch (trap)
            {
                case "EffectScaleTrap":
                    EffectTrapActive = true;
                    break;
                default: break;
            }
        }

        public void DeactivateTrap(string trap)
        {
            switch (trap)
            {
                case "EffectScaleTrap":
                    EffectTrapActive = false;
                    break;
                default: break;
            }
        }
    }

    [HarmonyPatch(typeof(AccessibilitySettings), nameof(AccessibilitySettings.SetScaleForCategory))]
    internal class ZeroEffectsPatchCheck
    {
        [HarmonyPrefix]
        private static void Prefix(DisplayCategory category, float scale)
        {
            ModHelper.Msg<BloonsArchipelago>(category + " is Effect Category and Zero is " + scale);
        }
    }

    [HarmonyPatch(typeof(AccessibilitySettings), nameof(AccessibilitySettings.GetScaleForCategory))]
    internal class ZeroEffectsPatch
    {
        [HarmonyPrefix]
        private static void Prefix(AccessibilitySettings __instance)
        {
            if (BloonsArchipelago.Traps.EffectTrapActive)
            {
                __instance.SetScaleForCategory(DisplayCategory.Effect, 0);
                __instance.SetScaleForCategory(DisplayCategory.Projectile, 0);
            }
        }
    }
}
