using Airships.Models;
using HarmonyLib;
using Jotunn.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Airships.Patches
{
    static class Player_Patch
    {
        private static GameObject text;
        private static float storedCameraDistance;
        private static float storedMaxCameraDistance;

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
                    storedCameraDistance = GameCamera.instance.m_distance;
                    storedMaxCameraDistance = GameCamera.instance.m_maxDistance;
                    GameCamera.instance.m_distance = airship.m_cameraDistance;
                    GameCamera.instance.m_maxDistance = airship.m_cameraDistance;
                    text = GUIManager.Instance.CreateText("JötunnLib, the Valheim Lib", GUIManager.PixelFix.transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.7f),
                        new Vector2(0f, 450f), GUIManager.Instance.AveriaSerifBold, 18, GUIManager.Instance.ValheimOrange, true, Color.black, 400f, 30f, false);
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
                text.GetComponent<Text>().text = $"Thrust: {(airship.ThrottleZ * 100):F0}%\nLift: {(airship.ThrottleY * 100):F0}%";
                return false;
            }
        }
    }
}
