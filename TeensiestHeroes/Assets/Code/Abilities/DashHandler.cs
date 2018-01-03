using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashHandler : MonoBehaviour, IHandler
{
    public const float BASE_DASH_SPEED = 15f;
    public const float BASE_DASH_DISTANCE = 1.8f;
    private const float LIGHT_DASH_DISTANCE = 1.5f;
    private const float MEDIUM_DASH_DISTANCE = 1.25f;
    private const float HEAVY_DASH_DISTANCE = .6f;
    private const float DASH_INVINCIBLE_DURATION = .3f;
    private const float BLOCK_INVINCIBLE_DURATION = .2f;

    private Player p_PlayerHandler;
    private AttackHandler p_AttackHandler;
    private DashAbility m_DashAbility;
    private Coroutine m_DashCoroutine;
    private Transform m_PlayerBase;
    private Vector3 m_Direction;
    private float m_DistanceModifier;
    private float m_SpeedModifier;
    private float m_TimeLeft;
    private DashType m_DashType;

    #region Interface Methods

    /// <summary>
    /// Used by interface. Use overloaded methods.
    /// </summary>
    public void Initialize() { Log.Error("Using wrong method for Initialization!", 32); }
    /// <summary>
    /// Used by interface. Use overloaded methods.
    /// </summary>
    public void Activate() { Log.Error("Using wrong method for Activation!", 36); }

    #endregion

    public void Activate(Vector3 direction, float speedmodifier = 1.0f, float distancemodifier = 1.0f)
    {
        if(m_DashCoroutine == null)
        {
            m_Direction = direction;
            m_SpeedModifier = speedmodifier;
            m_DistanceModifier = distancemodifier;

            p_AttackHandler.currentlyActing = true;
            p_AttackHandler.currentlyDashing = true;

            //Check for Guarding instead of dashing
            if (direction.Equals(Vector3.zero))
            {
                m_DashCoroutine = StartCoroutine(HandleBlock());
            }
            //Dash
            else
            {
                m_DashCoroutine = StartCoroutine(HandleDash());
            }
            
        }
        else
        {
            Log.Error("Attempting to dash while already dashing!", 65);
        }
    }

    public void Callback()
    {
        m_DashCoroutine = null; 

        if(p_AttackHandler.currentlyActing)
        {
            p_AttackHandler.currentlyActing = false;
        }
        if(p_AttackHandler.currentlyDashing)
        {
            p_AttackHandler.currentlyDashing = false;
        }
    }

    public void Cancel()
    {
        //Can't cancel a dash.
    }

    public void Cleanup()
    {
        StopAllCoroutines();
    }

    public void Initialize(AttackHandler atkHandler, DashType dType)
    {
        p_PlayerHandler = atkHandler.transform.GetComponent<Player>();
        p_AttackHandler = atkHandler;
        m_PlayerBase = transform;
        m_DashType = dType;
        m_DashAbility = atkHandler.Dash;
        StartCoroutine(PostInitCallback());
    }

    /// <summary>
    /// Prevent Race condition.
    /// </summary>
    /// <returns></returns>
    private IEnumerator PostInitCallback()
    {
        yield return new WaitForSeconds(.05f);
        m_DashAbility.PlayerEffect = new PlayerEffect();
        yield return null;
    }

    private IEnumerator HandleBlock()
    {
        float timeStartedBlocking = 0f;
        while(Input.GetKey(KeyCode.LeftControl))
        {
            if(timeStartedBlocking < BLOCK_INVINCIBLE_DURATION)
            {
                if(m_DashAbility.PlayerEffect != null)
                {
                    m_DashAbility.PlayerEffect.IsInvincible = true;
                }
                    

                timeStartedBlocking += Time.deltaTime;
            }
            else
            {
                m_DashAbility.PlayerEffect.IsInvincible = false;
            }
            
            yield return null;
        }

        Callback();
        yield return null;
    }

    private IEnumerator HandleDash()
    {
        float dashTypeDistance = 0f;
        switch(m_DashType)
        {
            case DashType.Light:
                {
                    dashTypeDistance = LIGHT_DASH_DISTANCE;
                    break;
                }
            case DashType.Medium:
                {
                    dashTypeDistance = MEDIUM_DASH_DISTANCE;
                    break;
                }
            case DashType.Heavy:
                {
                    dashTypeDistance = HEAVY_DASH_DISTANCE;
                    break;
                }
            case DashType.None:
                {
                    Callback();
                    yield break;
                }
        }
        
        p_AttackHandler.currentlyDashing = true;
        
        float distanceTraveled = BASE_DASH_DISTANCE * (dashTypeDistance * m_DistanceModifier);
        float invincibilityStart = (distanceTraveled * .5f) + ((DASH_INVINCIBLE_DURATION * m_DistanceModifier) * .5f);

        VelocityObj MoveVelocity = null;
        while (distanceTraveled > 0)
        {
            Vector3 dashVelocity = m_Direction * m_SpeedModifier * BASE_DASH_SPEED;

            //Set Velocity On Player
            if(MoveVelocity != null)
            {
                MoveVelocity.Velocity = dashVelocity;
                p_PlayerHandler.SetVelocity(MoveVelocity);
            }
            else
            {
                int vObj_ID = p_PlayerHandler.AddVelocity(dashVelocity);
                MoveVelocity = new VelocityObj(vObj_ID, dashVelocity);
            }

            distanceTraveled -= Vector3.Magnitude(dashVelocity * Time.deltaTime);

            if(distanceTraveled < invincibilityStart)
            {
                //TODO: MAKE CHARACTER INVINCIBLE
            }
            else if(distanceTraveled < invincibilityStart + (DASH_INVINCIBLE_DURATION * m_DistanceModifier))
            {
                //TODO: MAKE CHARACTER NOT INVINCIBLE
            }
            yield return null;
        }

        if(!p_PlayerHandler.RemoveVelocity(MoveVelocity.ID))
        {
            Log.Error("Problem removing player velocity!", 189);
        }
        Callback();
    }
}
