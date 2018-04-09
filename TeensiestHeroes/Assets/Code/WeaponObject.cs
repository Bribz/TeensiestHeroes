using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class WeaponObject : ScriptableObject
{
    public WeaponType Weapon_Type;

    public WeaponAbility MainHand_1;
    public WeaponAbility MainHand_2;

    public WeaponAbility OffHand_1;
    
}
