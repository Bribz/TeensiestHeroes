using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Linq;
using System;

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
    private static readonly string[] SERVER_DEBUG_DEFINES =
        {
        "SERVER",
        "DEBUG_VERBOSE"
        };
    private static readonly string[] SERVER_BUILD_DEFINES =
        {
        "SERVER"
        };

    #endregion



    [MenuItem("Helper/DEBUG", false, 1)]
    public static void AssignDebug()
    {
        AssignScriptDefine(DEBUG_DEFINES, true);
        Debug.Log(string.Format("[EDITOR] : Scripting Defines Set to <DEBUG> [{0}]", DateTime.Now.ToShortTimeString()));
    }

    [MenuItem("Helper/RELEASE", false, 51)]
    public static void AssignRelease()
    {
        AssignScriptDefine(RELEASE_DEFINES,true);
        Debug.Log(string.Format("[EDITOR] : Scripting Defines Set to <RELEASE> [{0}]", DateTime.Now.ToShortTimeString()));
    }

    [MenuItem("Helper/BUILD", false, 101)]
    public static void AssignBuild()
    {
        AssignScriptDefine(BUILD_DEFINES,true);
        Debug.Log(string.Format("[EDITOR] : Scripting Defines Set to <BUILD> [{0}]", DateTime.Now.ToShortTimeString()));
    }

    [MenuItem("Helper/SERVER_DEBUG", false, 151)]
    public static void AssignServerDebugBuild()
    {
        AssignScriptDefine(SERVER_DEBUG_DEFINES, true);
        Debug.Log(string.Format("[EDITOR] : Scripting Defines Set to <SERVER_DEBUG> [{0}]", DateTime.Now.ToShortTimeString()));
    }

    [MenuItem("Helper/SERVER_BUILD", false, 152)]
    public static void AssignServerBuild()
    {
        AssignScriptDefine(SERVER_BUILD_DEFINES, true);
        Debug.Log(string.Format("[EDITOR] : Scripting Defines Set to <SERVER_BUILD> [{0}]", DateTime.Now.ToShortTimeString()));
    }

    /// <summary>
    /// The magic. Remove Scripting Defines
    /// </summary>
    /// <param name="inputDefines">Input define strings</param>
    private static void AssignScriptDefine(string[] inputDefines, bool clear = false)
    {
        
        string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        List<string> allDefines = definesString.Split(';').ToList();
        if(!clear)
        {
            allDefines.AddRange(inputDefines.Except(allDefines));
        }
        else
        {
            allDefines.Clear();
            allDefines.AddRange(inputDefines);
        }
        PlayerSettings.SetScriptingDefineSymbolsForGroup(
            EditorUserBuildSettings.selectedBuildTargetGroup,
            string.Join(";", allDefines.ToArray()));
    }
}
