using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityObj
{
    public Vector3 Velocity { get; set; }
    public int ID { get; private set; }

    public VelocityObj(int vID, Vector3 vInput)
    {
        ID = vID;

        if(vInput == null)
        {
            vInput = Vector3.zero;
        }
    }
}
