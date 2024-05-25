using BTD_Mod_Helper;

using HarmonyLib;

using Il2CppAssets.Scripts.Data.Knowledge;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Knowledge;
using Il2CppSystem.Collections.Generic;

namespace BloonsArchipelago.Patches.InMap
{
    [HarmonyPatch(typeof(ActiveKnowledge), nameof(ActiveKnowledge.Get), [typeof(HashSet<string>), typeof(GameModel)])]
    internal class KnowledgeActivationPatch
    {
        [HarmonyPrefix]
        private static void Postfix(HashSet<string> knowledges, GameModel gm)
        {
            for(int i = 1; i < 5; i++)
            {
                KnowledgeModel[] list = KnowledgeHelper.GetKnowledgeForCategory((KnowledgeCategory)i);
                foreach (var knowledge in list)
                {
                    ModHelper.Msg<BloonsArchipelago>(knowledge.name + " " + knowledge.idx);
                }
            }
        }
    }
}
