using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emote : IAbility
{
    public float Emote_Duration = 0f;

    public override void Activate()
    {
        throw new NotImplementedException();
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

    public override void Initialize()
    {
        throw new NotImplementedException();
    }

    public override void Initialize(AttackHandler atkHandler)
    {
        Log.Error("Do not use overloaded Initialize. Use Default Initialize instead.");
    }

    public override object CreateRTInstance()
    {
        var data = ScriptableObject.CreateInstance<Emote>() as Emote;
        
        data.Ability_Cooldown = Ability_Cooldown;
        data.Ability_Icon = Ability_Icon;
        data.Ability_Name = Ability_Name;
        data.Ability_Particles = Ability_Particles;
        data.Emote_Duration = Emote_Duration;

        return data;
    }
}
