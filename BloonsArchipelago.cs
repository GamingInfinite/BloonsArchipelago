using MelonLoader;
using BTD_Mod_Helper;
using BloonsArchipelago;
using BloonsArchipelago.Utils;
using BTD_Mod_Helper.Api.ModOptions;
using Il2CppAssets.Scripts.Data.MapSets;
using Il2CppAssets.Scripts.Data;

[assembly: MelonInfo(typeof(BloonsArchipelago.BloonsArchipelago), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace BloonsArchipelago;

public class BloonsArchipelago : BloonsTD6Mod
{
    public static SessionHandler Session = new();

    static readonly ModSettingString url = "archipelago.gg";
    static readonly ModSettingInt port = 25565;
    static readonly ModSettingString slot = "Player";
    static readonly ModSettingString password = "";
    static readonly ModSettingButton archipelagoConnect = new(()=>
    {
        //ModHelper.Msg<BloonsArchipelago>("Connecting...");
        //Session = new SessionHandler(url, port, slot, password);

        foreach (MapDetails map in GameData._instance.mapSet.Maps.items)
        {
            ModHelper.Msg<BloonsArchipelago>(map.id);
        }
    });

    public override void OnApplicationStart()
    {
        ModHelper.Msg<BloonsArchipelago>("BloonsArchipelago loaded!");
    }
}