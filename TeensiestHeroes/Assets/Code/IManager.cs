// ==================================
//
//  IManager.cs
//
//  Author   :   PineCone
//  Date     :   12/7/2017
//
// ==================================

using UnityEngine;

/// <summary>
/// Abstract implementation of Manager class. Used by GameManager
/// </summary>
public abstract class IManager : MonoBehaviour
{
    #region NO_TOUCH
    private void Awake()
    {

    }

    private void Start()
    {

    }
    #endregion

    /// <summary>
    /// Initialization of Manager type. Called by GameManager.
    /// </summary>
    /// <returns>Initialize was successful</returns>
    internal abstract bool Initialize();

    /// <summary>
    /// Update called by MonoBehaviour. Feel free to adapt in base class.
    /// </summary>
    internal virtual void Update()
    {

    }

}
