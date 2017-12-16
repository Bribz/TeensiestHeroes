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