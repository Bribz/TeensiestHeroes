using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using BeardedManStudios.Forge.Networking.Lobby;
using BeardedManStudios.SimpleJSON;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkPreload : MonoBehaviour
{
    internal void Awake()
    {
    #if SERVER
        GameManager.instance.UserConnection.Host();
    #else
        GameManager.instance.UserConnection.Connect();
    #endif
    }
}

