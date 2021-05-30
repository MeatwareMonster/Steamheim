using System;
using System.Runtime.CompilerServices;

namespace Airships.Models
{
    [Serializable]
    public class PlayerAdditionalData
    {
        public Airship m_airship;

        public PlayerAdditionalData()
        {
            m_airship = null;
        }
    }

    public static class PlayerExtension
    {
        private static readonly ConditionalWeakTable<Player, PlayerAdditionalData> data =
            new ConditionalWeakTable<Player, PlayerAdditionalData>();

        public static PlayerAdditionalData GetAdditionalData(this Player player)
        {
            return data.GetOrCreateValue(player);
        }

        public static void AddData(this Player player, PlayerAdditionalData value)
        {
            try
            {
                data.Add(player, value);
            }
            catch (Exception) { }
        }
    }
}
