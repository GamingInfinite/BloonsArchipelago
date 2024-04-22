using BloonsArchipelago.Utils;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Scripts.Data;
using Il2CppAssets.Scripts.Data.MapSets;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.UI_New.GameOver;
using Il2CppAssets.Scripts.Unity.UI_New.Main;
using Il2CppAssets.Scripts.Unity.UI_New.Main.EventPanel;
using Il2CppAssets.Scripts.Unity.UI_New.Main.MapSelect;
using Il2CppAssets.Scripts.Unity.UI_New.Main.ModeSelect;
using Il2CppAssets.Scripts.Unity.UI_New.Main.PlayerInfo;
using Il2CppAssets.Scripts.Unity.UI_New.Popups;
using Il2CppSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
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

                string modPath = ModContent.GetInstance<BloonsArchipelago>().GetModDirectory();

                var TempAPWorlds = BloonsArchipelago.notifJSON.APWorlds;
                TempAPWorlds.TryAdd(BloonsArchipelago.apWorldID, BloonsArchipelago.previousNotifs.ToArray());
                TempAPWorlds[BloonsArchipelago.apWorldID] = BloonsArchipelago.previousNotifs.ToArray();
                ModHelper.Msg<BloonsArchipelago>(BloonsArchipelago.previousNotifs.Count());

                var NotifObject = new NotificationJSON
                {
                    APWorlds = TempAPWorlds,
                };
                var JSONString = JsonSerializer.Serialize(NotifObject);

                File.WriteAllText(Path.Combine(modPath, "Notifications.json"), JSONString);
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
                MapDetails[] mapArr;
                if (BloonsArchipelago.defaultMapList == null)
                {
                    BloonsArchipelago.defaultMapList = GameData._instance.mapSet.Maps.items;
                    mapArr = (MapDetails[])System.Array.CreateInstance(typeof(MapDetails), GameData._instance.mapSet.Maps.items.Length);
                    System.Array.Copy(GameData._instance.mapSet.Maps.items, mapArr, GameData._instance.mapSet.Maps.items.Length);
                } else
                {
                    mapArr = (MapDetails[])System.Array.CreateInstance(typeof(MapDetails), BloonsArchipelago.defaultMapList.Length);
                    System.Array.Copy(BloonsArchipelago.defaultMapList, mapArr, BloonsArchipelago.defaultMapList.Length);
                }
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
            } else
            {
                for (int i = 0; i < GameData._instance.mapSet.Maps.items.Length; i++)
                {
                    MapDetails map = GameData._instance.mapSet.Maps.items[i];
                    ModHelper.Msg<BloonsArchipelago>(map.id);
                }
            }
        }
    }

    //Adjusting Difficulties for Randomizer Difficulty
    //[HarmonyPatch(typeof(ModeButton), nameof(ModeButton.Initialise))]
    //internal class ModeButtonLock
    //{
    //    [HarmonyPostfix]
    //    private static void Postfix(ModeButton __instance)
    //    {
    //        if (BloonsArchipelago.sessionReady)
    //        {
    //            __instance.Lock();
    //            if (BloonsArchipelago.Difficulty >= 4)
    //            {
    //                if (__instance.modeType == "Standard")
    //                {
    //                    __instance.Unlock();
    //                }
    //                else if (__instance.modeType == "Impoppable")
    //                {
    //                    __instance.Unlock();
    //                }
    //            }
    //            if (BloonsArchipelago.Difficulty >= 5)
    //            {
    //                if (__instance.modeType == "Clicks")
    //                {
    //                    __instance.Unlock();
    //                }
    //            }
    //            if (BloonsArchipelago.Difficulty == 14)
    //            {
    //                __instance.Unlock();
    //            }
    //        }
    //    }
    //}

    //Tracking Current Map and Mode
    [HarmonyPatch(typeof(ModeButton), nameof(ModeButton.ButtonClicked))]
    internal class GetCurrentMode
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
                //Adding Sprite to Victory Map
                //Sprite VMapIcon = ModContent.GetSprite<BloonsArchipelago>("ArchipelagoLogo", 50);
                //GameObject VMapIconObject = UnityEngine.Object.Instantiate(__instance.mapImage.gameObject, __instance.gameObject.transform);
                //VMapIconObject.GetComponent<Image>().SetSprite(VMapIcon);
                //VMapIconObject.GetComponent<Image>().transform.localScale = new Vector3(0.3f, 0.5f, 1f);
                //VMapIconObject.transform.localPosition = new Vector3(-400, 320, 0);
                //if (__instance.mapId == BloonsArchipelago.VictoryMap)
                //{
                //    VMapIconObject.gameObject.SetActive(true);
                //}
                //else
                //{
                //    VMapIconObject.gameObject.SetActive(false);
                //}

                //Actual stuff related to the map being unlocked or not
                if (__instance.mapId == BloonsArchipelago.VictoryMap && BloonsArchipelago.MedalRequirement > BloonsArchipelago.Medals)
                {
                    __instance.isLocked = true;
                    __instance.gameObject.transform.GetChild(6).gameObject.SetActive(true);
                }
                else
                {
                    __instance.isLocked = false;
                    __instance.gameObject.transform.GetChild(6).gameObject.SetActive(false);
                }
                return false;
            }
            return true;
        }
    }

    //Track Medals
    [HarmonyPatch(typeof(MapButton), nameof(MapButton.SetMedal))]
    internal class MedalTracker
    {
        [HarmonyPrefix]
        private static bool Prefix(string mapId, string difficulty, string mode)
        {
            
            string newMode = mode;
            if (mode == "Standard")
            {
                newMode = difficulty;
            }
            if (BloonsArchipelago.sessionReady)
            {
                bool collected = BloonsArchipelago.session.Locations.AllLocationsChecked.Contains(BloonsArchipelago.session.Locations.GetLocationIdFromName("Bloons TD6", mapId + "-" + newMode));
                return collected;
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
                BloonsArchipelago.CurrentMode = __instance.difficulty.text.Substring(__instance.difficulty.text.IndexOf(":") + 2);
            }
            var checkstring = BloonsArchipelago.CurrentMap + "-" + BloonsArchipelago.CurrentMode;
            ModHelper.Msg<BloonsArchipelago>(checkstring);
            if (BloonsArchipelago.sessionReady)
            {
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
                    __instance.difficulty.text = "You have just beaten the Randomizer! Congragulations!";
                    return;
                }
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

                __instance.rankImg.transform.localScale = new Vector3(0.8f, 0.8f, 1);
                __instance.rankImg.transform.localPosition = new Vector3(460, -160, 0);

                __instance.bar.gameObject.SetActive(false);

                GameObject medalText = UnityEngine.Object.Instantiate(__instance.level.gameObject, __instance.gameObject.transform);
                medalText.transform.localPosition = new Vector3(1625, -100, 0);
                medalText.GetComponent<NK_TextMeshProUGUI>().text = BloonsArchipelago.Medals + "/" + BloonsArchipelago.MedalRequirement;
                medalText.GetComponent<NK_TextMeshProUGUI>().textWrappingMode = Il2CppTMPro.TextWrappingModes.NoWrap;

                GameObject medalImage = UnityEngine.Object.Instantiate(__instance.rankImg.gameObject, __instance.gameObject.transform);
                medalImage.transform.localPosition = new Vector3(1400, -160, 0);
                medalImage.GetComponent<Image>().SetSprite(Game.instance.CreateSpriteReference(VanillaSprites.MedalGold));

                __instance.level.text = BloonsArchipelago.XPTracker.Level.ToString();

                if (BloonsArchipelago.XPTracker.Maxed)
                {
                    __instance.xpInfo.text = "MAX";
                }
                else
                {
                    __instance.xpInfo.text = BloonsArchipelago.XPTracker.XP + "/" + BloonsArchipelago.XPTracker.XPToNext;
                }
                return false;
            }
            return true;
        }
    }

    //Modifying Popups
    [HarmonyPatch(typeof(Popup), nameof(Popup.SetTitle))]
    internal class LockedPopupTitlePatch
    {
        [HarmonyPrefix]
        private static void Prefix(Popup __instance,ref string title)
        {
            if (BloonsArchipelago.sessionReady && title == "Unlocking Maps")
            {
                title = "Victory Map";
            }
        }
    }

    [HarmonyPatch(typeof(Popup), nameof(Popup.SetBody), new System.Type[] {typeof(string)})]
    internal class LockedPopupBodyPatch
    {
        [HarmonyPrefix]
        private static bool Prefix(Popup __instance)
        {
            if (BloonsArchipelago.sessionReady && __instance.title.text == "Victory Map")
            {
                __instance.body.text = "Sorry Chief! I'm only supposed to let you in once you have " + BloonsArchipelago.MedalRequirement + " Medals!";
                return false;
            }
            return true;
        }
    }

    //Making Continues Work (Needs to track whether or not the continue was made during archipelago or not)
    [HarmonyPatch(typeof(ContinueGamePanel), nameof(ContinueGamePanel.ContinueClicked))]
    internal class ContinuePatch
    {
        [HarmonyPostfix]
        private static void Postfix(ContinueGamePanel __instance)
        {
            BloonsArchipelago.CurrentMode = __instance.saveData.modeName;
        }
    }
}
