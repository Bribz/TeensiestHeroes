using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleWeaponHandler : MonoBehaviour, IHandler
{
    private SimpleWeaponAbility Current_Ability;

    /// <summary>
    /// Do not use. Implementation of interface. Use overloaded Initialize method instead
    /// </summary>
    public void Initialize() { Log.Error("Using wrong method for Initialization!", 33); }

    public void Initialize(SimpleWeaponAbility abilityData)
    {
        Current_Ability = abilityData;
    }

    public void Activate()
    {
        
    }

    public void Callback()
    {
        
    }

    public void Cancel()
    {
        
    }

    public void Cleanup()
    {
        
    }
}
