using BTD_Mod_Helper;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Enums;
using HarmonyLib;
using Il2CppAssets.Scripts.Data;
using Il2CppAssets.Scripts.Data.MapSets;
using Il2CppAssets.Scripts.Unity.UI_New.GameOver;
using Il2CppAssets.Scripts.Unity.UI_New.Main;
using Il2CppAssets.Scripts.Unity.UI_New.Main.EventPanel;
using Il2CppAssets.Scripts.Unity.UI_New.Main.MapSelect;
using Il2CppAssets.Scripts.Unity.UI_New.Main.ModeSelect;
using Il2CppAssets.Scripts.Unity.UI_New.Main.PlayerInfo;
using Il2CppAssets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BloonsArchipelago.Patches
{
    //Remove As many online buttons as possible
    [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.Start))]
    internal class MainMenuPatches
    {
        [HarmonyPostfix]
        private static void Postfix(MainMenu __instance)
        {
            if (BloonsArchipelago.sessionReady)
            {
                __instance.storeBtn.gameObject.SetActive(false);
                __instance.trophyStoreBtn.gameObject.SetActive(false);
                __instance.achievementsBtn.gameObject.SetActive(false);
                __instance.coopBtn.gameObject.SetActive(false);
                __instance.trophyStoreLimitedTimeObj.SetActive(false);
            }
        }
    }

    [HarmonyPatch(typeof(MainMenuEventPanel), nameof(MainMenuEventPanel.Awake))]
    internal class EventPanelPatch
    {
        [HarmonyPostfix]
        private static void Postfix(MainMenuEventPanel __instance)
        {
            if (BloonsArchipelago.sessionReady)
            {
                __instance.gameObject.SetActive(false);
            }
        }
    }

    //Adjusting MapSet
    [HarmonyPatch(typeof(MapSelectScreen), nameof(MapSelectScreen.Open))]
    internal class MapSelectScreenPatch
    {
        [HarmonyPostfix]
        private static void Postfix(MapSelectScreen __instance)
        {
            if (BloonsArchipelago.sessionReady)
            {
                __instance.communityButton.gameObject.SetActive(false);
            }
        }

        [HarmonyPrefix]
        private static void Prefix()
        {
            if (BloonsArchipelago.sessionReady)
            {
                if (BloonsArchipelago.defaultMapList == null)
                {
                    BloonsArchipelago.defaultMapList = GameData._instance.mapSet.Maps.items;
                }
                MapDetails[] mapArr = (MapDetails[])Array.CreateInstance(typeof(MapDetails), BloonsArchipelago.defaultMapList.Length);
                Array.Copy(BloonsArchipelago.defaultMapList, mapArr, BloonsArchipelago.defaultMapList.Length);
                List<MapDetails> mapArrayList = new List<MapDetails>();
                string[] mapsInSeed = BloonsArchipelago.MapsUnlocked.ToArray();

                for (int i = 0; i < mapArr.Length; i++)
                {
                    MapDetails map = mapArr[i];
                    if (mapsInSeed.Contains(map.id) || BloonsArchipelago.VictoryMap == map.id)
                    {
                        mapArrayList.Add(map);
                    }
                }

                GameData._instance.mapSet.Maps.items = mapArrayList.ToArray();
            }
        }
    }

    //Tracking Current Map and Mode
    [HarmonyPatch(typeof(ModeButton), nameof(ModeButton.ButtonClicked))]
    internal class ModePatch
    {
        [HarmonyPostfix]
        private static void Postfix(ModeButton __instance)
        {
            BloonsArchipelago.CurrentMode = __instance.modeType;
        }
    }

    [HarmonyPatch(typeof(MapButton), nameof(MapButton.OnClick))]
    internal class GetCurrentMap
    {
        [HarmonyPostfix]
        private static void Postfix(MapButton __instance)
        {
            BloonsArchipelago.CurrentMap = __instance.mapId;
            ModHelper.Msg<BloonsArchipelago>(__instance.isLocked);
        }
    }

    //UnlockAllMaps
    [HarmonyPatch(typeof(MapButton), nameof(MapButton.RefreshLockState))]
    internal class SetLock
    {
        [HarmonyPrefix]
        private static bool Prefix(MapButton __instance)
        {
            if (BloonsArchipelago.sessionReady)
            {
                if (__instance.mapId == BloonsArchipelago.VictoryMap && BloonsArchipelago.MedalRequirement > BloonsArchipelago.Medals)
                {
                    __instance.isLocked = true;
                } else
                {
                    __instance.isLocked = false;
                }
                return false;
            }
            return true;
        }
    }

    //Map Medal Checks
    [HarmonyPatch(typeof(VictoryScreen), nameof(VictoryScreen.Open))]
    internal class VictoryCatch
    {
        [HarmonyPostfix]
        private static void Postfix(VictoryScreen __instance)
        {
            if (BloonsArchipelago.CurrentMode == "Standard")
            {
                BloonsArchipelago.CurrentMode = __instance.difficulty.text.Substring(__instance.difficulty.text.IndexOf(":")+2);
            }
            var checkstring = BloonsArchipelago.CurrentMap + "-" + BloonsArchipelago.CurrentMode;
            if (BloonsArchipelago.CurrentMap == BloonsArchipelago.VictoryMap)
            {
                switch (BloonsArchipelago.Difficulty)
                {
                    case 4:
                        if (BloonsArchipelago.CurrentMode == "Impoppable")
                        {
                            BloonsArchipelago.CompleteRando();
                        }
                        break;
                    case 5:
                        if (BloonsArchipelago.CurrentMode == "Clicks")
                        {
                            BloonsArchipelago.CompleteRando();
                        }
                        break;
                    case 14:
                        if (BloonsArchipelago.CurrentMode == "Clicks")
                        {
                            BloonsArchipelago.CompleteRando();
                        }
                        break;
                    default:
                        break;
                }
                return;
            }
            ModHelper.Msg<BloonsArchipelago>(checkstring);
            if (BloonsArchipelago.sessionReady)
            {
                BloonsArchipelago.CompleteCheck(checkstring);
            }
        }
    }

    //XP Patch
    [HarmonyPatch(typeof(PlayerInfo), nameof(PlayerInfo.UpdateDisplay))]
    internal class UpdateDisplay
    {
        [HarmonyPrefix]
        private static bool Prefix(PlayerInfo __instance)
        {
            if (BloonsArchipelago.sessionReady)
            {
                Sprite ArchipelagoActiveSprite = ModContent.GetSprite<BloonsArchipelago>("ArchipelagoLogo", 50);
                __instance.rankImg.sprite = ArchipelagoActiveSprite;

                __instance.rankImg.transform.localScale = new Vector3 (0.8f, 0.8f, 1);
                __instance.rankImg.transform.localPosition = new Vector3(460, -160, 0);

                __instance.bar.gameObject.SetActive(false);
                __instance.level.text = BloonsArchipelago.XPTracker.Level.ToString();
                if (BloonsArchipelago.XPTracker.Maxed)
                {
                    __instance.xpInfo.text = "MAX";
                }
                __instance.xpInfo.text = BloonsArchipelago.XPTracker.XP+"/"+BloonsArchipelago.XPTracker.XPToNext;
                return false;
            }
            return true;
        }
    }
}
