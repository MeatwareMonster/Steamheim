using System;
using Airships.Models;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Airships.Patches
{
    public static class Player_Patch
    {
        public static GameObject text;
        public static float storedCameraDistance;
        public static float storedMaxCameraDistance;
        public static bool isAwaitingControl;

        [HarmonyPatch(typeof(Player), nameof(Player.AttachStart))]
        class Player_AttachStart_Patch
        {
            private static void Postfix(Player __instance, Transform attachPoint)
            {
                if (__instance.m_attachPoint != null && __instance.m_attachPoint.GetComponentInParent<Chair>().m_name
                    .Equals("controls", StringComparison.InvariantCultureIgnoreCase))
                {
                    var airship = attachPoint.gameObject.GetComponentInParent<Airship>();
                    if (airship != null)
                    {
                        isAwaitingControl = true;
                        airship.m_nview.InvokeRPC("RequestControl", __instance.GetZDOID());
                    }
                }
            }
        }

        [HarmonyPatch(typeof(Player), nameof(Player.AttachStop))]
        class Player_AttachStop_Patch
        {
            private static void Prefix(Player __instance)
            {
                if (__instance.m_attachPoint != null && __instance.m_attachPoint.GetComponentInParent<Chair>().m_name.Equals("controls", StringComparison.InvariantCultureIgnoreCase))
                {
                    var airship = __instance.m_attachPoint.gameObject.GetComponentInParent<Airship>();
                    if (airship != null)
                    {
                        Player.m_localPlayer.GetAdditionalData().m_airship = null;
                        GameCamera.instance.m_distance = storedCameraDistance;
                        GameCamera.instance.m_maxDistance = storedMaxCameraDistance;
                        Object.Destroy(text);
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
                if (isAwaitingControl) return false;

                var airship = __instance.GetAdditionalData().m_airship;

                if (airship == null)
                {
                    return true;
                }

                if ((ZInput.GetButtonDown("Use") || ZInput.GetButtonDown("JoyUse")) && airship.ControlStartTime < Time.time - 1f)
                {
                    airship.ApplyMovementControls(Vector3.zero);
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
                text.GetComponent<Text>().text = $"Thrust: {airship.m_nview.m_zdo.GetFloat("ThrottleZ") * 100:F0}%\nLift: {airship.m_nview.m_zdo.GetFloat("ThrottleY") * 100:F0}%";
                return false;
            }
        }
    }
}
