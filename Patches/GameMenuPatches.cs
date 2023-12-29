﻿using HarmonyLib;
using Il2CppAssets.Scripts.Unity.UI_New.InGame.StoreMenu;

namespace BloonsArchipelago.Patches
{
    [HarmonyPatch(typeof(TowerPurchaseButton), nameof(TowerPurchaseButton.GetLockedState))]
    internal class MonkeyLockPatch
    {
        [HarmonyPrefix]
        private static bool Prefix(TowerPurchaseButton __instance, ref TowerPurchaseLockState __result)
        {
            if (BloonsArchipelago.sessionReady)
            {
                if (!BloonsArchipelago.MonkeysUnlocked.Contains(__instance.ItemId) && !__instance.IsHero)
                {
                    __result = TowerPurchaseLockState.HasntBeenAquired;
                    return false;
                }
            }
            return true;
        }
    }
}
