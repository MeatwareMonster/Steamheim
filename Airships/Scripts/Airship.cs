using System;
using System.Collections.Generic;
using Airships.Models;
using UnityEngine;

public class Airship : MonoBehaviour
{
    public Player m_controllingPlayer;

    private List<Player> m_players = new List<Player>();

    private static List<Airship> m_currentAirships = new List<Airship>();

    private Rigidbody m_body;

    public ZNetView m_nview;

    public float ControlStartTime;

    public Vector3 m_moveDir;

    public float m_cameraDistance;

    public float m_lift;
    public float m_thrust;
    public float m_turnSpeed;

    public float m_throttleChangeSpeed = 0.5f;
    public float m_liftChangeSpeed = 0.5f;

    private void Awake()
    {
        m_nview = GetComponent<ZNetView>();
        m_nview.Register<ZDOID>("RequestControl", RPC_RequestControl);
        m_nview.Register<ZDOID>("ReleaseControl", RPC_ReleaseControl);
        m_nview.Register<bool>("RequestRespons", RPC_RequestRespons);

        m_body = GetComponent<Rigidbody>();
        WearNTear component = GetComponent<WearNTear>();
        if ((bool)component)
        {
            component.m_onDestroyed = (Action)Delegate.Combine(component.m_onDestroyed, new Action(OnDestroyed));
        }
        if (m_nview.GetZDO() == null)
        {
            base.enabled = false;
        }

        m_body.constraints =
            RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        m_body.centerOfMass = Vector3.zero;
        m_body.inertiaTensorRotation = Quaternion.identity;

        //m_body.maxDepenetrationVelocity = 2f;
        //Heightmap.ForceGenerateAll();
    }

    public bool CanBeRemoved()
    {
        return m_players.Count == 0;
    }

    private void Start()
    {
        InvokeRepeating("UpdateOwner", 2f, 2f);
    }

    public void ApplyMovementControls(Vector3 dir)
    {
        m_moveDir = dir;
    }

    private void FixedUpdate()
    {
        if ((bool)m_nview && !m_nview.IsOwner())
        {
            return;
        }

        var throttleZ = m_nview.m_zdo.GetFloat("ThrottleZ");
        var throttleY = m_nview.m_zdo.GetFloat("ThrottleY");

        if (m_players.Count == 0)
        {
            throttleZ = 0;
            throttleY = 0;
        }
        else
        {
            throttleZ = Mathf.Clamp(throttleZ + m_moveDir.z * Time.deltaTime * m_throttleChangeSpeed, -1, 1);
            throttleY = Mathf.Clamp(throttleY + m_moveDir.y * Time.deltaTime * m_liftChangeSpeed, -1, 1);
        }

        m_nview.m_zdo.Set("ThrottleZ", throttleZ);
        m_nview.m_zdo.Set("ThrottleY", throttleY);

        m_body.AddForce(transform.TransformDirection(0, throttleY * m_lift, throttleZ * m_thrust) * Time.deltaTime, ForceMode.VelocityChange);
        m_body.AddTorque(transform.up * m_moveDir.x * m_turnSpeed * Time.deltaTime, ForceMode.VelocityChange);
    }

    private void UpdateOwner()
    {
        if (m_nview.IsValid() && m_nview.IsOwner() && !(Player.m_localPlayer == null) && m_players.Count > 0 && !IsPlayerOnAirship(Player.m_localPlayer))
        {
            long owner = m_players[0].GetOwner();
            m_nview.GetZDO().SetOwner(owner);
            ZLog.Log("Changing ship owner to " + owner);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        Player component = collider.GetComponent<Player>();
        if ((bool)component)
        {
            m_players.Add(component);
            ZLog.Log("Player onboard, total onboard " + m_players.Count);
            if (component == Player.m_localPlayer)
            {
                m_currentAirships.Add(this);
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        Player component = collider.GetComponent<Player>();
        if ((bool)component)
        {
            m_players.Remove(component);
            ZLog.Log("Player over board, players left " + m_players.Count);
            if (component == Player.m_localPlayer)
            {
                m_currentAirships.Remove(this);
            }
        }
    }

    public bool IsPlayerOnAirship(ZDOID zdoid)
    {
        foreach (Player player in m_players)
        {
            if (player.GetZDOID() == zdoid)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsPlayerOnAirship(Player player)
    {
        return m_players.Contains(player);
    }

    public bool HasPlayerOnboard()
    {
        return m_players.Count > 0;
    }

    private void OnDestroyed()
    {
        if (m_nview.IsValid() && m_nview.IsOwner())
        {
            Gogan.LogEvent("Game", "ShipDestroyed", base.gameObject.name, 0L);
        }
        m_currentAirships.Remove(this);
    }

    public static Airship GetLocalShip()
    {
        if (m_currentAirships.Count == 0)
        {
            return null;
        }
        return m_currentAirships[m_currentAirships.Count - 1];
    }

    public bool HaveControllingPlayer()
    {
        return m_controllingPlayer != null;
    }

    public bool IsOwner()
    {
        if (!m_nview.IsValid())
        {
            return false;
        }
        return m_nview.IsOwner();
    }

    private void RPC_RequestControl(long sender, ZDOID playerID)
    {
        if (m_nview.IsOwner())
        {
            if (GetUser() == playerID || !HaveValidUser())
            {
                Jotunn.Logger.LogInfo("Requesting airship control.");
                m_nview.GetZDO().Set("user", playerID);
                m_nview.m_zdo.SetOwner(ZDOMan.instance.GetZDO(playerID).m_owner);
                m_nview.InvokeRPC(sender, "RequestRespons", true);
            }
            else
            {
                m_nview.InvokeRPC(sender, "RequestRespons", false);
            }
        }
    }

    private void RPC_ReleaseControl(long sender, ZDOID playerID)
    {
        if (m_nview.IsOwner() && GetUser() == playerID)
        {
            Jotunn.Logger.LogInfo("Airship released.");
            m_nview.GetZDO().Set("user", ZDOID.None);
        }
    }

    private void RPC_RequestRespons(long sender, bool granted)
    {
        if (!Player.m_localPlayer)
        {
            return;
        }
        if (granted)
        {
            Player.m_localPlayer.GetAdditionalData().m_airship = this;
            ControlStartTime = Time.time;
            Jotunn.Logger.LogInfo("Airship control granted.");
        }
        else
        {
            Player.m_localPlayer.Message(MessageHud.MessageType.Center, "$msg_inuse");
        }
    }

    private ZDOID GetUser()
    {
        if (!m_nview.IsValid())
        {
            return ZDOID.None;
        }
        return m_nview.GetZDO().GetZDOID("user");
    }
    public bool HaveValidUser()
    {
        ZDOID user = GetUser();
        if (user.IsNone())
        {
            return false;
        }
        return IsPlayerOnAirship(user);
    }
}
