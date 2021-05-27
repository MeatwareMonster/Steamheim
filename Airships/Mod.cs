// Airships
// a Valheim mod skeleton using Jötunn
// 
// File:    Airships.cs
// Project: Airships

using BepInEx;
using HarmonyLib;

namespace Airships
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    //[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class Airships : BaseUnityPlugin
    {
        public const string PluginGUID = "steamheim.Airships";
        public const string PluginName = "SteamheimAirships";
        public const string PluginVersion = "1.0.0";

        private readonly Harmony harmony = new Harmony(PluginGUID);

        private void Awake()
        {
            // Do all your init stuff here
            // Acceptable value ranges can be defined to allow configuration via a slider in the BepInEx ConfigurationManager: https://github.com/BepInEx/BepInEx.ConfigurationManager
            //Config.Bind<int>("Main Section", "Example configuration integer", 1, new ConfigDescription("This is an example config, using a range limitation for ConfigurationManager", new AcceptableValueRange<int>(0, 100)));

            // Jotunn comes with its own Logger class to provide a consistent Log style for all mods using it
            Jotunn.Logger.LogInfo("ModStub has landed");

            harmony.PatchAll();
        }

        void OnDestroy()
        {
            harmony.UnpatchSelf();
        }

#if DEBUG
        //private void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.F6))
        //    { // Set a breakpoint here to break on F6 key press
        //    }
        //}
#endif
    }
}