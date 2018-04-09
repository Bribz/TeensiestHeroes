using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base ability class. 
/// Apparently Unity doesn't like ScriptableObject classes being abstract if theyre getting made in the Editor, so something to note.
/// </summary>
[System.Serializable]
public class IAbility : ScriptableObject
{
    public ParticleSystem[] Ability_Particles;
    public Texture2D Ability_Icon                   = null;
    public string Ability_Name                      = "";
    public float Ability_Cooldown                   = 0f;

    public virtual void Initialize()
    { }

    public virtual void Initialize(AttackHandler atkHandler)
    { }

    public virtual void Activate()
    { }

    public virtual void Callback()
    { }

    public virtual void Cancel()
    { }

    public virtual void Cleanup()
    { }

    public virtual object CreateRTInstance()
    { return null; }
}
