using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Linq;

/// <summary>
/// Editor tool for assigning Script Define symbols via a click.
/// </summary>
public static class AssignScriptDefines {

    #region Predefined_Define_Arrays
    private static readonly string[] DEBUG_DEFINES =
        {
        "DEBUG_VERBOSE"
        };
    private static readonly string[] RELEASE_DEFINES =
        {
        "DEBUG_ERROR"
        };
    private static readonly string[] BUILD_DEFINES =
        {
        };
    private static readonly string[] SERVER_BUILD_DEFINES =
        {
        "SERVER"
        };

    #endregion



    [MenuItem("Helper/DEBUG", false, 1)]
    public static void AssignDebug()
    {
        AssignScriptDefine(DEBUG_DEFINES);
        Debug.Log("[EDITOR] : Scripting Defines Set to <DEBUG>");
    }

    [MenuItem("Helper/RELEASE", false, 51)]
    public static void AssignRelease()
    {
        AssignScriptDefine(RELEASE_DEFINES);
        Debug.Log("[EDITOR] : Scripting Defines Set to <RELEASE>");
    }

    [MenuItem("Helper/BUILD", false, 101)]
    public static void AssignBuild()
    {
        AssignScriptDefine(BUILD_DEFINES);
        Debug.Log("[EDITOR] : Scripting Defines Set to <BUILD>");
    }
    [MenuItem("Helper/SERVER_BUILD", false, 101)]
    public static void AssignServerBuild()
    {
        AssignScriptDefine(BUILD_DEFINES);
        Debug.Log("[EDITOR] : Scripting Defines Set to <SERVER_BUILD>");
    }

    /// <summary>
    /// The magic. Remove Scripting Defines
    /// </summary>
    /// <param name="inputDefines">Input define strings</param>
    private static void AssignScriptDefine(string[] inputDefines)
    {
        string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        List<string> allDefines = definesString.Split(';').ToList();
        allDefines.AddRange(inputDefines.Except(allDefines));
        PlayerSettings.SetScriptingDefineSymbolsForGroup(
            EditorUserBuildSettings.selectedBuildTargetGroup,
            string.Join(";", allDefines.ToArray()));
    }
}
