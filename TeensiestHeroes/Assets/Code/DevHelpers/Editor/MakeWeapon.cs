using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MakeWeapon
{
    [MenuItem("Design/Create/Weapon")]
    public static void CreateWeapon()
    {
        WeaponObject asset = ScriptableObject.CreateInstance<WeaponObject>();
        AssetDatabase.CreateAsset(asset, "Assets/Database/Weapons/NewWeapon.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }

}
