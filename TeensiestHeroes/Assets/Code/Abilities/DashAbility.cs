using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class DashAbility : IAbility
{
    private Rewired.Player m_RWPlayer;
    private Transform PlayerBase;
    public PlayerEffect PlayerEffect;
    private DashHandler handler;
    public Vector3 InputDirection;
    public DashType DashType;
    public float DashDistanceModifier = 1.0f;
    public float DashSpeedModifier = 1.0f;

    public override void Activate()
    {
        handler.Activate(new Vector3(m_RWPlayer.GetAxis("Axis_H"), 0, m_RWPlayer.GetAxis("Axis_V")), DashSpeedModifier, DashDistanceModifier);
    }

    public override void Callback()
    {
        throw new NotImplementedException();
    }

    public override void Cancel()
    {
        throw new NotImplementedException();
    }

    public override void Cleanup()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// DO NOT CALL, DASH ABILITY TAKES MORE INPUT.
    /// </summary>
    public override void Initialize() { Log.Error("Called Invalid Initialize!", 39); }

    /// <summary>
    /// DO NOT CALL, DASH ABILITY TAKES MORE INPUT.
    /// </summary>
    /// <param name="atkHandler"> NULL </param>
    public override void Initialize(AttackHandler atkHandler) { Log.Error("Called Invalid Initialize!", 45); }

    public void Initialize(AttackHandler atkHandler, DashType dType)
    {
        m_RWPlayer = ReInput.players.GetPlayer(0);

        handler = atkHandler.GetComponent<DashHandler>();
        if (!handler)
        {
            handler = atkHandler.gameObject.AddComponent<DashHandler>();
        }

        handler.Initialize(atkHandler, dType);
    }

    
}
