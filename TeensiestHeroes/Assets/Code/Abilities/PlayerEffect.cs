using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect
{
    [Header("EffectCore")]
    public float Duration;

    [Header("DamageReduction")]
    public bool IsInvincible;
    public float InvincibilityDuration;
    public float Protection;
    public float ProtectionDuration;

    [Header("Displacement")]
    public Vector3 MoveDirection;
    public float MoveDuration;
    public float MovePower;
	
}
