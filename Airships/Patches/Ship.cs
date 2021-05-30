using Airships.Models;
using UnityEngine;

namespace Airships.Patches
{
    static class Ship_Patch
    {
        //[HarmonyPatch(typeof(Ship), nameof(Ship.Awake))]
        class Ship_Awake_Patch
        {
            private static void Postfix(Ship __instance)
            {
                __instance.m_body.constraints =
                    RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            }
        }

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

                float waveFactor = 1f;
                Vector3 worldCenterOfMass = __instance.m_body.worldCenterOfMass;
                Vector3 vector = __instance.m_floatCollider.transform.position + __instance.m_floatCollider.transform.forward * __instance.m_floatCollider.size.z / 2f;
                Vector3 vector2 = __instance.m_floatCollider.transform.position - __instance.m_floatCollider.transform.forward * __instance.m_floatCollider.size.z / 2f;
                Vector3 vector3 = __instance.m_floatCollider.transform.position - __instance.m_floatCollider.transform.right * __instance.m_floatCollider.size.x / 2f;
                Vector3 vector4 = __instance.m_floatCollider.transform.position + __instance.m_floatCollider.transform.right * __instance.m_floatCollider.size.x / 2f;
                float waterLevel = WaterVolume.GetWaterLevel(worldCenterOfMass, waveFactor);
                float waterLevel2 = WaterVolume.GetWaterLevel(vector3, waveFactor);
                float waterLevel3 = WaterVolume.GetWaterLevel(vector4, waveFactor);
                float waterLevel4 = WaterVolume.GetWaterLevel(vector, waveFactor);
                float waterLevel5 = WaterVolume.GetWaterLevel(vector2, waveFactor);
                float num = (waterLevel + waterLevel2 + waterLevel3 + waterLevel4 + waterLevel5) / 5f;
                float num2 = worldCenterOfMass.y - num - __instance.m_waterLevelOffset;
                if (num2 > __instance.m_disableLevel)
                {
                    __instance.m_body.WakeUp();

                    float num4 = Vector3.Dot(__instance.m_body.velocity, __instance.transform.forward);
                    float num5 = Vector3.Dot(__instance.m_body.velocity, __instance.transform.right);

                    float sailSize = 0f;
                    if (__instance.m_speed == Ship.Speed.Full)
                    {
                        sailSize = 1f;
                    }
                    else if (__instance.m_speed == Ship.Speed.Half)
                    {
                        sailSize = 0.5f;
                    }
                    Vector3 sailForce = __instance.GetSailForce(sailSize, Time.fixedDeltaTime);
                    Vector3 position = worldCenterOfMass + __instance.transform.up * __instance.m_sailForceOffset;
                    __instance.m_body.AddForceAtPosition(sailForce, position, ForceMode.VelocityChange);
                    Vector3 position2 = __instance.transform.position + __instance.transform.forward * __instance.m_stearForceOffset;
                    float d2 = num4 * __instance.m_stearVelForceFactor;
                    __instance.m_body.AddForceAtPosition(__instance.transform.right * d2 * (0f - __instance.m_rudderValue) * Time.fixedDeltaTime, position2, ForceMode.VelocityChange);
                    Vector3 zero = Vector3.zero;
                    switch (__instance.m_speed)
                    {
                        case Ship.Speed.Slow:
                            zero += __instance.transform.forward * __instance.m_backwardForce * (1f - Mathf.Abs(__instance.m_rudderValue));
                            break;
                        case Ship.Speed.Back:
                            zero += -__instance.transform.forward * __instance.m_backwardForce * (1f - Mathf.Abs(__instance.m_rudderValue));
                            break;
                    }
                    if (__instance.m_speed == Ship.Speed.Back || __instance.m_speed == Ship.Speed.Slow)
                    {
                        float d3 = ((__instance.m_speed != Ship.Speed.Back) ? 1 : (-1));
                        zero += __instance.transform.right * __instance.m_stearForce * (0f - __instance.m_rudderValue) * d3;
                    }
                    __instance.m_body.AddForceAtPosition(zero * Time.fixedDeltaTime, position2, ForceMode.VelocityChange);
                    __instance.ApplyEdgeForce(Time.fixedDeltaTime);
                }
            }
        }
    }
}
