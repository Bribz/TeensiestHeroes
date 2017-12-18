using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CrowdControl : MonoBehaviour
{
    public Vector3 ForceDirection;
    public float ForcePower;
    public float Duration;

    public float DamageOverTime;
    public DamageType DamageType;

    public ScreenEffectType ScreenEffect;

    public bool CharacterCanMove;
    public bool CharacterCanUseAbilities;

}
