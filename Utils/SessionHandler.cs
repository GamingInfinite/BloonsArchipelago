using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;
using BTD_Mod_Helper;
using Il2CppAssets.Scripts.Data.MapSets;
using System;
using System.Collections.Generic;
using GameData = Il2CppAssets.Scripts.Data.GameData;

namespace BloonsArchipelago.Utils
{
    public class SessionHandler
    {
        public ArchipelagoSession session;
        public bool ready = false;

        public List<string> MapsUnlocked = [];
        public List<string> MonkeysUnlocked = [];
        public List<string> KnowledgeUnlocked = [];
        public List<string> HeroesUnlocked = [];

        public MapDetails[] defaultMapList;

        public string APID = "";
        public string VictoryMap = "";
        public long MedalRequirement = 0;
        public long Difficulty = 0;
        public int Medals = 0;

        public SessionHandler() { }
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
                return;
            }

            ready = true;
            LoginSuccessful loginSuccess = (LoginSuccessful)result;

            Dictionary<string, object> slotData = loginSuccess.SlotData;

            //Item Recieving
            session.Items.ItemReceived += (recievedItemsHelper) =>
            {
                ItemInfo item = recievedItemsHelper.PeekItem();
                string itemName = item.ItemName;
                string itemPlayer = item.Player.Name;
                string itemLocation = item.LocationName;
                ModHelper.Msg<BloonsArchipelago>($"Recieved item {itemName} from {itemPlayer} at {itemLocation}");

                if (itemName is not null)
                {

                }
                recievedItemsHelper.DequeueItem();
            };
        }
    
        public void CompleteCheck()
        {
        }
    }
}
