using BloonsArchipelago.Utils;

using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;

using Il2CppAssets.Scripts.Unity.UI_New.Main;

using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace BloonsArchipelago.Patches.HomeMenu
{
    [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.Start))]
    internal class SaveNotificationsPatch
    {
        [HarmonyPostfix]
        private static void Postfix()
        {
            if (BloonsArchipelago.sessionHandler.ready)
            {
                string modPath = ModContent.GetInstance<BloonsArchipelago>().GetModDirectory();

                Dictionary<string, string[]> notifDict = BloonsArchipelago.notifJson.APWorlds;
                notifDict.TryAdd(BloonsArchipelago.sessionHandler.APID, BloonsArchipelago.sessionHandler.previousNotifications.ToArray());

                var notificationJSON = JsonSerializer.Serialize(new NotificationJSON { APWorlds = notifDict });
                File.WriteAllText(Path.Combine(modPath, "Notifications.json"), notificationJSON);
            }
        }
    }
}
