using Airships.Models;
using HarmonyLib;
using UnityEngine;

namespace Airships.Patches
{
    static class Player_Patch
    {
        [HarmonyPatch(typeof(Player), nameof(Player.AttachStart))]
        class Player_AttachStart_Patch
        {
            private static void Postfix(Player __instance, Transform attachPoint)
            {
                var airship = attachPoint.gameObject.GetComponentInParent<Airship>();
                if (airship != null)
                {
                    __instance.GetAdditionalData().m_airship = airship;
                    airship.m_nview.InvokeRPC("RequestControl", __instance.GetZDOID());
                }
            }
        }

        [HarmonyPatch(typeof(Player), nameof(Player.AttachStop))]
        class Player_AttachStop_Patch
        {
            private static void Prefix(Player __instance)
            {
                if (__instance.m_attachPoint != null)
                {
                    var airship = __instance.m_attachPoint.gameObject.GetComponentInParent<Airship>();
                    if (airship != null)
                    {
                        Player.m_localPlayer.GetAdditionalData().m_airship = null;
                        airship.m_nview.InvokeRPC("ReleaseControl", __instance.GetZDOID());
                    }
                }
            }
        }

        [HarmonyPatch(typeof(Player), nameof(Player.SetControls))]
        class Player_SetControls_Patch
        {
            private static bool Prefix(Player __instance, Vector3 movedir, bool jump)
            {
                var airship = __instance.GetAdditionalData().m_airship;

                if (airship == null || jump) return true;

                if (Input.GetKey(KeyCode.UpArrow))
                {
                    movedir.y += 1;
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    movedir.y -= 1;
                }

                airship.ApplyMovementControls(movedir);
                return false;
            }
        }
    }
}
