using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LogLimited : MonoBehaviour
{
    private CanvasGroup m_canvasGroup;
    private Text m_FPSText;
    private Text m_PingText;
    private float m_deltaTime = 0f;
    private float m_latestPing = 0f;
    
    private void Awake()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();

        if (transform.GetChild(0).Find("Text_FPS"))
            m_FPSText = transform.GetChild(0).Find("Text_FPS").GetComponent<Text>();

        if (transform.GetChild(0).Find("Text_Ping"))
            m_PingText = transform.GetChild(0).Find("Text_Ping").GetComponent<Text>();
    }

	private void Update ()
    {

        if(m_canvasGroup.alpha == 1)
        {
            OnGUIUpdate();
        }
	}

    private void OnGUIUpdate()
    {
        if(m_FPSText)
        {
            m_deltaTime += (Time.unscaledDeltaTime - m_deltaTime) * 0.1f;
            float fps = 1.0f / m_deltaTime;
            m_FPSText.text = string.Format(" FPS: {0:0.}", fps);
        }

        if(m_PingText)
        {
            //TODO: Display Ping
            m_PingText.text = string.Format(" Ping: {0:0.}", m_latestPing);
        }
    }
}
