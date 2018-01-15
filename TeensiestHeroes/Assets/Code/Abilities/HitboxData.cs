using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HitboxData
{
    public ParticleSystem[] Particles;
    public HitboxType Shape;
    public float Hitbox_Linger;
    public int Hitbox_Damage;
    public CrowdControl[] Crowd_Control;
    public bool ShouldHitSelf;
    public bool isTrigger;
}
