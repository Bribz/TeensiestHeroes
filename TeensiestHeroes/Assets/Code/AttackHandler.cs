using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    [Header("Main Hand")]
    public IAbility MainHand_1;
    public IAbility MainHand_2;
    [Header("Offhand")]
    public IAbility OffHand_1;
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

    

    private void Awake()
    {
        Cooldowns = new float[6];
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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!currentlyActing)
            {
                if(MainHand_1 != null)
                {
                    if(Cooldown(AbilityType.MainHand_Primary))
                    {
                        SetCooldown(AbilityType.MainHand_Primary);
                        MainHand_1.Activate();
                    }
                }
                else
                {
                    Log.Msg("Skill MainHand_1 is null!");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if(!currentlyActing)
            {
                if(MainHand_2 != null)
                {
                    if(Cooldown(AbilityType.MainHand_Secondary))
                    {
                        SetCooldown(AbilityType.MainHand_Secondary);
                        MainHand_2.Activate();
                    }
                }
                else
                {
                    Log.Msg("Skill MainHand_2 is null!");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if(!currentlyActing)
            {
                if (OffHand_1 != null)
                {
                    if (Cooldown(AbilityType.OffHand))
                    {
                        SetCooldown(AbilityType.OffHand);
                        OffHand_1.Activate();
                    }
                }
                else
                {
                    Log.Msg("Skill OffHand_1 is null!");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if(!currentlyActing)
            {
                if(Class != null )
                {
                    if(Cooldown(AbilityType.Class))
                    {
                        SetCooldown(AbilityType.Class);
                        Class.Activate();
                    }
                }
                else
                {
                    Log.Msg("Skill Class is null!");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if(!currentlyActing)
            {
                if (Dash != null)
                {
                    if (Cooldown(AbilityType.Dash))
                    {
                        SetCooldown(AbilityType.Dash);
                        Dash.Activate();
                    }
                }
                else
                {
                    Log.Msg("Skill Dash is null!");
                }
            }
            
        }

        if(Input.GetKeyDown(KeyCode.T))
        {
            if(!currentlyActing)
            {
                if(Tool != null)
                {
                    if (Cooldown(AbilityType.Tool))
                    {
                        SetCooldown(AbilityType.Tool);
                        Tool.Activate();
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
