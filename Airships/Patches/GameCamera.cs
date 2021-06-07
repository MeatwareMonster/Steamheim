using Airships.Models;
using HarmonyLib;

namespace Airships.Patches
{
    class GameCamera_Patch
    {
        [HarmonyPatch(typeof(GameCamera), nameof(GameCamera.CollideRay2))]
        class GameCamera_CollideRay2_Patch
        {
            private static bool Prefix()
            {
                return !Player.m_localPlayer.GetAdditionalData().m_airship;

            }
        }
    }
}
