using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStage_Handler : MonoBehaviour
{
    #region Declaration Station
    private const int NUM_ICONS = 6;

    private Transform HPBarBase;
    private Transform Compass;
    private Transform[] Icons;
    private TextMesh[] TextMeshes;
    private Material[] CooldownWheels;
    private float[] MaxAbilityTimes;
    public Material BaseCooldownWheelMaterial;
    public Texture2D SkillNullTexture;

    private Player ClientPlayer;
    private AttackHandler ClientPlayerAttackHandler;
    #endregion

    private void Awake()
    {
        HPBarBase = transform.Find("HP_Bar");

        Compass = transform.Find("Compass");
        Icons = new Transform[NUM_ICONS];
        Icons[0] = transform.Find("Icon[MH1]");
        Icons[1] = transform.Find("Icon[MH2]");
        Icons[2] = transform.Find("Icon[OH1]");
        Icons[3] = transform.Find("Icon[Class]");
        Icons[4] = transform.Find("Icon[Tool]");
        Icons[5] = transform.Find("Icon[Dash]");

        TextMeshes = new TextMesh[NUM_ICONS];
        TextMeshes[0] = Icons[0].GetComponentInChildren<TextMesh>();
        TextMeshes[1] = Icons[1].GetComponentInChildren<TextMesh>();
        TextMeshes[2] = Icons[2].GetComponentInChildren<TextMesh>();
        TextMeshes[3] = Icons[3].GetComponentInChildren<TextMesh>();
        TextMeshes[4] = Icons[4].GetComponentInChildren<TextMesh>();
        TextMeshes[5] = Icons[5].GetComponentInChildren<TextMesh>();

        CooldownWheels = new Material[6];
        int i = 0;
        foreach(var icon in Icons)
        {
            var CooldownWheel = icon.transform.Find("CooldownWheel");
            CooldownWheel.GetComponent<Renderer>().material = new Material(BaseCooldownWheelMaterial);
            CooldownWheels[i] = CooldownWheel.GetComponent<Renderer>().material;
            i++;
        }

        MaxAbilityTimes = new float[NUM_ICONS];
    }

    private void Update()
    {
        if (!GameManager.instance.PlayerManager.CLIENT_PLAYER) return;

        if(!ClientPlayer)
        {
            ClientPlayer = GameManager.instance.PlayerManager.CLIENT_PLAYER;
        }
        if (!ClientPlayer) return;
        else
        {
            ClientPlayerAttackHandler = ClientPlayer.GetComponent<AttackHandler>();
            ClientPlayerAttackHandler.OnCooldown += ClientPlayerAttackHandler_OnCooldown;
            ClientPlayerAttackHandler.OnWeaponUpdate += ClientPlayerAttackHandler_OnWeaponUpdate;
            //Force UI Update
            UpdateSkillIcons(
                ClientPlayerAttackHandler.MainHand_1, 
                ClientPlayerAttackHandler.MainHand_2, 
                ClientPlayerAttackHandler.OffHand_1, 
                ClientPlayerAttackHandler.Class, 
                ClientPlayerAttackHandler.Tool, 
                ClientPlayerAttackHandler.Dash);
            //
        }

        UpdateCooldowns();
    }

    private void UpdateCooldowns()
    {
        for(int i = 0; i < CooldownWheels.Length; i++)
        {
            float currentCD = ClientPlayerAttackHandler.Cooldowns[i];
            if (currentCD > 0)
            {
                TextMeshes[i].text = string.Format("{0:0.0}", currentCD);
                CooldownWheels[i].SetFloat("_Cutoff", ((MaxAbilityTimes[i] - currentCD) / MaxAbilityTimes[i]));
            }
            else if(currentCD < -998)
            {
                TextMeshes[i].text = "∞";
            }
            else
            {
                CooldownWheels[i].SetFloat("_Cutoff", 1);
                TextMeshes[i].text = "";
            }
        }
    }


    private void ClientPlayerAttackHandler_OnWeaponUpdate(WeaponObject MainHand, WeaponObject OffHand)
    {
        if(MainHand != null)
        {
            Icons[0].GetComponent<Renderer>().material.SetTexture("_MainTex", MainHand.MainHand_1.Ability_Icon);
            Icons[1].GetComponent<Renderer>().material.SetTexture("_MainTex", MainHand.MainHand_2.Ability_Icon);
        }
        else
        {
            Icons[0].GetComponent<Renderer>().material.SetTexture("_MainTex", SkillNullTexture);
            Icons[1].GetComponent<Renderer>().material.SetTexture("_MainTex", SkillNullTexture);
        }

        if(OffHand != null)
        {
            Icons[2].GetComponent<Renderer>().material.SetTexture("_MainTex", OffHand.OffHand_1.Ability_Icon);
        }
        else
        {
            Icons[2].GetComponent<Renderer>().material.SetTexture("_MainTex", SkillNullTexture);
        }
    }

    private void UpdateSkillIcons(
        IAbility Primary1   = null, 
        IAbility Primary2   = null, 
        IAbility Offhand    = null, 
        IAbility Class      = null, 
        IAbility Tool       = null, 
        IAbility Dash       = null)
    {
        if(Primary1)
        {
            Icons[0].GetComponent<Renderer>().material.SetTexture("_MainTex", Primary1.Ability_Icon);
        }
        else Icons[0].GetComponent<Renderer>().material.SetTexture("_MainTex", SkillNullTexture);

        if(Primary2)
        {
            Icons[1].GetComponent<Renderer>().material.SetTexture("_MainTex", Primary2.Ability_Icon);
        }
        else Icons[1].GetComponent<Renderer>().material.SetTexture("_MainTex", SkillNullTexture);

        if (Offhand)
        {
            Icons[2].GetComponent<Renderer>().material.SetTexture("_MainTex", Offhand.Ability_Icon);
        }
        else Icons[2].GetComponent<Renderer>().material.SetTexture("_MainTex", SkillNullTexture);

        if (Class)
        {
            Icons[3].GetComponent<Renderer>().material.SetTexture("_MainTex", Class.Ability_Icon);
        }
        else Icons[3].GetComponent<Renderer>().material.SetTexture("_MainTex", SkillNullTexture);

        if (Tool)
        {
            Icons[4].GetComponent<Renderer>().material.SetTexture("_MainTex", Tool.Ability_Icon);
        }
        else Icons[4].GetComponent<Renderer>().material.SetTexture("_MainTex", SkillNullTexture);

        if (Dash)
        {
            Icons[5].GetComponent<Renderer>().material.SetTexture("_MainTex", Dash.Ability_Icon);
        }
        else Icons[5].GetComponent<Renderer>().material.SetTexture("_MainTex", SkillNullTexture);
    }

    private void ClientPlayerAttackHandler_OnCooldown(AbilityType type, float maxTime)
    {
        MaxAbilityTimes[(int)type] = maxTime;
        TextMeshes[(int)type].text = string.Format("{0:0.0}", maxTime);
        CooldownWheels[(int)type].SetFloat("_Cutoff", 0);
    }

    private void OnDisable()
    {
        if(ClientPlayerAttackHandler)
        {
            ClientPlayerAttackHandler.OnCooldown -= ClientPlayerAttackHandler_OnCooldown;
            ClientPlayerAttackHandler.OnWeaponUpdate -= ClientPlayerAttackHandler_OnWeaponUpdate;
        }
    }
}
