using BTD_Mod_Helper;
using HarmonyLib;
using Il2CppAssets.Scripts.Data.Knowledge;
using Il2CppAssets.Scripts.Models.Knowledge;
using Il2CppAssets.Scripts.Unity.UI_New.Knowledge;

namespace BloonsArchipelago.Patches.KnowledgeMenu
{
    [HarmonyPatch(typeof(KnowledgeSkillBtn), nameof(KnowledgeSkillBtn.SetState))]
    internal class KnowledgeButtonPatch
    {
        [HarmonyPrefix]
        private static void Prefix(KnowledgeSkillBtn __instance, ref KnowlegdeSkillBtnState state)
        {
            if (BloonsArchipelago.sessionHandler.ready)
            {
                KnowledgeModel model = KnowledgeHelper.GetKnowledge(__instance.knowledgeID);
                string[] prereqs = model.prerequisiteIds;
                if (BloonsArchipelago.sessionHandler.KnowledgeUnlocked.Contains(model.name))
                {
                    long id = BloonsArchipelago.sessionHandler.session.Locations.GetLocationIdFromName("Bloons TD6", model.name + "-Tree");
                    if (BloonsArchipelago.sessionHandler.session.Locations.AllLocationsChecked.Contains(id))
                    {
                        state = KnowlegdeSkillBtnState.Purchased;
                    }
                    else
                    {
                        bool available = true;
                        foreach (string prereq in prereqs)
                        {
                            long prereqid = BloonsArchipelago.sessionHandler.session.Locations.GetLocationIdFromName("Bloons TD6", prereq + "-Tree");
                            if (!BloonsArchipelago.sessionHandler.session.Locations.AllLocationsChecked.Contains(prereqid))
                            {
                                available = false;
                            }
                        }
                        if (available)
                        {
                            state = KnowlegdeSkillBtnState.Available;
                        }
                        else
                        {
                            state = KnowlegdeSkillBtnState.Locked;
                        }
                    }
                }
                else
                {
                    state = KnowlegdeSkillBtnState.Locked;
                }
            }
        }
    }
}
