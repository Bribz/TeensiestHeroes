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
}
