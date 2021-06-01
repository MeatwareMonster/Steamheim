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

        private AssetBundle AirshipBundle;
        private AssetBundle GodshipBundle;

        public static ConfigEntry<float> GodshipSpeed;
        public static ConfigEntry<float> GodshipTurnSpeed;
        public static ConfigEntry<float> GodshipLift;

        private void Awake()
        {
            GodshipSpeed = Config.Bind("Godship", "Godship speed", 100f, "Forward/backward speed for godships");
            GodshipTurnSpeed = Config.Bind("Godship", "Godship turn speed", 10f, "Turn speed for godships");
            GodshipLift = Config.Bind("Godship", "Godship lift", 100f, "Vertical speed for godships");

            // Jotunn comes with its own Logger class to provide a consistent Log style for all mods using it
            Jotunn.Logger.LogInfo("ModStub has landed");

            LoadAssetBundles();
            AddAirships();
            AddGodships();
            UnloadAssetBundles();

            harmony.PatchAll();
        }

        private void LoadAssetBundles()
        {
            // Load asset bundle from embedded resources
            Jotunn.Logger.LogInfo($"Embedded resources: {string.Join(",", typeof(Mod).Assembly.GetManifestResourceNames())}");
            GodshipBundle = AssetUtils.LoadAssetBundleFromResources("godships", typeof(Mod).Assembly);
            AirshipBundle = AssetUtils.LoadAssetBundleFromResources("airships", typeof(Mod).Assembly);
        }

        private void UnloadAssetBundles()
        {
            AirshipBundle.Unload(false);
            GodshipBundle.Unload(false);
        }

        private void AddAirships()
        {
            var prefab = AirshipBundle.LoadAsset<GameObject>("Assets/CustomItems/Steampunk/Airship/Airship.prefab");
            prefab.AddComponent<Airship>();
            prefab.GetComponentInChildren<Rigidbody>().freezeRotation = true;
            //prefab.GetComponent<Rigidbody>().isKinematic = true;
            var airship = new CustomPiece(prefab, "Hammer", true);
            PieceManager.Instance.AddPiece(airship);
        }

        private void AddGodships()
        {
            var prefab = GodshipBundle.LoadAsset<GameObject>("Assets/CustomItems/Steampunk/Titan.prefab");
            prefab.AddComponent<Airship>();
            prefab.GetComponent<Rigidbody>().freezeRotation = true;
            //prefab.GetComponent<Rigidbody>().isKinematic = true;
            var titan = new CustomPiece(prefab, "Hammer", true);
            PieceManager.Instance.AddPiece(titan);
        }
    }
}