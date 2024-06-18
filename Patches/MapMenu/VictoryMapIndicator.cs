using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2CppAssets.Scripts.Unity.UI_New.Main.MapSelect;
using Il2CppAssets.Scripts.Unity.UI_New.Transitions;
using UnityEngine;
using UnityEngine.UI;

namespace BloonsArchipelago.Patches.MapMenu
{
    [HarmonyPatch(typeof(MapButton), nameof(MapButton.Init))]
    internal class VictoryMapIndicator
    {
        [HarmonyPrefix]
        private static void Prefix(MapButton __instance, string mapId)
        {
            if (BloonsArchipelago.sessionHandler.ready)
            {
                if (mapId == BloonsArchipelago.sessionHandler.VictoryMap)
                {
                    BloonsArchipelago.vMapIndicator = Object.Instantiate(__instance.continueIcon, __instance.transform.parent);
                    BloonsArchipelago.vMapIndicator.GetComponent<Image>().sprite = ModContent.GetSprite<BloonsArchipelago>("ArchipelagoLogo", 50);
                    BloonsArchipelago.vMapIndicator.SetActive(true);
                    Vector3 oldPos = BloonsArchipelago.vMapIndicator.transform.localPosition;
                    oldPos.x += 800;
                    oldPos.y -= 200;
                    BloonsArchipelago.vMapIndicator.transform.localPosition = oldPos;
                }
            }
        }
    }

    [HarmonyPatch(typeof(MapSelectTransition), nameof(MapSelectTransition.NextPage))]
    internal class ClearIndicator
    {
        [HarmonyPrefix]
        private static void Prefix()
        {
            BloonsArchipelago.vMapIndicator.Destroy();
        }
    }

    [HarmonyPatch(typeof(MapSelectTransition), nameof(MapSelectTransition.PrevPage))]
    internal class ClearIndicator2
    {
        [HarmonyPrefix]
        private static void Prefix()
        {
            BloonsArchipelago.vMapIndicator.Destroy();
        }
    }

    [HarmonyPatch(typeof(MapSelectTransition), nameof(MapSelectTransition.Close))]
    internal class ClearIndicator3
    {
        [HarmonyPrefix]
        private static void Prefix()
        {
            BloonsArchipelago.vMapIndicator.Destroy();
        }
    }
}
