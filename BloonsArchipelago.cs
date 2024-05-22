using MelonLoader;
using BTD_Mod_Helper;

using BloonsArchipelago;
using BloonsArchipelago.Utils;

using BTD_Mod_Helper.Api.ModOptions;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Extensions;

using System.IO;
using System.Text.Json;
using System.Collections.Generic;

using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Data;

[assembly: MelonInfo(typeof(BloonsArchipelago.BloonsArchipelago), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace BloonsArchipelago;

public class BloonsArchipelago : BloonsTD6Mod
{
    public static NotificationJSON notifJson = new() { APWorlds = new Dictionary<string, string[]>() };

    public static SessionHandler sessionHandler = new();

    // Mod Settings; Used for Connection Purposes
    static readonly ModSettingString url = "archipelago.gg";
    static readonly ModSettingInt port = 25565;
    static readonly ModSettingString slot = "Player";
    static readonly ModSettingString password = "";
    static readonly ModSettingButton archipelagoConnect = new(() =>
    {
        ModHelper.Msg<BloonsArchipelago>("Connecting...");

        sessionHandler = new SessionHandler(url, port, slot, password);
    });

    public override void OnApplicationStart()
    {
        string modPath = ModContent.GetInstance<BloonsArchipelago>().GetModDirectory();

        if (!Directory.Exists(modPath))
        {
            Directory.CreateDirectory(modPath);
        }

        string filepath = Path.Combine(modPath, "Notifications.json");
        if (File.Exists(filepath))
        {
            var JSONString = File.ReadAllText(filepath);
            var NotifObject = JsonSerializer.Deserialize<NotificationJSON>(JSONString);

            notifJson = NotifObject;
        }
        else
        {
            notifJson = new NotificationJSON { APWorlds = new Dictionary<string, string[]>() };
        }
        ModHelper.Msg<BloonsArchipelago>("BloonsArchipelago loaded!");
    }

    public override void OnUpdate()
    {
        if (InGame.instance == null) return;

        for (int i = 0; i < sessionHandler.notifications.Count; i++)
        {
            string notification = sessionHandler.notifications[i];
            if (!sessionHandler.previousNotifications.Contains(notification))
            {
                Game.instance.ShowMessage(notification, 5f, "Archipelago");
                sessionHandler.notifications.Remove(notification);
                sessionHandler.previousNotifications.Add(notification);
            }
            else
            {
                sessionHandler.notifications.Remove(notification);
            }
        }
    }
}