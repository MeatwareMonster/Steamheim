// Airships
// a Valheim mod skeleton using Jötunn
// 
// File:    Airships.cs
// Project: Airships

using BepInEx;
using BepInEx.Configuration;
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
        public const string PluginGUID = "steamheim.Godships";
        public const string PluginName = "SteamheimGodships";
        public const string PluginVersion = "1.0.0";

        private readonly Harmony harmony = new Harmony(PluginGUID);

        private AssetBundle EmbeddedResourceBundle;

        public static ConfigEntry<float> TitanSpeed;
        public static ConfigEntry<float> TitanTurnSpeed;
        public static ConfigEntry<float> TitanLift;

        private void Awake()
        {
            TitanSpeed = Config.Bind("Titan", "Titan speed", 100f, "Forward/backward speed for the Titan");
            TitanTurnSpeed = Config.Bind("Titan", "Titan turn speed", 10f, "Forward/backward speed for the Titan");
            TitanLift = Config.Bind("Titan", "Titan lift", 100f, "Forward/backward speed for the Titan");

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
            EmbeddedResourceBundle = AssetUtils.LoadAssetBundleFromResources("godships", typeof(Mod).Assembly);
        }

        private void UnloadAssetBundle()
        {
            EmbeddedResourceBundle.Unload(false);
        }

        private void AddAirship()
        {
            var prefab = EmbeddedResourceBundle.LoadAsset<GameObject>("Assets/CustomItems/Steampunk/Titan.prefab");
            prefab.AddComponent<Airship>();
            prefab.GetComponent<Rigidbody>().freezeRotation = true;
            var titan = new CustomPiece(prefab, "Hammer", true);
            PieceManager.Instance.AddPiece(titan);
        }
    }
}