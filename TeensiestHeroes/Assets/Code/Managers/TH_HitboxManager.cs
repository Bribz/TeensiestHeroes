using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TH_HitboxManager : IManager
{
    [SerializeField] private Material HitboxMaterial;
    [SerializeField] private Mesh BOX_FILTER;
    [SerializeField] private Mesh CIRCLE_FILTER;
    [SerializeField] private Mesh CONE_FILTER;

    private List<GameObject> GameObject_Pool;
    private const int OBJECT_POOL_ITEMS = 100;
    private bool SHOW_HITBOXES = true;

    internal override bool Initialize()
    {
        GameObject_Pool = new List<GameObject>();
        return true;
    }

    internal override void Update()
    {
        base.Update();
    }

    public void ShowHitboxes(bool value)
    {
        bool changed = (value != SHOW_HITBOXES);
        SHOW_HITBOXES = value;

        if(changed)
        {
            foreach (var obj in GameObject_Pool)
            {
                var renderer = obj.GetComponent<Renderer>();
                renderer.enabled = SHOW_HITBOXES;
            }
        }
    }

    public Hitbox MakeHitbox(HitboxData data, Vector3 Position, float scale)
    {
        GameObject obj = new GameObject();
        obj.transform.position = Position;
        obj.transform.rotation = Quaternion.identity;
        obj.transform.localScale = new Vector3(scale, scale, scale);

        obj.tag = "Hitbox";

        SetColliderAndRenderer(obj, data.Shape, data.isTrigger, data.ShouldHitSelf);
        var script = HandleHitboxScript(obj, data);

        GameObject_Pool.Add(obj);
        return script;
    }
    
    public Hitbox MakeHitbox(HitboxData data, Vector3 Origin, Vector3 Direction, float distance, float scale)
    {
        GameObject obj = new GameObject();
        obj.transform.position = Origin + (Direction * distance);
        obj.transform.rotation = Quaternion.LookRotation(Direction, Vector3.up);
        obj.transform.localScale = new Vector3(scale, scale, scale);

        obj.tag = "Hitbox";

        SetColliderAndRenderer(obj, data.Shape, data.isTrigger, data.ShouldHitSelf);
        var script = HandleHitboxScript(obj, data);

        GameObject_Pool.Add(obj);

        return script;
    }

    private Hitbox HandleHitboxScript(GameObject obj, HitboxData data)
    {
        var hbScript = obj.AddComponent<Hitbox>();
        hbScript.Initialize(data);
        return hbScript;
    }

    private void SetColliderAndRenderer(GameObject obj, HitboxType type, bool isTrigger, bool shouldHitSelf = false)
    {
        // Add collider to new hitbox object
        Mesh filter = BOX_FILTER;
        Collider hb_collider;
        switch (type)
        {
            case HitboxType.BOX:
                {
                    hb_collider = obj.AddComponent<BoxCollider>();
                    ((BoxCollider)hb_collider).center = Vector3.up * 0.5f;
                    hb_collider.isTrigger = isTrigger;

                    if (!shouldHitSelf)
                    {
                        Physics.IgnoreCollision(hb_collider, obj.GetComponent<Collider>());
                    }

                    //filter = BOX_FILTER;
                    break;
                }
            case HitboxType.CIRCLE:
                {
                    //TODO: Implement Later
                    hb_collider = obj.AddComponent<MeshCollider>();
                    hb_collider.isTrigger = isTrigger;

                    if (!shouldHitSelf)
                    {
                        Physics.IgnoreCollision(hb_collider, obj.GetComponent<Collider>());
                    }

                    filter = CIRCLE_FILTER;
                    break;
                }
            case HitboxType.CONE:
                {
                    //TODO: Implement Later
                    hb_collider = obj.AddComponent<MeshCollider>();
                    hb_collider.isTrigger = isTrigger;

                    if (!shouldHitSelf)
                    {
                        Physics.IgnoreCollision(hb_collider, obj.GetComponent<Collider>());
                    }

                    filter = CONE_FILTER;
                    break;
                }
        }
        
        //Add Renderer
        var MeshFilter = obj.AddComponent<MeshFilter>();
        MeshFilter.mesh = filter;
        var MeshRenderer = obj.AddComponent<MeshRenderer>();
        MeshRenderer.material = HitboxMaterial;

        if (!SHOW_HITBOXES)
        {
            MeshRenderer.enabled = false;
        }
    }
}
