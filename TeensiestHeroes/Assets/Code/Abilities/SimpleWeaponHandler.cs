using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleWeaponHandler : MonoBehaviour, IHandler
{
    private Hitbox p_Hitbox;
    private SimpleWeaponAbility Current_Ability;

    /// <summary>
    /// Do not use. Implementation of interface. Use overloaded Initialize method instead
    /// </summary>
    public void Initialize() { Log.Error("Using wrong method for Initialization!", 33); }

    public void Initialize(SimpleWeaponAbility abilityData)
    {
        Current_Ability = abilityData;
        p_Hitbox = null;
    }

    public void Activate()
    {
        if (Current_Ability.Hitbox != null)
        {
            #if SERVER
            p_Hitbox = GameManager.instance.HitboxManager.MakeHitbox(
                Current_Ability.Hitbox, 
                transform.position + gameObject.transform.forward, 
                1f);
            p_Hitbox.HitboxCallback += HitboxCallback;
            #endif
        }
    }

    private void HitboxCallback(HitboxCallbackType hb_callback_type, GameObject obj = null)
    {
        switch (hb_callback_type)
        {
            case HitboxCallbackType.OnEnter:
                {
                    if(obj != null)
                    {
                        obj.GetComponent<EntityStats>().HEALTH -= p_Hitbox.DATA.Hitbox_Damage;
                    }
                    break;
                }
            case HitboxCallbackType.OnStay:
            {
                break;
            }
            case HitboxCallbackType.OnExit:
            {
                break;
            }
            case HitboxCallbackType.OnDispose:
            {
                p_Hitbox.HitboxCallback -= HitboxCallback;
                break;
            }
        }
    }

    public void Callback()
    {
        
    }

    public void Cancel()
    {
        
    }

    public void Cleanup()
    {
        
    }
}
