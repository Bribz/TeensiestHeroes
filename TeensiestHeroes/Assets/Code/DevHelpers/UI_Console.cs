using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Console : MonoBehaviour
{
    private InputField m_CommandInput;
    [SerializeField] private GameObject m_TextPrefab;
    private RectTransform m_ContentTransform;
    private CanvasGroup m_CanvasGroup;
    private string[] m_LastCommands;
    private int m_CommandIterator = 0;

    private void Awake()
    {
        m_CommandInput = transform.Find("CommandInput").GetComponent<InputField>();
        m_CanvasGroup = transform.GetComponent<CanvasGroup>();
        m_ContentTransform = transform.Find("Scroll View").Find("Viewport").Find("Content").GetComponent<RectTransform>();
        m_LastCommands = new string[3];
    }

    public void Update()
    {
        if(m_CanvasGroup.alpha == 1)
        {
            if (EventSystem.current.currentSelectedGameObject == m_CommandInput.gameObject)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    m_CommandIterator = 0;
                    HandleCommandEvent();
                    EventSystem.current.SetSelectedGameObject(m_CommandInput.gameObject);
                    m_CommandInput.ActivateInputField();
                }
                if(Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (m_CommandIterator < 2)
                    {
                        m_CommandIterator++;
                    }
                    m_CommandInput.text = m_LastCommands[m_CommandIterator];
                    m_CommandInput.ActivateInputField();
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (m_CommandIterator > 0)
                    {
                        m_CommandIterator--;
                    }
                    m_CommandInput.text = m_LastCommands[m_CommandIterator];
                    m_CommandInput.ActivateInputField();
                }
            }
        }
    }

    public void Cleanup(bool enabled)
    {
        if(enabled)
        {
            m_CanvasGroup.alpha = 1;
            m_CanvasGroup.interactable = true;
        }
        else
        {
            ClearConsole();
            m_CanvasGroup.alpha = 0;
            m_CanvasGroup.interactable = false;
        }
    }

    private void HandleCommandEvent()
    {
        StringBuilder s = new StringBuilder();
        s.Append('>');
        string Command = m_CommandInput.text.ToLower();
        m_CommandInput.text = "";
        s.Append(Command);
        s.Append("->");
        
        switch (Command)
        {
            case "quit":
                {
                    s.Append("Quitting Game in 3 seconds...");
                    StartCoroutine(CommandQuit());
                    break;
                }
            case "clear":
                {
                    ClearConsole();
                    s.Append("Clearing Console...");
                    break;
                }

            default:
                {
                    s.Append("Unknown Command!");
                    break;
                }
        }
        m_LastCommands[2] = m_LastCommands[1];
        m_LastCommands[1] = m_LastCommands[0];
        m_LastCommands[0] = Command;
        HandleConsoleOutput(s);
    }

    private IEnumerator CommandQuit()
    {
        yield return new WaitForSeconds(3);
        Application.Quit(); 
    }

    private void HandleConsoleOutput(StringBuilder strBuild)
    {
        GameObject textObj = GameObject.Instantiate(m_TextPrefab, m_ContentTransform);
        
        textObj.GetComponent<Text>().text = strBuild.ToString();
    }

    private void ClearConsole()
    {
        var children = new List<GameObject>();
        foreach (RectTransform child in m_ContentTransform.GetComponentsInChildren<RectTransform>())
        {
            if(child != m_ContentTransform)
            {
                children.Add(child.gameObject);
            }
        }
        children.ForEach(child => Destroy(child));
    }
}
