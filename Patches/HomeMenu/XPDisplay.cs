using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Api;

using BTD_Mod_Helper.Extensions;

using HarmonyLib;

using Il2Cpp;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.UI_New.Main.PlayerInfo;

using UnityEngine;
using UnityEngine.UI;

namespace BloonsArchipelago.Patches.HomeMenu
{
    [HarmonyPatch(typeof(PlayerInfo), nameof(PlayerInfo.UpdateDisplay))]
    internal class XPDisplay
    {
        [HarmonyPrefix]
        private static bool Prefix(PlayerInfo __instance)
        {
            if (BloonsArchipelago.sessionHandler.ready)
            {
                Sprite ArchipelagoActiveSprite = ModContent.GetSprite<BloonsArchipelago>("ArchipelagoLogo", 50);
                __instance.rankImg.sprite = ArchipelagoActiveSprite;

                __instance.rankImg.transform.localScale = new Vector3(0.8f, 0.8f, 1);
                __instance.rankImg.transform.localPosition = new Vector3(460, -160, 0);

                __instance.bar.gameObject.SetActive(false);

                GameObject medalText = Object.Instantiate(__instance.level.gameObject, __instance.gameObject.transform);
                medalText.transform.localPosition = new Vector3(1625, -100, 0);
                medalText.GetComponent<NK_TextMeshProUGUI>().text = BloonsArchipelago.sessionHandler.Medals + "/" + BloonsArchipelago.sessionHandler.MedalRequirement;
                medalText.GetComponent<NK_TextMeshProUGUI>().textWrappingMode = Il2CppTMPro.TextWrappingModes.NoWrap;

                GameObject medalImage = Object.Instantiate(__instance.rankImg.gameObject, __instance.gameObject.transform);
                medalImage.transform.localPosition = new Vector3(1400, -160, 0);
                medalImage.GetComponent<Image>().SetSprite(Game.instance.CreateSpriteReference(VanillaSprites.MedalGold));

                __instance.level.text = BloonsArchipelago.sessionHandler.XPTracker.Level.ToString();

                if (BloonsArchipelago.sessionHandler.XPTracker.Maxed)
                {
                    __instance.xpInfo.text = "MAX";
                }
                else
                {
                    __instance.xpInfo.text = BloonsArchipelago.sessionHandler.XPTracker.XP + "/" + BloonsArchipelago.sessionHandler.XPTracker.XPToNext;
                }
                return false;
            }
            return true;
        }
    }
}
