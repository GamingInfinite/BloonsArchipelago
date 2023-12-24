using Archipelago.MultiClient.Net;
using BloonsArchipelago;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.ModOptions;
using System;
using MelonLoader;
using Archipelago.MultiClient.Net.Enums;

[assembly: MelonInfo(typeof(BloonsArchipelago.BloonsArchipelago), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace BloonsArchipelago;

public class BloonsArchipelago : BloonsTD6Mod
{
    static ArchipelagoSession? session;

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
            result = session.TryConnectAndLogin("Stardew Valley", archipelagoSlot, ItemsHandlingFlags.AllItems);
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

        var loginSuccess = (LoginSuccessful)result;
        HandleSession();
    });

    private static void HandleSession()
    {
        foreach (var item in session.Players.AllPlayers)
        {
            ModHelper.Msg<BloonsArchipelago>(item.Alias + " - " + item.Game);
        }
    }
}