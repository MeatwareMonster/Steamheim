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
                    //airship.ControlStartTime = Time.time;
                    //__instance.GetAdditionalData().m_airship = airship;
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
            private static bool Prefix(Player __instance, Vector3 movedir)
            {
                var airship = __instance.GetAdditionalData().m_airship;

                if (airship == null)
                {
                    return true;
                }

                if ((ZInput.GetButtonDown("Use") || ZInput.GetButtonDown("JoyUse")) && airship.ControlStartTime < Time.time - 1f)
                {
                    __instance.AttachStop();
                    return true;
                }

                if (ZInput.GetButton("Jump") || ZInput.GetButton("JoyJump"))
                {
                    movedir.y += 1;
                }
                if (ZInput.GetButton("Crouch") || ZInput.GetButton("JoyCrouch"))
                {
                    movedir.y -= 1;
                }

                airship.ApplyMovementControls(movedir);
                return false;
            }
        }
    }
}
