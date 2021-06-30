using System;
using System.Collections.Generic;
using System.Linq;
using Jotunn.Configs;
using Jotunn.Entities;
using UnityEngine;

namespace Airships.Models
{
    [Serializable]
    public class AirshipConfigRequirement
    {
        public string item;
        public int amount;

        public static RequirementConfig Convert(AirshipConfigRequirement airshipConfigRequirement)
        {
            return new RequirementConfig()
            {
                Amount = airshipConfigRequirement.amount,
                Item = airshipConfigRequirement.item,
                Recover = true
            };
        }
    }

    [Serializable]
    public class AirshipConfig
    {
        public string name;
        public string bundleName;
        public string prefabPath;
        public string description;
        public string pieceTable;
        public float mass;
        public float thrust;
        public float lift;
        public float drag;
        public float turnSpeed;
        public float cameraDistance;
        public bool enabled;
        public List<AirshipConfigRequirement> resources;

        public static CustomPiece Convert(GameObject prefab, AirshipConfig airshipConfig)
        {
            return new CustomPiece(prefab,
                new PieceConfig
                {
                    Description = airshipConfig.description,
                    Enabled = airshipConfig.enabled,
                    PieceTable = airshipConfig.pieceTable,
                    Category = "Airships",
                    Requirements = airshipConfig.resources.Select(AirshipConfigRequirement.Convert).ToArray()
                }
            );
        }
    }
}
