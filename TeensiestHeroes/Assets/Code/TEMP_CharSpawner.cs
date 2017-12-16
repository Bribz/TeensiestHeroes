using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;

public class TEMP_CharSpawner : MonoBehaviour
{

    public void Start()
    {
        StartCoroutine(SpawnPlayer());
    }

    private IEnumerator SpawnPlayer()
    {
        while(NetworkManager.Instance == null)
        {
            yield return null;
        }

        NetworkManager.Instance.InstantiatePlayer();

        yield return null;
    }
	
}
