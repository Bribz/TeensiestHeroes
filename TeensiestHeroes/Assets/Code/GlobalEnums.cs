// ==================================
//
//  GLOBAL_ENUMERATIONS
//
// ==================================

public enum INITIALIZATION_ERRORCODE
{
    NONE                =   0x0,

    UNITY_INTERNAL      =   0x1,
    INPUT_MANAGER       =   0x2,
    NETWORK_MANAGER     =   0x3,

    UNKNOWN             =   0xF
}

public enum ChangeEvent_SceneType
{
    MENU = 0x0,
    GAME = 0x1,

    DEFAULT = 0xF
}

public enum HitboxType
{
    BOX = 0x0,
    CIRCLE = 0x1,
    CONE = 0x2
}

public enum DamageType
{
    Physical,
    Magical,
    True
}

public enum ScreenEffectType
{
    POISON,
    BLIND,
    LIGHT_BLIND,
    BLEED,
    STAGGER,
    CONFUSE,
    SLOW
}

public enum DashType
{
    Light,
    Medium,
    Heavy,
    None
}

public enum AbilityType
{
    MainHand_Primary        = 0x0,
    MainHand_Secondary      = 0x1,
    OffHand                 = 0x2,
    Class                   = 0x3, 
    Tool                    = 0x4,
    Dash                    = 0x5,
    None                    = 0xE,
    All                     = 0xF
}

public enum WeaponType
{
    #region Physical

    //Melee
    Sword,
    Dagger,
    Shield,
    Axe,
    Spear,
    Blunt,

    //Ranged
    Crossbow,
    Thrown,
    Whip,

    #endregion

    #region Magical

    //Melee
    Tome,
    Focus,
    Talisman,
    Runes,

    //Ranged
    Wand,
    Lantern,
    Orb,
    Tarot

    #endregion
}

public enum HitboxCallbackType
{
    OnEnter,
    OnExit,
    OnStay,
    OnDispose
}