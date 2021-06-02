using System.Collections.Generic;
using Airships.Models;
using Jotunn.Utils;

namespace Airships.Services
{
    class AirshipConfigManager
    {
        public static List<AirshipConfig> LoadShipsFromJson(string shipConfigPath)
        {
            var json = AssetUtils.LoadText(shipConfigPath);
            return SimpleJson.SimpleJson.DeserializeObject<List<AirshipConfig>>(json);
        }
    }
}
