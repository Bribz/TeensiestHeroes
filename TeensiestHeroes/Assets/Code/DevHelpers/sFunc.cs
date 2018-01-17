using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class sFunc
{
    internal static T AddGetComponent<T>(GameObject _obj) where T : Component
    {
        T retObj = _obj.GetComponent<T>();
        if(retObj == null)
        {
            retObj = _obj.AddComponent<T>();
        }

        return retObj;
    }
}
