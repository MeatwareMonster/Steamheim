// SteamheimAirships
// Adds flying airships to the game.
// 
// File:    Mod.cs
// Project: SteamheimAirships

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Airships.Models;
using Airships.Services;
using BepInEx;
using HarmonyLib;
using Jotunn.Managers;
using Jotunn.Utils;
using UnityEngine;

namespace Airships
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class Mod : BaseUnityPlugin
    {
        public const string PluginGUID = "MeatwareMonster.SteamheimAirships";
        public const string PluginName = "Steamheim Airships";
        public const string PluginVersion = "1.1.0";

        public static string ModLocation = Path.GetDirectoryName(typeof(Mod).Assembly.Location);

        private readonly Harmony harmony = new Harmony(PluginGUID);

        private readonly Dictionary<string, AssetBundle> AssetBundles = new Dictionary<string, AssetBundle>();

        private void Awake()
        {
            LoadAssetBundles();
            AddAirships();
            UnloadAssetBundles();

            harmony.PatchAll();
        }

        private void LoadAssetBundles()
        {
            // Load asset bundle from embedded resources
            //Jotunn.Logger.LogInfo($"Embedded resources: {string.Join(",", typeof(Mod).Assembly.GetManifestResourceNames())}");
            //AssetBundles["godships"] = AssetUtils.LoadAssetBundleFromResources("godships", typeof(Mod).Assembly);
            //AssetBundles["airships"] = AssetUtils.LoadAssetBundleFromResources("airships", typeof(Mod).Assembly);

            foreach (var file in Directory.GetFiles($"{ModLocation}/Assets/AssetBundles"))
            {
                AssetBundles.Add(Path.GetFileName(file), AssetUtils.LoadAssetBundle(file));
            }
        }

        private void UnloadAssetBundles()
        {
            foreach (var assetBundle in AssetBundles)
            {
                assetBundle.Value.Unload(false);
            }
        }

        private void AddAirships()
        {
            var airshipConfigs = new List<AirshipConfig>();
            var customConfigFiles = Directory.GetFiles($"{ModLocation}/Assets/CustomConfigs").ToDictionary(file => Path.GetFileName(file));

            foreach (var file in Directory.GetFiles($"{ModLocation}/Assets/Configs"))
            {
                string configPath;
                if (customConfigFiles.TryGetValue(Path.GetFileName(file), out var customConfigFile))
                {
                    configPath = customConfigFile;
                }
                else
                {
                    configPath = file;
                }

                airshipConfigs.AddRange(AirshipConfigManager.LoadShipsFromJson(configPath));
            }

            airshipConfigs.ForEach(airshipConfig =>
            {
                if (airshipConfig.enabled)
                {
                    // Load prefab from asset bundle and apply config
                    var prefab = AssetBundles[airshipConfig.bundleName].LoadAsset<GameObject>(airshipConfig.prefabPath);
                    var airship = prefab.AddComponent<Airship>();
                    airship.m_thrust = airshipConfig.thrust;
                    airship.m_lift = airshipConfig.lift;
                    airship.m_turnSpeed = airshipConfig.turnSpeed;
                    airship.m_cameraDistance = airshipConfig.cameraDistance;
                    airship.m_drag = airshipConfig.drag;
                    var airshipBody = prefab.GetComponent<Rigidbody>();
                    airshipBody.mass = airshipConfig.mass;
                    airshipBody.drag = 1f;
                    var airshipPiece = AirshipConfig.Convert(prefab, airshipConfig);

                    // Jotunn code is currently not setting the description, potentially a bug
                    airshipPiece.Piece.m_description = airshipConfig.description;

                    PieceManager.Instance.AddPiece(airshipPiece);
                }
            });
        }
    }
}