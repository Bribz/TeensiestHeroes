using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TH_SceneManager : IManager
{
    public int m_CurrentSceneID;
    public delegate void SceneChanged(int sceneID, ChangeEvent_SceneType sceneType);
    public static event SceneChanged OnSceneChange;

    //TEMPORARY
    private bool gameLoaded = false;

    internal override bool Initialize()
    {
        m_CurrentSceneID = 0;

        return true;
    }

    //TEMPORARY. Network manager doesnt call sceneChanged
    internal override void Update()
    {
        if(!gameLoaded && SceneManager.GetActiveScene().buildIndex == 2)
        {
            gameLoaded = true;
            OnSceneChange(2, ChangeEvent_SceneType.GAME);
        }
    }

    /// <summary>
    /// Load Scene. Probably temporary.
    /// </summary>
    /// <param name="sceneID">Build Order Scene ID</param>
    /// <param name="async">Asynchronous Load</param>
    /// <param name="additive">Additive Load</param>
    /// <returns>Operation if async. Null for non async</returns>
    public AsyncOperation LoadScene(int sceneID, bool async, bool additive)
    {
        AsyncOperation operation = null;

        if (!async)
        {
            SceneManager.LoadScene(sceneID, additive ? LoadSceneMode.Additive : LoadSceneMode.Single);
            m_CurrentSceneID = sceneID;
        }
        else
        {
            operation = SceneManager.LoadSceneAsync(sceneID, additive ? LoadSceneMode.Additive : LoadSceneMode.Single);
            m_CurrentSceneID = sceneID;
        }

        bool isGame = sceneID >= 2;
        OnSceneChange(sceneID, isGame ? ChangeEvent_SceneType.GAME : ChangeEvent_SceneType.MENU);

        return operation;
    }
}
