using Airships.Models;
using UnityEngine;

namespace Airships.Patches
{
    static class Ship_Patch
    {
        //[HarmonyPatch(typeof(Ship), nameof(Ship.FixedUpdate))]
        class Ship_FixedUpdate_Patch
        {
            private static void Postfix(Ship __instance)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    __instance.GetAdditionalData().VerticalForce += 0.1f;
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    __instance.GetAdditionalData().VerticalForce -= 0.1f;
                }

                Jotunn.Logger.LogInfo(__instance.GetAdditionalData().VerticalForce);
                //__instance.m_mastObject.transform.position
                //var center = __instance.m_floatCollider.bounds.center;
                //center.y = __instance.m_floatCollider.bounds.max.y;
                __instance.transform.rotation = Quaternion.Euler(0, __instance.transform.rotation.eulerAngles.y, 0);
                __instance.m_body.AddForceAtPosition(
                    Vector3.up * Time.fixedDeltaTime * __instance.GetAdditionalData().VerticalForce,
                    __instance.m_mastObject.transform.position, ForceMode.VelocityChange);

                //if (!__instance.GetAdditionalData().isLocked)
                //    ___displayText.ShowText("Press to LOCK rotation", MyInput.Keybinds["RMB"].MainKey, 2, 0, false);
                //else
                //    ___displayText.ShowText("Press to UNLOCK rotation", MyInput.Keybinds["RMB"].MainKey, 2, 0, false);

                //if (MyInput.GetButtonDown("RMB"))
                //    __instance.GetAdditionalData().isLocked = !__instance.GetAdditionalData().isLocked; // Toggle bool
            }
        }
    }
}
