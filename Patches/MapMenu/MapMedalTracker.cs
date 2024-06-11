using BTD_Mod_Helper;
using HarmonyLib;
using Il2CppAssets.Scripts.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonsArchipelago.Patches.MapMenu
{
    [HarmonyPatch(typeof(MapInfoManager), nameof(MapInfoManager.HasCompletedMode))]
    internal class MapMedalTracker
    {
        [HarmonyPrefix]
        public static bool Postfix(string map, string difficulty, string mode, ref bool __result)
        {   
            if (BloonsArchipelago.sessionHandler.ready)
            {
                string locationName = map + "-";
                if (mode == "Standard")
                {
                    locationName += difficulty;
                } else
                {
                    locationName += mode;
                }
                if (BloonsArchipelago.sessionHandler.LocationChecked(locationName))
                {
                    __result = true;
                } else
                {
                    __result = false;
                }
                return false;
            }
            return true;
        }
    }
}
