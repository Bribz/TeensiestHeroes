using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponAbility : IAbility
{
    public WeaponAbility Ability_Combo_Next;

    public override void Activate()
    { }

    public override void Callback()
    { }

    public override void Cancel()
    { }

    public override void Cleanup()
    { }

    public override void Initialize()
    {
        Log.Error("Do not call the default Initialize, instead call the overloaded method.");
    }

    public override void Initialize(AttackHandler atkHandler)
    { }

    public override object CreateRTInstance()
    { return base.CreateRTInstance(); }
}
