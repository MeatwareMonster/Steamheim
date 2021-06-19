﻿// SteamheimAirships
// Adds flying airships to the game.
// 
// File:    Mod.cs
// Project: SteamheimAirships

using System.Collections.Generic;
using System.IO;
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
        public const string PluginVersion = "1.0.1";

        private readonly Harmony harmony = new Harmony(PluginGUID);

        private readonly Dictionary<string, AssetBundle> EmbeddedResourceBundles = new Dictionary<string, AssetBundle>();

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
            var airshipConfigs = AirshipConfigManager.LoadShipsFromJson($"{Path.GetDirectoryName(typeof(Mod).Assembly.Location)}/Assets/airshipConfig.json");
            airshipConfigs.ForEach(airshipConfig =>
            {
                if (airshipConfig.enabled)
                {
                    // Load prefab from asset bundle and apply config
                    var prefab = EmbeddedResourceBundles[airshipConfig.bundleName].LoadAsset<GameObject>(airshipConfig.prefabPath);
                    var airship = prefab.AddComponent<Airship>();
                    airship.m_thrust = airshipConfig.thrust;
                    airship.m_lift = airshipConfig.lift;
                    airship.m_turnSpeed = airshipConfig.turnSpeed;
                    airship.m_cameraDistance = airshipConfig.cameraDistance;
                    var airshipBody = prefab.GetComponent<Rigidbody>();
                    airshipBody.mass = airshipConfig.mass;
                    airshipBody.drag = airshipConfig.drag;
                    var airshipPiece = AirshipConfig.Convert(prefab, airshipConfig);
                    airshipPiece.Piece.m_name = airshipConfig.name;
                    PieceManager.Instance.AddPiece(AirshipConfig.Convert(prefab, airshipConfig));
                }
            });
        }
    }
}