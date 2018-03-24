// ==================================
//
//  Log.cs
//
//  Author   :   PineCone
//  Date     :   10/17/2017
//
// ==================================

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// A wrapper for Unity's log. Just a collection of static methods to make peoples lives easier.
/// </summary>
public class Log : MonoBehaviour
{
    public delegate void LogEvent(string msg, bool error);
    public static event LogEvent Log_Msg;

    /// <summary>
    /// Logs a message to Unity. NOTE: CALLING WITH LINE NUMBER FROM A COROUTINE WILL YIELD AN UNWANTED RESULT!
    /// </summary>
    /// <param name="_msg">Message to send</param>
    /// <param name="lineNum">Line number in question (Optional Parameter)</param>
    public static void Msg(string _msg, int lineNum = -1)
    {
        #if UNITY_EDITOR || DEBUG_VERBOSE
        string msg = "";
        if (lineNum != -1)
        {
            StackTrace sT = new StackTrace();

            var mth = sT.GetFrame(1).GetMethod();
            var cls = mth.ReflectedType.Name;

            msg += "[" + cls + " <" + lineNum + ">]: ";
        }
        msg += _msg;

        if(Log_Msg != null)
        {
            Log_Msg(msg, false);
        }
        UnityEngine.Debug.Log(msg);
        #endif
    }

    /// <summary>
    /// Logs an error to Unity. NOTE: CALLING WITH LINE NUMBER FROM A COROUTINE WILL YIELD AN UNWANTED RESULT!
    /// </summary>
    /// <param name="msg">Message to send</param>
    /// <param name="lineNum">Line Number in question (Optional Parameter)</param>
    public static void Error(string msg, int lineNum = -1)
    {
        #if UNITY_EDITOR || DEBUG_ERROR || DEBUG_VERBOSE
        string error_msg = "";
        
        if (lineNum != -1)
        {
            StackTrace sT = new StackTrace();

            var mth = sT.GetFrame(1).GetMethod();
            var cls = mth.ReflectedType.Name;

            error_msg += "[" + cls + " <" + lineNum + ">]: ";
        }

        error_msg += msg;

        if (Log_Msg != null)
        {
            Log_Msg(error_msg, true);
        }
        UnityEngine.Debug.LogError(error_msg);
        #endif
    }

}
