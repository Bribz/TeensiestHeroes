using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base ability class. 
/// Apparently Unity doesn't like ScriptableObject classes being abstract if theyre getting made in the Editor, so something to note.
/// </summary>
[System.Serializable]
public abstract class IAbility : ScriptableObject
{
    public ParticleSystem[] Ability_Particles;
    public Texture2D Ability_Icon                   = null;
    public string Ability_Name                      = "";
    public float Ability_Cooldown                   = 0f;

    public abstract void Initialize();

    public abstract void Initialize(AttackHandler atkHandler);

    public abstract void Activate();

    public abstract void Callback();

    public abstract void Cancel();

    public abstract void Cleanup();
}
