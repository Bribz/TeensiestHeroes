using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using System;

public class TEMP_CharSpawner : MonoBehaviour
{

    public WeaponObject Debug_PrimaryWeapon;
    public WeaponObject Debug_SecondaryWeapon;
    public IAbility Debug_ToolAbility;
    public DashAbility Debug_DashAbility;

#if !SERVER
    public void Start()
    {
        StartCoroutine(SpawnPlayer());
        StartCoroutine(SpawnUserConnection());
    }

    private IEnumerator SpawnUserConnection()
    {
        while (NetworkManager.Instance == null)
        {
            yield return null;
        }

        //var pUC = NetworkManager.Instance.InstantiateUserConnection();

        //GameManager.instance.UserConnection = (TH_UserConnection) pUC;

        //GameManager.instance.UserConnection.Initialize();
    }

    private IEnumerator SpawnPlayer()
    {
        while(NetworkManager.Instance == null)
        {
            yield return null;
        }

        var pBh = NetworkManager.Instance.InstantiatePlayer(position:Vector3.zero + Vector3.up);

        if (Debug_PrimaryWeapon != null || Debug_SecondaryWeapon != null)
        {
            pBh.GetComponent<AttackHandler>().Initialize(Debug_PrimaryWeapon, Debug_SecondaryWeapon);
        }

        if (Debug_DashAbility != null)
        {
            pBh.GetComponent<AttackHandler>().Initialize(Debug_DashAbility);
        }

        Player ply = (Player) pBh;
        GameManager.instance.PlayerManager.SetClientPlayer(ply);

        yield return null;
    }
#endif
}
