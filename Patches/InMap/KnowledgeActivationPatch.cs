using HarmonyLib;

using Il2CppAssets.Scripts.Data.Knowledge;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Knowledge;
using Il2CppSystem.Collections.Generic;

namespace BloonsArchipelago.Patches.InMap
{
    [HarmonyPatch(typeof(ActiveKnowledge), nameof(ActiveKnowledge.Get), new[] { typeof(HashSet<string>), typeof(GameModel) })]
    internal class KnowledgeActivationPatch
    {
        [HarmonyPrefix]
        private static void Prefix(ref HashSet<string> knowledges, GameModel gm)
        {
            if (BloonsArchipelago.sessionHandler.ready)
            {
                knowledges = new HashSet<string>();
                for (int i = 0; i <= 5; i++)
                {
                    KnowledgeModel[] list = KnowledgeHelper.GetKnowledgeForCategory((KnowledgeCategory)i);
                    foreach (KnowledgeModel k in list)
                    {
                        if (BloonsArchipelago.sessionHandler.KnowledgeUnlocked.Contains(k.name))
                        {
                            knowledges.Add(k.name);
                        }
                    }
                }
            }
        }
    }
}
