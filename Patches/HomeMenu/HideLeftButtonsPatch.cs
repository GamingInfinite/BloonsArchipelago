using HarmonyLib;
using Il2CppAssets.Scripts.Unity.UI_New.Main;

namespace BloonsArchipelago.Patches.HomeMenu
{
    [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.Start))]
    internal class HideLeftButtonsPatch
    {
        [HarmonyPostfix]
        private static void Postfix(MainMenu __instance)
        {
            if (BloonsArchipelago.sessionHandler.ready)
            {
                __instance.storeBtn.gameObject.SetActive(false);
                //__instance.mrBeastEventStoreObj.SetActive(false);


                __instance.achievementsBtn.gameObject.SetActive(false);

                __instance.coopBtn.gameObject.SetActive(false);

                __instance.trophyStoreBtn.gameObject.SetActive(false);
                __instance.trophyStoreLimitedTimeObj.SetActive(false);
            }
        }
    }
}
