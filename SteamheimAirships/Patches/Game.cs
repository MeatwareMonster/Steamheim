namespace Airships.Patches
{
    class Game_Patch
    {
        // Attempt to send the player home if they log out on top of an airship
        // Not working yet, commenting out code for now
        //[HarmonyPatch(typeof(Game), nameof(Game.FindSpawnPoint))]
        //class Game_Interact_Patch
        //{
        //    private static void Prefix(Game __instance)
        //    {
        //        if (!__instance.m_playerProfile.HaveLogoutPoint()) return;

        //        var logoutPoint = __instance.m_playerProfile.GetLogoutPoint();
        //        if (ZoneSystem.instance.GetSolidHeight(logoutPoint, out var height))
        //        {
        //            Jotunn.Logger.LogInfo($"Login height: {logoutPoint.y}, Solid height: {height}");
        //            if (logoutPoint.y - height > 10)
        //            {
        //                Jotunn.Logger.LogInfo("Player too high.");
        //                __instance.m_playerProfile.ClearLoguoutPoint();
        //            }
        //        }
        //    }
        //}
    }
}
