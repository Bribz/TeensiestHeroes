// ==================================
//
//  Player.cs
//
//  Author   :   PineCone
//  Date     :   1/1/2018
//
// ==================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;

public class Player : PlayerBehavior
{
    #region Declaration_Station
    private const float MoveSpeedMultiplier = 50f;
    private const float BaseMoveSpeed = 5f;
    private const float MaxStepOffset = .75f;

    private VelocityHandler m_VelHandler;
    private AttackHandler p_AttackHandler;
    private Rigidbody m_RigidBody;
    private Quaternion m_RotateTo;
    private Vector3 m_InputDirection;
    private Vector3 m_PreVelocity;
    private CameraController m_CamController;
    private bool initialized;
    #endregion

    /// <summary>
    /// All ForgeNetworking Classes Use this instead of Start() or Awake().
    /// </summary>
    protected override void NetworkStart()
    //private void Start()
    {
        //
        base.NetworkStart();
        //
        m_RotateTo = Quaternion.identity;
        m_InputDirection = Vector3.zero;
        m_PreVelocity = Vector3.zero;
        m_RigidBody = GetComponent<Rigidbody>();

        if(networkObject.IsOwner)
        {
            m_CamController = Camera.main.gameObject.GetComponent<CameraController>();
            m_CamController.SetFollowTarget(transform);
        }
        
        p_AttackHandler = GetComponent<AttackHandler>();
        m_VelHandler = new VelocityHandler();
        initialized = true;
    }

    public void SuperUpdate()
    {
        if (!initialized) return;

        if(networkObject.IsOwner)
        {
            HandleInput();
        }

        HandleMovement();
    }

    private void HandleInput()
    {
        m_InputDirection.Set(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        m_InputDirection.Normalize();
    }

    private void HandleMovement()
    {
        if (networkObject.IsOwner)
        {
            #region Deprecated
            //m_PreVelocity = m_InputDirection * BaseMoveSpeed * Time.deltaTime; // *MoveSpeedMultiplier

            //m_PreVelocity.Set(m_PreVelocity.x, m_RigidBody.velocity.y, m_PreVelocity.z);
            //m_RigidBody.velocity = m_PreVelocity;

            //bool forwardRayHit = CheckForwardRay(m_InputDirection);
            //Vector3 pointHeight = transform.position;
            //if(forwardRayHit)
            //{
            //    pointHeight = GetBlockHeight(m_InputDirection);

            //  if (pointHeight.y == transform.position.y || pointHeight == Vector3.zero)
            //  {
            //      m_PreVelocity += GetForwardHitNormal(m_InputDirection).normalized * BaseMoveSpeed * Time.deltaTime;
            //  }

            //  transform.position += (Vector3.up * (pointHeight.y - transform.position.y));
            //}
            #endregion
            
            transform.position += CalculateVelocity();
            networkObject.m_Position = transform.position;
            
            if (m_InputDirection != Vector3.zero)
            {
                m_RotateTo = Quaternion.LookRotation(m_InputDirection, Vector3.up);
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, m_RotateTo, (BaseMoveSpeed * 1.5f) * Time.deltaTime);
            networkObject.m_Rotation = transform.rotation;
        }
        else
        {
            transform.position = networkObject.m_Position;
            transform.rotation = networkObject.m_Rotation;
        }
    }

   
    
    private Vector3  CalculateVelocity()
    {
        Vector3 retVal;
        int numVelocities = 0;
        //Calculate Input Velocity;
        if (!p_AttackHandler.currentlyActing)
        {
            m_PreVelocity = m_InputDirection * BaseMoveSpeed * Time.deltaTime;
            numVelocities++;
        }
        else
        {
            m_PreVelocity = Vector3.zero;
        }
        retVal = m_PreVelocity;

        //Get Values from VelocityHandler. Assume all velocities are getting read.
        //TODO: Allow some velocities to get "cached". For now, grabbing all velocities to read.
        List<VelocityObj> vObjs = m_VelHandler.GetAsList();
        foreach(var vObj in vObjs)
        {
            retVal += (vObj.Velocity * Time.deltaTime);
            numVelocities++;
        }

        if(numVelocities != 0)
        {
            retVal /= numVelocities;
        }
        

        return retVal;
    }

    #region VelocityHandler Functionality
    public int AddVelocity(Vector3 input)
    {
        return m_VelHandler.Add(input);
    }

    public void SetVelocity(VelocityObj vObj)
    {
        m_VelHandler.Set(vObj.ID, vObj.Velocity);
    }

    public void SetVelocity(int ID, Vector3 input)
    {
        m_VelHandler.Set(ID, input);
    }

    public bool RemoveVelocity(int ID)
    {
        return m_VelHandler.Remove(ID);
    }

    public VelocityObj PopVelocity(int ID)
    {
        return m_VelHandler.Pop(ID);
    }
    #endregion

    #region Deprecated
    /*
    private bool CheckForwardRay(Vector3 Direction)
    {
        Ray forwardCollisionRay = new Ray(transform.position + (Vector3.up * .1f), Direction);

        bool retval = Physics.Raycast(forwardCollisionRay, .71f, LayerMask.NameToLayer("TERRAIN"));

        #if DEBUG_VERBOSE
            Debug.DrawLine(forwardCollisionRay.origin, transform.position + (forwardCollisionRay.direction * .71f), retval? Color.red : Color.green);
        #endif

        return retval;
    }

    private Vector3 GetForwardHitNormal(Vector3 Direction)
    {
        Ray forwardCollisionRay = new Ray(transform.position + (Vector3.up * .1f), Direction);
        RaycastHit hitInfo = new RaycastHit();

        bool retval = Physics.Raycast(forwardCollisionRay, out hitInfo, .75f, LayerMask.NameToLayer("TERRAIN"));

        return hitInfo.normal;
    }

    private Vector3 GetBlockHeight(Vector3 Direction)
    {
        Ray forwardGrabRay = new Ray(transform.position + (Direction * .75f) + Vector3.up * MaxStepOffset, Direction + Vector3.down);
        
        RaycastHit grabRayInfo = new RaycastHit();
        bool retval = Physics.Raycast(forwardGrabRay, out grabRayInfo, MaxStepOffset, LayerMask.NameToLayer("TERRAIN"));

        #if DEBUG_VERBOSE
            Debug.DrawLine(forwardGrabRay.origin, transform.position + forwardGrabRay.direction, retval ? Color.red : Color.green);
        #endif

        return grabRayInfo.point;
    }
    */
    #endregion
}
