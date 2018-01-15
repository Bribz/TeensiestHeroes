// ==================================
//
//  GameManager.cs
//
//  Author   :   PineCone
//  Date     :   12/7/2017
//
// ==================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Declaration_Station

    public TH_SceneManager SceneManager;
    public TH_UIManager UIManager;

    public TH_PlayerManager PlayerManager;
    public TH_UserConnection UserConnection;

    #if SERVER
    public TH_HitboxManager HitboxManager;
    #endif

    #endregion

    #region Singleton_Management
    public static GameManager instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            StartCoroutine(Init());
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        //No Touchy
    }

    #endregion

    private IEnumerator Init()
    {
        SceneManager = GetComponent<TH_SceneManager>();
        if(!SceneManager)
        {
            SceneManager = gameObject.AddComponent<TH_SceneManager>();
        }

        UIManager = GetComponent<TH_UIManager>();
        if (!UIManager)
        {
            UIManager = gameObject.AddComponent<TH_UIManager>();
        }

        PlayerManager = GetComponent<TH_PlayerManager>();
        if(!PlayerManager)
        {
            PlayerManager = gameObject.AddComponent<TH_PlayerManager>();
        }

        #if SERVER
        HitboxManager = GetComponent<TH_HitboxManager>();
        if(!HitboxManager)
        {
            HitboxManager = gameObject.AddComponent<TH_HitboxManager>();
        }
        #endif

        UserConnection = null;

        //Initialize Order
        instance.SceneManager.Initialize();
        instance.UIManager.Initialize();
        instance.PlayerManager.Initialize();

        #if SERVER
        instance.HitboxManager.Initialize();
        #endif

        instance.SceneManager.LoadScene(1, false, false);
        
        yield return null;
    }
}
