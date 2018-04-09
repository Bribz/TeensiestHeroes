﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleWeaponAbility : WeaponAbility
{
    internal SimpleWeaponHandler handler;
    internal AttackHandler atkHandler;

    public HitboxData Hitbox;
    public PlayerEffect PlayerEffect;
    public float Hitbox_StartUpTime;
    public float PlayerEffect_StartUpTime;
    
    public override void Activate()
    {
        Log.Msg(string.Format("Player used skill[{0}]", Ability_Name));
        if(handler)
        {
            handler.Activate();
        }
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

    /// <summary>
    /// Returns a Runtime Instance of the ability. Must be converted.
    /// </summary>
    /// <returns>Object of Ability. Convert to relevant type</returns>
    public override object CreateRTInstance()
    {
        SimpleWeaponAbility data = ScriptableObject.CreateInstance(typeof(SimpleWeaponAbility)) as SimpleWeaponAbility;

        if (Ability_Combo_Next != null)
        {
            SimpleWeaponAbility comboNext = Ability_Combo_Next.CreateRTInstance() as SimpleWeaponAbility;
            data.Ability_Combo_Next = comboNext;
        }

        data.Ability_Cooldown = Ability_Cooldown;
        data.Ability_Icon = Ability_Icon;
        data.Ability_Name = Ability_Name;
        data.Ability_Particles = Ability_Particles;

        data.Hitbox = Hitbox;
        data.Hitbox_StartUpTime = Hitbox_StartUpTime;
        data.PlayerEffect = PlayerEffect;
        data.PlayerEffect_StartUpTime = PlayerEffect_StartUpTime;
        
        return data;
    }
}
