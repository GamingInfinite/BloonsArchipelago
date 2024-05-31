using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;
using Archipelago.MultiClient.Net.Packets;

using BTD_Mod_Helper;

using Il2CppAssets.Scripts.Data.MapSets;
using GameData = Il2CppAssets.Scripts.Data.GameData;

using System;
using System.Collections.Generic;

namespace BloonsArchipelago.Utils
{
    public class SessionHandler
    {
        public ArchipelagoSession session;
        public bool ready = false;

        public ArchipelagoXP XPTracker;

        public List<string> notifications = new();
        public List<string> previousNotifications = new();

        public List<string> MapsUnlocked = new();
        public List<string> MonkeysUnlocked = new();
        public List<string> KnowledgeUnlocked = new();

        public MapDetails[] defaultMapList;

        public string APID = "";
        public string VictoryMap = "";
        public long MedalRequirement = 0;
        public long Difficulty = 0;
        public int Medals = 0;

        public string currentMap = "";
        public string currentMode = "";

        public SessionHandler() { }
        //C# notes that XPTracker has a state where it isn't set at the end of this.  It's never used under that case.
        public SessionHandler(string url, int port, string slot, string password)
        {
            defaultMapList = GameData._instance.mapSet.Maps.items;

            session = ArchipelagoSessionFactory.CreateSession(url, port);

            LoginResult result;

            try
            {
                result = session.TryConnectAndLogin("Bloons TD6", slot, ItemsHandlingFlags.AllItems, password: password);
            }
            catch (Exception ex)
            {
                result = new LoginFailure(ex.GetBaseException().Message);
            }

            if (!result.Successful)
            {
                LoginFailure failure = (LoginFailure)result;
                string errorMessage = $"Failed to Connect to {url} as {slot}:";
                foreach (string error in failure.Errors)
                {
                    errorMessage += error;
                }
                return;
            }

            ready = true;

            LoginSuccessful loginSuccess = (LoginSuccessful)result;
            Dictionary<string, object> slotData = loginSuccess.SlotData;

            //Setup Item Recieving;
            session.Items.ItemReceived += (receivedItemsHelper) =>
            {
                NetworkItem item = receivedItemsHelper.PeekItem();
                string itemName = receivedItemsHelper.PeekItemName();
                string itemPlayer = session.Players.GetPlayerAlias(item.Player);
                string itemLocation = session.Locations.GetLocationNameFromId(item.Location);
                ModHelper.Msg<BloonsArchipelago>(itemName + " Received from Server");

                notifications.Add("You've received " + itemName + " from " + itemPlayer + " at " + itemLocation);
                if (itemName.Contains("-MUnlock"))
                {
                    MapsUnlocked.Add(itemName.Replace("-MUnlock", ""));
                }
                else if (itemName.Contains("-TUnlock"))
                {
                    MonkeysUnlocked.Add(itemName.Replace("-TUnlock", ""));
                }
                else if (itemName.Contains("-KUnlock"))
                {
                    KnowledgeUnlocked.Add(itemName.Replace("-KUnlock", ""));
                }
                else if (itemName == "Medal")
                {
                    Medals++;
                }
                receivedItemsHelper.DequeueItem();
            };

            //Add Back previously received notifications (as to not run them after a player has closed the game)
            APID = session.RoomState.Seed;
            if (BloonsArchipelago.notifJson.APWorlds.ContainsKey(APID))
            {
                previousNotifications.AddRange(BloonsArchipelago.notifJson.APWorlds[APID]);
            }

            //Setup for XP Passthrough;
            if (session.DataStorage["XP-" + PlayerSlotName()])
            {
                XPTracker = new ArchipelagoXP(session.DataStorage["Level-" + PlayerSlotName()], session.DataStorage["XP-" + PlayerSlotName()], (Int64)slotData["staticXPReq"], (Int64)slotData["maxLevel"], (bool)slotData["xpCurve"]);
            }
            else
            {
                XPTracker = new ArchipelagoXP((Int64)slotData["staticXPReq"], (Int64)slotData["maxLevel"], (bool)slotData["xpCurve"]);
            }

            //Set Slot Settings
            VictoryMap = (string)slotData["victoryLocation"];
            MedalRequirement = (Int64)slotData["medalsNeeded"];
            Difficulty = (Int64)slotData["difficulty"];

            ModHelper.Msg<BloonsArchipelago>(MedalRequirement + " Medals Required to Unlock " + VictoryMap);
        }

        public void CompleteCheck(string checkstring)
        {
            session.Locations.CompleteLocationChecks(session.Locations.GetLocationIdFromName("Bloons TD6", checkstring));
        }

        public void CompleteRando()
        {
            StatusUpdatePacket statusUpdatePackage = new StatusUpdatePacket
            {
                Status = ArchipelagoClientState.ClientGoal
            };
            session.Socket.SendPacket(statusUpdatePackage);
            BloonsArchipelago.notifJson.APWorlds.Remove(APID);
        }

        public MapDetails[] GetMapDetails()
        {
            List<MapDetails> mapDetails = new();
            foreach (var map in defaultMapList)
            {
                if (MapsUnlocked.Contains(map.id) || map.id == VictoryMap)
                {
                    mapDetails.Add(map);
                }
            }
            return mapDetails.ToArray();
        }

        public string PlayerSlotName()
        {
            int slot = session.ConnectionInfo.Slot;
            string name = session.Players.GetPlayerName(slot);
            return name;
        }
    }
}
