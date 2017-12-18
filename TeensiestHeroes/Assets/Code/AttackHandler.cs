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
    public IAbility Dash;

    public bool currentlyActing = false;

    private void Awake()
    {

    }

    private void Initialize(WeaponObject Primary = null, WeaponObject Secondary = null)
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
    }

    public void Update()
    {

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!currentlyActing)
            {
                if(MainHand_1 != null)
                {
                    MainHand_1.Activate();
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
                    MainHand_2.Activate();
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
                if(OffHand_1 != null)
                {
                    OffHand_1.Activate();
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
                if(Class != null)
                {
                    Class.Activate();
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
                if(Dash != null)
                {
                    Dash.Activate();
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
                    Tool.Activate();
                }
                else
                {
                    Log.Msg("Skill Tool is null!");
                }
            }
        }
    }

}
