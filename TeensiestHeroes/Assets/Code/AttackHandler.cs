using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class AttackHandler : MonoBehaviour
{
    private Rewired.Player m_RWPlayer;
    private NetworkObject p_networkObject;
    [Header("Main Hand")]
    public IAbility MainHand_1;
    public IAbility MainHand_2;
    [Header("Offhand")]
    public IAbility OffHand_1;
    [Header("Class")]
    public IAbility Class;
    [Header("Tool")]
    public IAbility Tool;
    [Header("Dash")]
    public DashAbility Dash;

    public float[] Cooldowns { get; private set; }

    public delegate void CooldownEvent(AbilityType type, float maxTime);
    public event CooldownEvent OnCooldown;
    public delegate void WeaponUpdateEvent(WeaponObject MH, WeaponObject OH);
    public event WeaponUpdateEvent OnWeaponUpdate;

    public bool currentlyActing = false;
    public bool currentlyDashing = false;

    //TEMP: Placeholders.
    public WeaponObject Debug_SwordWeapon;
    public WeaponObject Debug_ShieldWeapon;
    public DashAbility Debug_Dash;

    private void Awake()
    {
        Cooldowns = new float[6];
        m_RWPlayer = ReInput.players.GetPlayer(0);
    }

    internal void SetNetworkObject(NetworkObject netObj)
    {
        p_networkObject = netObj;
    }

    public void Initialize(WeaponObject Primary = null, WeaponObject Secondary = null)
    {
        if(Primary != null)
        {
            MainHand_1 = Primary.MainHand_1;
            MainHand_2 = Primary.MainHand_2;

            MainHand_1.Initialize(this);
            MainHand_2.Initialize(this);
        }
        else
        {
            MainHand_1 = null;
            MainHand_2 = null;
        }

        if(Secondary != null)
        {
            OffHand_1 = Secondary.OffHand_1;

            OffHand_1.Initialize(this);
        }
        else
        {
            OffHand_1 = null;
        }

        if (Primary != null && Secondary != null)
        {
            //Class = WeaponDatabase.GetClassAbility(Primary, Secondary);
        }

        if(OnWeaponUpdate != null)
        OnWeaponUpdate(Primary, Secondary);
    }

    public void Initialize(DashAbility DashAbility)
    {
        Dash = DashAbility;

        Dash.Initialize(this, DashAbility.DashType);
    }

    public void Initialize(DashType dashType, float dashSpeedModifier = 1.0f, float dashDistanceModifier = 1.0f)
    {
        switch (dashType)
        {
            case DashType.None:
                {
                    Dash.Initialize(this, DashType.None );
                    break;
                }
            default:
                {
                    Dash.Initialize(this, dashType);
                    break;
                }
        }
    }
    
    /// <summary>
    /// Overload for easy ability cooldown setting. DO NOT USE WITH AbilityType.All or AbilityType.None!
    /// </summary>
    /// <param name="type">Ability Type</param>
    public void SetCooldown(AbilityType type)
    {
        switch (type)
        {
            case AbilityType.All:
            case AbilityType.None:
                {
                    Log.Error("Attempting to use wrong overload of SetCooldown!", 97);
                    break;
                }
            case AbilityType.MainHand_Primary:
                {
                    Cooldowns[(int)AbilityType.MainHand_Primary] = MainHand_1.Ability_Cooldown;
                    break;
                }
            case AbilityType.MainHand_Secondary:
                {
                    Cooldowns[(int)AbilityType.MainHand_Secondary] = MainHand_2.Ability_Cooldown;
                    break;
                }
            case AbilityType.OffHand:
                {
                    Cooldowns[(int)AbilityType.OffHand] = OffHand_1.Ability_Cooldown;
                    break;
                }
            case AbilityType.Class:
                {
                    Cooldowns[(int)AbilityType.Class] = Class.Ability_Cooldown;
                    break;
                }
            case AbilityType.Tool:
                {
                    Cooldowns[(int)AbilityType.Tool] = Tool.Ability_Cooldown;
                    break;
                }
            case AbilityType.Dash:
                {
                    Cooldowns[(int)AbilityType.Dash] = Dash.Ability_Cooldown;
                    break;
                }
        }
    }

    public void SetCooldown(AbilityType type, float time)
    {
        switch (type)
        {
            case AbilityType.None:
            {
                break;
            }
            case AbilityType.All:
            {
                for(int i = 0; i < Cooldowns.Length; i++)
                {
                    Cooldowns[i] = time;
                    OnCooldown((AbilityType)i, time);
                }
                break;
            }
            default:
            {
                Cooldowns[(int)type] = time;
                OnCooldown(type, time);
                break;
            }
        }

        
    }

    /// <summary>
    /// Called by server. Issues ability on Client.
    /// </summary>
    /// <param name="ID">Ability ID</param>
    internal void RPCAbility(byte ID)
    {
        switch (ID)
        {
            //Mainhand 1
            case 0x00:
                {
                    if(MainHand_1 != null)
                    {
                        MainHand_1.Activate();
                    }
                    break;
                }
            //Mainhand 2
            case 0x01:
                {
                    if(MainHand_2 != null)
                    {
                        MainHand_2.Activate();
                    }
                    break;
                }
            //Offhand
            case 0x02:
                {
                    if(OffHand_1 != null)
                    {
                        OffHand_1.Activate();
                    }
                    break;
                }
            //Class
            case 0x03:
                {
                    if(Class != null)
                    {
                        Class.Activate();
                    }
                    break;
                }
            //Tool
            case 0x04:
                {
                    if(Tool != null)
                    {
                        Tool.Activate();
                    }
                    break;
                }
            //Dash
            case 0x05:
                {
                    //Dont Positionally move character for dashes. 
                    //Just play animation and invincibility
                    break;
                }
        }
    }

    public void Update()
    {
        #region Cooldowns
        for (int i = 0; i < Cooldowns.Length; i++)
        {
            if(Cooldowns[i] > 0)
            {
                Cooldowns[i] -= Time.unscaledDeltaTime;
            }
            else if(Cooldowns[i] < 0 && Cooldowns[i] > -998)
            {
                Cooldowns[i] = 0;
            }
        }
        #endregion

        #region Inputs
        if (m_RWPlayer.GetButtonDown("MainHand_1"))
        {
            if (!currentlyActing)
            {
                if(MainHand_1 != null)
                {
                    if(Cooldown(AbilityType.MainHand_Primary))
                    {
                        SetCooldown(AbilityType.MainHand_Primary);
                        MainHand_1.Activate();
                        p_networkObject.SendRpc(Player.RPC_SEND_ABILITY, Receivers.All, (byte)0x0);
                    }
                }
                else
                {
                    Log.Msg("Skill MainHand_1 is null!");
                }
            }
        }

        if (m_RWPlayer.GetButtonDown("MainHand_2"))
        {
            if(!currentlyActing)
            {
                if(MainHand_2 != null)
                {
                    if(Cooldown(AbilityType.MainHand_Secondary))
                    {
                        SetCooldown(AbilityType.MainHand_Secondary);
                        MainHand_2.Activate();
                        p_networkObject.SendRpc(Player.RPC_SEND_ABILITY, Receivers.All, (byte)0x1);
                    }
                }
                else
                {
                    Log.Msg("Skill MainHand_2 is null!");
                }
            }
        }

        if (m_RWPlayer.GetButtonDown("OffHand_1"))
        {
            if(!currentlyActing)
            {
                if (OffHand_1 != null)
                {
                    if (Cooldown(AbilityType.OffHand))
                    {
                        SetCooldown(AbilityType.OffHand);
                        OffHand_1.Activate();
                        p_networkObject.SendRpc(Player.RPC_SEND_ABILITY, Receivers.All, (byte)0x2);
                    }
                }
                else
                {
                    Log.Msg("Skill OffHand_1 is null!");
                }
            }
        }

        if (m_RWPlayer.GetButtonDown("Class"))
        {
            if(!currentlyActing)
            {
                if(Class != null )
                {
                    if(Cooldown(AbilityType.Class))
                    {
                        SetCooldown(AbilityType.Class);
                        Class.Activate();
                        p_networkObject.SendRpc(Player.RPC_SEND_ABILITY, Receivers.All, (byte)0x3);
                    }
                }
                else
                {
                    Log.Msg("Skill Class is null!");
                }
            }
        }

        if (m_RWPlayer.GetButtonDown("Dodge"))
        {
            if(!currentlyActing)
            {
                if (Dash != null)
                {
                    if (Cooldown(AbilityType.Dash))
                    {
                        SetCooldown(AbilityType.Dash);
                        Dash.Activate();
                        p_networkObject.SendRpc(Player.RPC_SEND_ABILITY, Receivers.All, (byte)0x5);
                    }
                }
                else
                {
                    Log.Msg("Skill Dash is null!");
                }
            }
            
        }

        if(m_RWPlayer.GetButtonDown("Tool"))
        {
            if(!currentlyActing)
            {
                if(Tool != null)
                {
                    if (Cooldown(AbilityType.Tool))
                    {
                        SetCooldown(AbilityType.Tool);
                        Tool.Activate();
                        p_networkObject.SendRpc(Player.RPC_SEND_ABILITY, Receivers.All, (byte)0x4);
                    }
                }
                else
                {
                    Log.Msg("Skill Tool is null!");
                }
            }
        }
        #endregion

    }

    /// <summary>
    /// Check if ability cooldown is up
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private bool Cooldown(AbilityType type)
    {
        return Cooldowns[(int)type] == 0;
    }

}
