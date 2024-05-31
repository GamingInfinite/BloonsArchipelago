using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Scripts.Unity.UI_New.Knowledge;
using UnityEngine.UI;

namespace BloonsArchipelago.Patches.KnowledgeMenu
{
    [HarmonyPatch(typeof(KnowledgeSkillTree), nameof(KnowledgeSkillTree.Open))]
    internal class KnowledgeMenuUIPatches
    {
        [HarmonyPrefix]
        private static void Prefix(KnowledgeSkillTree __instance)
        {
            if (BloonsArchipelago.sessionHandler.ready)
            {
                Button addpoints = __instance.selectedPanelAddPointsBtn;
                NK_TextMeshProUGUI text = addpoints.transform.GetChild(2).GetComponent<NK_TextMeshProUGUI>();
                text.text = "Check";
            }
        }
    }
}
