using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MakeAbility
{
    [MenuItem("Design/Create/Abilities/Simple")]
    public static void CreateDefaultAbility()
    {
        IAbility asset = ScriptableObject.CreateInstance<SimpleWeaponAbility>();
        AssetDatabase.CreateAsset(asset, "Assets/Database/Abilities/NewDefaultAbility.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }

    [MenuItem("Design/Create/Abilities/Emote")]
    public static void CreateEmoteAbility()
    {
        IAbility asset = ScriptableObject.CreateInstance<Emote>();
        AssetDatabase.CreateAsset(asset, "Assets/Database/Emotes/NewEmoteAbility.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }

    [MenuItem("Design/Create/Abilities/Dash")]
    public static void CreateDashAbility()
    {
        DashAbility asset = ScriptableObject.CreateInstance<DashAbility>();
        AssetDatabase.CreateAsset(asset, "Assets/Database/Dashes/NewDashAbility.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}
