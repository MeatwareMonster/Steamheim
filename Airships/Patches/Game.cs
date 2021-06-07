using HarmonyLib;

namespace Godships.Patches
{
    class Game_Patch
    {
        [HarmonyPatch(typeof(Game), nameof(Game.FindSpawnPoint))]
        class Game_Interact_Patch
        {
            private static void Prefix(Game __instance)
            {
            }
        }
    }
}
