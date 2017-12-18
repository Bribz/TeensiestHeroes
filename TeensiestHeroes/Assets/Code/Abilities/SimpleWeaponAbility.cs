using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleWeaponAbility : WeaponAbility
{
    private SimpleWeaponHandler handler;
    private AttackHandler atkHandler;

    public HitboxData Hitbox;
    public PlayerEffect PlayerEffect;
    public float Hitbox_StartUpTime;
    public float PlayerEffect_StartUpTime;
    
    public override void Activate()
    {
        Log.Msg(string.Format("Player used skill[{0}]", Ability_Name));
        handler.Activate();
    }

    public override void Callback()
    {
        handler.Callback(); 
    }

    public override void Cancel()
    {
        Log.Msg(string.Format("Player cancelled skill[{0}]", Ability_Name));
        handler.Cancel();
    }

    public override void Cleanup()
    {
        handler.Cleanup();
    }

    public override void Initialize(AttackHandler atkHandler)
    {
        this.atkHandler = atkHandler;
        handler = atkHandler.gameObject.AddComponent<SimpleWeaponHandler>();
        handler.Initialize(this); 
    }
}
