using HarmonyLib;
using Il2CppAssets.MainMenuWorld.Scripts;

namespace BloonsArchipelago.Patches.HomeMenu
{
    [HarmonyPatch(typeof(MainMenuWorldChoreographer), nameof(MainMenuWorldChoreographer.Start))]
    internal class QuestMonkeyRemover
    {
        [HarmonyPrefix]
        private static void Prefix(MainMenuWorldChoreographer __instance)
        {
            if (BloonsArchipelago.sessionHandler.ready)
            {
                var Interactive = __instance.gameObject.transform.GetChild(0);
                Interactive.GetChild(1).gameObject.SetActive(false);
                Interactive.GetChild(2).gameObject.SetActive(false);
                Interactive.GetChild(11).gameObject.SetActive(false);
            }
        }
    }
}
