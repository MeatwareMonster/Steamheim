// Airships
// a Valheim mod skeleton using Jötunn
// 
// File:    Airships.cs
// Project: Airships

using BepInEx;
using HarmonyLib;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using UnityEngine;

namespace Airships
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    //[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class Mod : BaseUnityPlugin
    {
        public const string PluginGUID = "steamheim.Airships";
        public const string PluginName = "SteamheimAirships";
        public const string PluginVersion = "1.0.0";

        private readonly Harmony harmony = new Harmony(PluginGUID);

        private AssetBundle EmbeddedResourceBundle;

        private void Awake()
        {
            // Do all your init stuff here
            // Acceptable value ranges can be defined to allow configuration via a slider in the BepInEx ConfigurationManager: https://github.com/BepInEx/BepInEx.ConfigurationManager
            //Config.Bind<int>("Main Section", "Example configuration integer", 1, new ConfigDescription("This is an example config, using a range limitation for ConfigurationManager", new AcceptableValueRange<int>(0, 100)));

            // Jotunn comes with its own Logger class to provide a consistent Log style for all mods using it
            Jotunn.Logger.LogInfo("ModStub has landed");

            LoadAssetBundle();
            AddAirship();
            UnloadAssetBundle();

            harmony.PatchAll();
        }

        private void LoadAssetBundle()
        {
            // Load asset bundle from embedded resources
            Jotunn.Logger.LogInfo($"Embedded resources: {string.Join(",", typeof(Mod).Assembly.GetManifestResourceNames())}");
            EmbeddedResourceBundle = AssetUtils.LoadAssetBundleFromResources("airships", typeof(Mod).Assembly);
        }

        private void UnloadAssetBundle()
        {
            EmbeddedResourceBundle.Unload(false);
        }

        private void AddAirship()
        {
            var prefab = EmbeddedResourceBundle.LoadAsset<GameObject>("Assets/CustomItems/Steampunk/Airship/Airship.prefab");
            prefab.AddComponent<Airship>();
            var airship = new CustomPiece(prefab, "Hammer", true);
            PieceManager.Instance.AddPiece(airship);
        }
    }
}