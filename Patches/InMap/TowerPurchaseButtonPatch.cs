using BTD_Mod_Helper;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppAssets.Scripts.Unity.UI_New.InGame.StoreMenu;

namespace BloonsArchipelago.Patches.InMap
{
    [HarmonyPatch(typeof(TowerPurchaseButton), nameof(TowerPurchaseButton.GetLockedState))]
    internal class TowerPurchaseButtonPatch
    {
        [HarmonyPrefix]
        private static bool Prefix(TowerPurchaseButton __instance, ref TowerPurchaseLockState __result)
        {
            if (BloonsArchipelago.sessionHandler.ready)
            {
                if (!BloonsArchipelago.sessionHandler.MonkeysUnlocked.Contains(__instance.towerModel.baseId) && !__instance.IsHero)
                {
                    __result = TowerPurchaseLockState.HasntBeenAquired;
                    return false;
                }
                else if (__instance.IsHero)
                {
                    return true;
                }
                else if (__instance.cost <= InGame.instance.GetCash())
                {
                    __result = TowerPurchaseLockState.Available;
                    return false;
                }
                else
                {
                    __result = TowerPurchaseLockState.NotEnoughCash;
                    return false;
                }
            }
            return true;
        }

        //[HarmonyPostfix]
        //private static void Postfix(TowerPurchaseButton __instance, TowerPurchaseLockState __result)
        //{
        //    if (__result == TowerPurchaseLockState.HasntBeenAquired)
        //    {
        //        ModHelper.Msg<BloonsArchipelago>(__instance.towerModel.name);
        //    }
        //}
    }
}
