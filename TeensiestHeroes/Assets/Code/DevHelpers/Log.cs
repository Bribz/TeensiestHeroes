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

        UnityEngine.Debug.LogError(error_msg);
        #endif
    }

}
