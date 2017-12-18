using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponAbility : IAbility
{
    public WeaponAbility Ability_Combo_Next;

    public abstract override void Activate();

    public abstract override void Callback();

    public abstract override void Cancel();

    public abstract override void Cleanup();

    public override void Initialize()
    {
        Log.Error("Do not call the default Initialize, instead call the overloaded method.");
    }

    public abstract override void Initialize(AttackHandler atkHandler);
    
}
