using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbility : IAbility
{
    private Transform PlayerBase;
    public PlayerEffect PlayerEffect;
    private DashHandler handler;
    public Vector3 InputDirection;
    public DashType DashType;
    public float DashDistanceModifier = 1.0f;
    public float DashSpeedModifier = 1.0f;

    public override void Activate()
    {
        handler.Activate(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")), DashSpeedModifier, DashDistanceModifier);
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
        handler = atkHandler.GetComponent<DashHandler>();
        if (!handler)
        {
            handler = atkHandler.gameObject.AddComponent<DashHandler>();
        }

        handler.Initialize(atkHandler, dType);
    }

    
}
