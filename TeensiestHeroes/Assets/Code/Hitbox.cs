using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    #region Declaration_Station
    public HitboxData DATA;
    public delegate void HBCallback(HitboxCallbackType hb_callback_type, GameObject obj = null);
    public event HBCallback HitboxCallback;
    private float m_LingerTime = 0f;
    private bool m_ShouldLinger = false;
    #endregion

    /// <summary>
    /// Handles Initialization of new Hitbox. Called by TH_HitboxManager
    /// </summary>
    /// <param name="data">Hitbox Data</param>
    public void Initialize(HitboxData data)
    {
        DATA = data;
        m_LingerTime = data.Hitbox_Linger;
        if (m_LingerTime != 0f)
        {
            m_ShouldLinger = true;
        }
    }

    /// <summary>
    /// Called instead of Destroy. 
    /// </summary>
    public void Dispose()
    {
        if(HitboxCallback != null)
        {
            HitboxCallback(HitboxCallbackType.OnDispose);
        }

        m_ShouldLinger = false;
        gameObject.SetActive(false);
    }

    public void Update()
    {
        if(m_ShouldLinger)
        {
            m_LingerTime -= Time.deltaTime;
            if (m_LingerTime < 0)
            {
                Dispose();
            }
        }
    }

    #region Collisions
    private void OnCollisionEnter(Collision col)
    {
        if(HitboxCallback != null)
        {
            HitboxCallback(HitboxCallbackType.OnEnter, col.gameObject);
        }
    }

    private void OnCollisionStay(Collision col)
    {
        if (HitboxCallback != null)
        {
            HitboxCallback(HitboxCallbackType.OnStay, col.gameObject);
        }
    }

    private void OnCollisionExit(Collision col)
    {
        if (HitboxCallback != null)
        {
            HitboxCallback(HitboxCallbackType.OnExit, col.gameObject);
        }
    }
    #endregion

    #region Triggers
    private void OnTriggerEnter(Collider col)
    {
        if (HitboxCallback != null)
        {
            HitboxCallback(HitboxCallbackType.OnEnter, col.gameObject);
        }
    }
    
    private void OnTriggerStay(Collider col)
    {
        if (HitboxCallback != null)
        {
            HitboxCallback(HitboxCallbackType.OnStay, col.gameObject);
        }
    }
    
    private void OnTriggerExit(Collider col)
    {
        if (HitboxCallback != null)
        {
            HitboxCallback(HitboxCallbackType.OnExit, col.gameObject);
        }
    }
    #endregion
}
