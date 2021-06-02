using HarmonyLib;

namespace Godships.Patches
{
    class Ladder_Patch
    {
        [HarmonyPatch(typeof(Ladder), nameof(Ladder.Interact))]
        class Ladder_Interact_Patch
        {
            private static void Prefix(Ladder __instance, Humanoid character, bool hold)
            {
                if (!hold && __instance.InUseDistance(character))
                {
                    character.m_maxAirAltitude = __instance.m_targetPos.transform.position.y;
                }
            }
        }
    }
}
