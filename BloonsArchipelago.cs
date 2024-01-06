using Archipelago.MultiClient.Net;
using BloonsArchipelago;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.ModOptions;
using System;
using MelonLoader;
using Archipelago.MultiClient.Net.Enums;
using System.Collections.Generic;
using Archipelago.MultiClient.Net.Models;
using Il2CppAssets.Scripts.Data.MapSets;
using BloonsArchipelago.Utils;
using Archipelago.MultiClient.Net.Packets;
using BloonsArchipelago.Patches;
using Il2CppAssets.Scripts.Unity;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using System.Linq;
using Harmony;
using System.Threading.Tasks;

[assembly: MelonInfo(typeof(BloonsArchipelago.BloonsArchipelago), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace BloonsArchipelago;

public class BloonsArchipelago : BloonsTD6Mod
{
    public static ArchipelagoSession? session;
    public static bool sessionReady = false;
    public static string CurrentMap = "";
    public static string CurrentMode = "";

    public static MapDetails[] defaultMapList;

    public static List<string> Players = new List<string>();
    public static List<string> MapsUnlocked = new List<string>();
    public static List<string> MonkeysUnlocked = new List<string>();

    public static List<string> previousNotifs = new List<string>();

    public static string VictoryMap;
    public static long MedalRequirement = 0;
    public static long Difficulty;
    public static int Medals = 0;

    public static ArchipelagoXP XPTracker;
    public static TrapPatches Traps = new TrapPatches();

    private static List<string> notifications = new List<string>();

    public override void OnApplicationStart()
    {
        ModHelper.Msg<BloonsArchipelago>("Bloons Archipelago loaded!");
    }

    private static readonly ModSettingString archipelagoIP = "archipelago.gg";
    private static readonly ModSettingInt archipelagoPort = 38281;
    private static readonly ModSettingString archipelagoSlot = "Player";
    private static readonly ModSettingButton archipelagoConnect = new(delegate ()
    {
        session = ArchipelagoSessionFactory.CreateSession(archipelagoIP, archipelagoPort);

        LoginResult result;

        try
        {
            result = session.TryConnectAndLogin("Bloons TD6", archipelagoSlot, ItemsHandlingFlags.AllItems);
        }
        catch (Exception ex)
        {
            result = new LoginFailure(ex.GetBaseException().Message);
        }

        if (!result.Successful)
        {
            LoginFailure failure = (LoginFailure)result;
            string errorMessage = $"Failed to Connect to {archipelagoIP} as {archipelagoSlot}:";
            foreach (string error in failure.Errors)
            {
                errorMessage += error;
            }
            return;
        }

        sessionReady = true;

        LoginSuccessful loginSuccess = (LoginSuccessful)result;
        HandleSession(loginSuccess.SlotData);
    });

    public override void OnUpdate()
    {
        if (InGame.instance == null) return;

        for (int i = 0; i < notifications.Count; i++)
        {
            var notification = notifications[i];
            //if (!previousNotifs.Contains(notification))
            //{
                Game.instance.ShowMessage(notification, 5f, "Archipelago");
                notifications.Remove(notification);
                i--;
                //previousNotifs.Add(notification);
            //}
            //UpdateServerNotifications();
        }
    }

    //public async void UpdateServerNotifications()
    //{
    //    await Task.Run(() =>
    //    {
    //        session.DataStorage["Notifs"] = previousNotifs;
    //    });
    //}

    private static void HandleSession(Dictionary<string, object> slotData)
    {
        session.Items.ItemReceived += (receivedItemsHelper) =>
        {
            var item = receivedItemsHelper.PeekItem();
            var itemReceivedName = receivedItemsHelper.PeekItemName();
            var itemReceivedPlayer = session.Players.GetPlayerAlias(item.Player);
            var itemReceivedLocation = session.Locations.GetLocationNameFromId(item.Location);
            ModHelper.Msg<BloonsArchipelago>(itemReceivedName + " Recieved from Server");
            notifications.Add("You've received " + itemReceivedName + " from " + itemReceivedPlayer + " at " + itemReceivedLocation);
            if (itemReceivedName.Contains("-MUnlock"))
            {
                MapsUnlocked.Add(itemReceivedName.Replace("-MUnlock", ""));
            } else if (itemReceivedName.Contains("-TUnlock")) {
                MonkeysUnlocked.Add(itemReceivedName.Replace("-TUnlock", ""));
            } else if (itemReceivedName == "Medal")
            {
                Medals++;
            }
            receivedItemsHelper.DequeueItem();
        };

        previousNotifs = session.DataStorage["Notifs"];

        if (session.DataStorage["XP"])
        {
            XPTracker = new ArchipelagoXP(session.DataStorage["Level"], session.DataStorage["XP"],(Int64)slotData["staticXPReq"], (Int64)slotData["maxLevel"]);
        } else
        {
            XPTracker = new ArchipelagoXP((Int64)slotData["staticXPReq"], (Int64)slotData["maxLevel"]);
        }
        ModHelper.Msg<BloonsArchipelago>(slotData["medalsNeeded"] + " Medals Required to Unlock " + slotData["victoryLocation"]);

        VictoryMap = (string)slotData["victoryLocation"];
        MedalRequirement = (Int64)slotData["medalsNeeded"];
        Difficulty = (Int64)slotData["difficulty"];
    }

    public static void CompleteCheck(string checkstring)
    {
        session.Locations.CompleteLocationChecks(session.Locations.GetLocationIdFromName("Bloons TD6",checkstring));
    }

    public static void CompleteRando()
    {
        var statusUpdatePackage = new StatusUpdatePacket();
        statusUpdatePackage.Status = ArchipelagoClientState.ClientGoal;
        session.Socket.SendPacket(statusUpdatePackage);
    }
}