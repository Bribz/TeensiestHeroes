using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class VelocityHandler
{
    
    private Dictionary<int, VelocityObj> Velocities;
    private int CurrentKeyValue;
    public int currentCount { get; private set; }

    public VelocityHandler()
    {
        Velocities = new Dictionary<int, VelocityObj>();
        CurrentKeyValue = 0;
        currentCount = 0;
    }

    /// <summary>
    /// Add Velocity Object
    /// </summary>
    /// <param name="vInput">Vector3 object to add</param>
    /// <returns>ID in container</returns>
    public int Add(Vector3 vInput)
    {
        if(CurrentKeyValue+1 < int.MaxValue)
        {
            CurrentKeyValue++;
        }
        else
        {
            CurrentKeyValue = 0;
        }
        
        int retVal = CurrentKeyValue;

        VelocityObj vObj = new VelocityObj(retVal, vInput);
        Velocities.Add(vObj.ID, vObj);

        currentCount++;

        return retVal;
    }

    /// <summary>
    /// Set Vector3 Value of VelocityObj in container
    /// </summary>
    /// <param name="ID">VelocityObj ID</param>
    /// <param name="value">Value to set</param>
    public void Set(int ID, Vector3 value)
    {
        if (!Velocities.ContainsKey(ID))
        {
            Log.Error("ID of VelocityObj does not exist in container!", 45);
        }
        Velocities[ID].Velocity = value;
    }

    /// <summary>
    /// Get VelocityObject Value
    /// </summary>
    /// <param name="ID">ID of velocity</param>
    /// <returns></returns>
    public VelocityObj Get(int ID)
    {
        VelocityObj vObj;
        Velocities.TryGetValue(ID, out vObj);
        return vObj;
    }

    /// <summary>
    /// Remove Velocity from container
    /// </summary>
    /// <param name="ID">ID of velocity</param>
    /// <returns>Removal was successful</returns>
    public bool Remove(int ID)
    {
        currentCount--;
        return Velocities.Remove(ID);
    }

    /// <summary>
    /// Remove and return Velocity from container
    /// </summary>
    /// <param name="ID">ID of velocity</param>
    /// <returns>VelocityObject</returns>
    public VelocityObj Pop(int ID)
    {
        currentCount--;

        VelocityObj vObj;
        Velocities.TryGetValue(ID, out vObj);
        Velocities.Remove(ID);
        return vObj;
    }

    public List<VelocityObj> GetAsList()
    {
        return Velocities.Values.ToList();
    }

}
