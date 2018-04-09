using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterAbilityHandler : MonoBehaviour, IHandler
{
    private Hitbox p_Hitbox;
    private CounterAbility Current_Ability;

    /// <summary>
    /// Do not use. Implementation of interface. Use overloaded Initialize method instead
    /// </summary>
    public void Initialize() { Log.Error("Using wrong method for Initialization!", 33); }

    public void Initialize(CounterAbility abilityData)
    {
        Current_Ability = abilityData;
        p_Hitbox = null;
    }

    public void Activate()
    {
        if (Current_Ability.Hitbox != null)
        {
#if SERVER
            Current_Ability.PlayerEffect.InvincibilityDuration = Current_Ability.Hitbox.Hitbox_Linger;
            Current_Ability.PlayerEffect.IsInvincible = true;

            p_Hitbox = GameManager.instance.HitboxManager.MakeHitbox(
                Current_Ability.Hitbox,
                transform.position + gameObject.transform.forward,
                1f,
                transform.gameObject);
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
                    if (obj != null)
                    {
                        //If we make the counter reflect damage, or give it a follow-up skill, handle it here.
                        //Currently, we are applying a crowd control effect when they hit the player.

                        if(obj.tag.Equals("Hitbox"))
                        {
                            //obj.GetComponent<Hitbox>().DATA.Owner_Controller.AddCrowdControl();
                        }
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
                    Cleanup();
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
        //Current_Ability.PlayerEffect.InvincibilityDuration = 0;
        Current_Ability.PlayerEffect.IsInvincible = false;
    }

    public void Cleanup()
    {
        //Current_Ability.PlayerEffect.InvincibilityDuration = 0;
        Current_Ability.PlayerEffect.IsInvincible = false;
    }
}
