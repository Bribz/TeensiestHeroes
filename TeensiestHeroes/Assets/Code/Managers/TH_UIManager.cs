using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TH_UIManager : IManager
{
    #region Declaration_Station

    private Camera m_3D_Camera;
    private Transform m_3D_UI_Set;
    private Canvas m_2D_UI_Set;
    private CanvasGroup m_Console;
    private CanvasGroup m_Log;

    #endregion

    internal override bool Initialize()
    {
        m_3D_Camera = GameObject.Find("UI_Camera").GetComponent<Camera>();

        m_3D_UI_Set = m_3D_Camera.transform.GetChild(0);
        m_3D_UI_Set.gameObject.SetActive(false);
        DontDestroyOnLoad(m_3D_Camera.gameObject);

        m_2D_UI_Set = GameObject.Find("2DCanvas").transform.GetComponent<Canvas>();
        DontDestroyOnLoad(m_2D_UI_Set.gameObject);

        m_Console = m_2D_UI_Set.transform.Find("Console").GetComponent<CanvasGroup>();
        m_Log = m_2D_UI_Set.transform.Find("Info").GetComponent<CanvasGroup>();

        TH_SceneManager.OnSceneChange += OnSceneChange;

        if(!m_3D_Camera || !m_3D_UI_Set || !m_2D_UI_Set)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void OnSceneChange(int sceneID, ChangeEvent_SceneType sceneType)
    {
        m_3D_UI_Set.gameObject.SetActive(sceneType == ChangeEvent_SceneType.GAME);
    }

    internal override void Update()
    {
        //Display Console 
        if(Input.GetKeyUp(KeyCode.F12))
        {
            m_Console.GetComponent<UI_Console>().Cleanup(m_Console.alpha == 0);
        }

        //Display info log
        if(Input.GetKeyUp(KeyCode.F3))
        {
            m_Log.alpha = m_Log.alpha == 1? 0:1;
        }
    }

    private void OnDisable()
    {
        TH_SceneManager.OnSceneChange -= OnSceneChange;
    }
}
