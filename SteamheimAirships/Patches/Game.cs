using HarmonyLib;

namespace Airships.Patches
{
    class Game_Patch
    {
        // Attempt to send the player home if they log out on top of an airship
        // Not working yet, commenting out code for now
        [HarmonyPatch(typeof(Game), nameof(Game.FindSpawnPoint))]
        class Game_Interact_Patch
        {
            private static bool Prefix(Game __instance, float dt)
            {
                if (!__instance.m_playerProfile.HaveLogoutPoint()) return true;

                var logoutPoint = __instance.m_playerProfile.GetLogoutPoint();
                ZNet.instance.SetReferencePosition(logoutPoint);
                if (ZNetScene.instance.IsAreaReady(logoutPoint))
                {
                    if (logoutPoint.y - ZoneSystem.instance.GetSolidHeight(logoutPoint) > 10)
                    {
                        Jotunn.Logger.LogInfo("Player too high.");
                        __instance.m_playerProfile.ClearLoguoutPoint();
                    }
                    return true;
                }

                __instance.m_respawnWait += dt;
                return false;
            }
        }
    }
}
