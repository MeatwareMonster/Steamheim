using System;
using Airships.Models;
using UnityEngine;

public class Airship : MonoBehaviour
{
    public enum Speed
    {
        Stop,
        Back,
        Slow,
        Half,
        Full
    }

    public Player m_controllingPlayer;

    public bool m_forwardPressed;

    public bool m_backwardPressed;

    public Speed m_speed;

    public Vector3 m_moveDir;

    //private List<Player> m_players = new List<Player>();

    //private static List<Airship> m_currentShips = new List<Airship>();

    //private Rigidbody m_body;

    public ZNetView m_nview;

    private void Awake()
    {
        m_nview = GetComponent<ZNetView>();
        m_nview.Register<ZDOID>("RequestControl", RPC_RequestControl);
        m_nview.Register<ZDOID>("ReleaseControl", RPC_ReleaseControl);
        m_nview.Register<bool>("RequestRespons", RPC_RequestRespons);

        //m_body = GetComponent<Rigidbody>();
        WearNTear component = GetComponent<WearNTear>();
        if ((bool)component)
        {
            component.m_onDestroyed = (Action)Delegate.Combine(component.m_onDestroyed, new Action(OnDestroyed));
        }
        if (m_nview.GetZDO() == null)
        {
            base.enabled = false;
        }
        //m_body.maxDepenetrationVelocity = 2f;
        //Heightmap.ForceGenerateAll();
        //m_sailCloth = m_sailObject.GetComponentInChildren<Cloth>();
    }

    //public bool CanBeRemoved()
    //{
    //    return m_players.Count == 0;
    //}

    private void Start()
    {
        m_nview.Register("Stop", RPC_Stop);
        m_nview.Register("Forward", RPC_Forward);
        m_nview.Register("Backward", RPC_Backward);
        //m_nview.Register<float>("Rudder", RPC_Rudder);
        InvokeRepeating("UpdateOwner", 2f, 2f);
    }

    //private void PrintStats()
    //{
    //    if (m_players.Count != 0)
    //    {
    //        ZLog.Log("Vel:" + m_body.velocity.magnitude.ToString("0.0"));
    //    }
    //}

    public void ApplyMovementControls(Vector3 dir)
    {
        bool flag = (double)dir.z > 0.5;
        bool flag2 = (double)dir.z < -0.5;
        if (flag && !m_forwardPressed)
        {
            Forward();
        }
        if (flag2 && !m_backwardPressed)
        {
            Backward();
        }
        m_forwardPressed = flag;
        m_backwardPressed = flag2;

        m_moveDir = dir;
    }

    public void Forward()
    {
        m_nview.InvokeRPC("Forward");
    }

    public void Backward()
    {
        m_nview.InvokeRPC("Backward");
    }

    public void Stop()
    {
        m_nview.InvokeRPC("Stop");
    }

    private void RPC_Stop(long sender)
    {
        m_speed = Speed.Stop;
    }

    private void RPC_Forward(long sender)
    {
        switch (m_speed)
        {
            case Speed.Stop:
                m_speed = Speed.Slow;
                break;
            case Speed.Slow:
                m_speed = Speed.Half;
                break;
            case Speed.Half:
                m_speed = Speed.Full;
                break;
            case Speed.Back:
                m_speed = Speed.Stop;
                break;
            case Speed.Full:
                break;
        }
    }

    private void RPC_Backward(long sender)
    {
        switch (m_speed)
        {
            case Speed.Stop:
                m_speed = Speed.Back;
                break;
            case Speed.Slow:
                m_speed = Speed.Stop;
                break;
            case Speed.Half:
                m_speed = Speed.Slow;
                break;
            case Speed.Full:
                m_speed = Speed.Half;
                break;
            case Speed.Back:
                break;
        }
    }

    private void FixedUpdate()
    {
        Jotunn.Logger.LogInfo($"{m_moveDir.x} {m_moveDir.y} {m_moveDir.z}");
        bool flag = HaveControllingPlayer();
        UpdateControlls(Time.fixedDeltaTime);
        if ((bool)m_nview && !m_nview.IsOwner())
        {
            return;
        }
        if (!flag && (m_speed == Speed.Slow || m_speed == Speed.Back))
        {
            m_speed = Speed.Stop;
        }

        Vector3 zero = Vector3.zero;
        switch (m_speed)
        {
            case Speed.Slow:
                //zero += base.transform.forward * m_backwardForce * (1f - Mathf.Abs(m_rudderValue));
                break;
            case Speed.Back:
                //zero += -base.transform.forward * m_backwardForce * (1f - Mathf.Abs(m_rudderValue));
                break;
        }
        transform.position += transform.up * m_moveDir.y * 50f * Time.deltaTime;
    }

    private void UpdateControlls(float dt)
    {
        if (m_nview.IsOwner())
        {
            m_nview.GetZDO().Set("forward", (int)m_speed);
            return;
        }
        m_speed = (Speed)m_nview.GetZDO().GetInt("forward");
    }

    //private void UpdateOwner()
    //{
    //    if (m_nview.IsValid() && m_nview.IsOwner() && !(Player.m_localPlayer == null) && m_players.Count > 0 && !IsPlayerInBoat(Player.m_localPlayer))
    //    {
    //        long owner = m_players[0].GetOwner();
    //        m_nview.GetZDO().SetOwner(owner);
    //        ZLog.Log("Changing ship owner to " + owner);
    //    }
    //}

    //private void OnTriggerEnter(Collider collider)
    //{
    //    Player component = collider.GetComponent<Player>();
    //    if ((bool)component)
    //    {
    //        m_players.Add(component);
    //        ZLog.Log("Player onboard, total onboard " + m_players.Count);
    //        if (component == Player.m_localPlayer)
    //        {
    //            m_currentShips.Add(this);
    //        }
    //    }
    //}

    //private void OnTriggerExit(Collider collider)
    //{
    //    Player component = collider.GetComponent<Player>();
    //    if ((bool)component)
    //    {
    //        m_players.Remove(component);
    //        ZLog.Log("Player over board, players left " + m_players.Count);
    //        if (component == Player.m_localPlayer)
    //        {
    //            m_currentShips.Remove(this);
    //        }
    //    }
    //}

    //public bool IsPlayerInBoat(ZDOID zdoid)
    //{
    //    foreach (Player player in m_players)
    //    {
    //        if (player.GetZDOID() == zdoid)
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    //public bool IsPlayerInBoat(Player player)
    //{
    //    return m_players.Contains(player);
    //}

    //public bool HasPlayerOnboard()
    //{
    //    return m_players.Count > 0;
    //}

    private void OnDestroyed()
    {
        if (m_nview.IsValid() && m_nview.IsOwner())
        {
            Gogan.LogEvent("Game", "ShipDestroyed", base.gameObject.name, 0L);
        }
        //m_currentShips.Remove(this);
    }

    //public static Airship GetLocalShip()
    //{
    //    if (m_currentShips.Count == 0)
    //    {
    //        return null;
    //    }
    //    return m_currentShips[m_currentShips.Count - 1];
    //}

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

    public Speed GetSpeedSetting()
    {
        return m_speed;
    }

    private void RPC_RequestControl(long sender, ZDOID playerID)
    {
        if (m_nview.IsOwner())
        {
            if (GetUser() == playerID || !HaveValidUser())
            {
                Jotunn.Logger.LogInfo("Requesting airship control.");
                m_nview.GetZDO().Set("user", playerID);
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

        return true;
    }
}
