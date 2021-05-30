using System;
using System.Runtime.CompilerServices;

namespace Airships.Models
{
    [Serializable]
    public class ShipAdditionalData
    {
        public float VerticalForce;

        public ShipAdditionalData()
        {
            VerticalForce = 0;
        }
    }

    public static class ShipExtension
    {
        private static readonly ConditionalWeakTable<Ship, ShipAdditionalData> data =
            new ConditionalWeakTable<Ship, ShipAdditionalData>();

        public static ShipAdditionalData GetAdditionalData(this Ship ship)
        {
            return data.GetOrCreateValue(ship);
        }

        public static void AddData(this Ship ship, ShipAdditionalData value)
        {
            try
            {
                data.Add(ship, value);
            }
            catch (Exception) { }
        }
    }
}
