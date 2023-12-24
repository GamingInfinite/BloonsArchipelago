using MelonLoader;
using BTD_Mod_Helper;
using BloonsArchipelago;

[assembly: MelonInfo(typeof(BloonsArchipelago.BloonsArchipelago), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace BloonsArchipelago;

public class BloonsArchipelago : BloonsTD6Mod
{
    public override void OnApplicationStart()
    {
        ModHelper.Msg<BloonsArchipelago>("BloonsArchipelago loaded!");
    }
}