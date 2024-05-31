using HarmonyLib;
using Il2CppAssets.Scripts.Unity.UI_New.Knowledge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloonsArchipelago.Patches.KnowledgeMenu
{
    [HarmonyPatch(typeof(KnowledgeSkillTree), nameof(KnowledgeSkillTree.AddPointClicked))]
    internal class KnowledgeCheckPatch
    {
        [HarmonyPrefix]
        private static bool Prefix(KnowledgeSkillTree __instance)
        {
            if (BloonsArchipelago.sessionHandler.ready)
            {
                BloonsArchipelago.sessionHandler.CompleteCheck(__instance.currSelectedBtn.knowledgeID + "-Tree");
                __instance.currSelectedBtn.SetState(KnowlegdeSkillBtnState.Purchased);
                return false;
            }
            return true;
        }
    }
}
