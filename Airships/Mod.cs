// Airships
// a Valheim mod skeleton using Jötunn
// 
// File:    Airships.cs
// Project: Airships

using System.Collections.Generic;
using Airships.Models;
using Airships.Services;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
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

        public static ConfigEntry<float> GodshipSpeed;
        public static ConfigEntry<float> GodshipTurnSpeed;
        public static ConfigEntry<float> GodshipLift;

        private readonly Dictionary<string, AssetBundle> EmbeddedResourceBundles = new Dictionary<string, AssetBundle>();

        private void Awake()
        {
            GodshipSpeed = Config.Bind("Godship", "Godship speed", 100f, "Forward/backward speed for godships");
            GodshipTurnSpeed = Config.Bind("Godship", "Godship turn speed", 10f, "Turn speed for godships");
            GodshipLift = Config.Bind("Godship", "Godship lift", 100f, "Vertical speed for godships");

            // Jotunn comes with its own Logger class to provide a consistent Log style for all mods using it
            Jotunn.Logger.LogInfo("ModStub has landed");

            LoadAssetBundles();
            AddAirships();
            UnloadAssetBundles();

            harmony.PatchAll();
        }

        private void LoadAssetBundles()
        {
            // Load asset bundle from embedded resources
            Jotunn.Logger.LogInfo($"Embedded resources: {string.Join(",", typeof(Mod).Assembly.GetManifestResourceNames())}");
            EmbeddedResourceBundles["godships"] = AssetUtils.LoadAssetBundleFromResources("godships", typeof(Mod).Assembly);
            EmbeddedResourceBundles["airships"] = AssetUtils.LoadAssetBundleFromResources("airships", typeof(Mod).Assembly);
        }

        private void UnloadAssetBundles()
        {
            foreach (var embeddedResourceBundle in EmbeddedResourceBundles)
            {
                embeddedResourceBundle.Value.Unload(false);
            }
        }

        private void AddAirships()
        {
            var airshipConfigs = AirshipConfigManager.LoadShipsFromJson("Airships/Assets/airshipConfig.json");
            airshipConfigs.ForEach(airshipConfig =>
            {
                if (airshipConfig.enabled)
                {
                    // Load prefab from asset bundle
                    var prefab = EmbeddedResourceBundles[airshipConfig.bundleName].LoadAsset<GameObject>(airshipConfig.prefabPath);
                    var airship = prefab.AddComponent<Airship>();
                    airship.m_thrust = airshipConfig.thrust;
                    airship.m_lift = airshipConfig.lift;
                    airship.m_turnSpeed = airshipConfig.turnSpeed;
                    var airshipBody = prefab.GetComponent<Rigidbody>();
                    airshipBody.freezeRotation = true;
                    airshipBody.mass = airshipConfig.mass;
                    airshipBody.drag = airshipConfig.drag;
                    PieceManager.Instance.AddPiece(AirshipConfig.Convert(prefab, airshipConfig));
                }
            });
        }
    }
}