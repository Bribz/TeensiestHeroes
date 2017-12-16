// ==================================
//
//  CameraController.cs
//
//  Author   :   PineCone
//  Date     :   12/7/2017
//
// ==================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera m_Camera;
    private Transform m_FollowTarget;
    [SerializeField]
    private Vector3 m_Offset;
    [SerializeField]
    private Vector3 m_FocusOffset;

    public void Awake()
    {
        m_Camera = GetComponent<Camera>();
    }

    public void SetFollowTarget(Transform target)
    {
        m_FollowTarget = target;
    }
	
	void LateUpdate ()
    {
        if(m_FollowTarget)
        {
            transform.position = m_FollowTarget.position + m_Offset;
            transform.LookAt(m_FollowTarget.position + m_FocusOffset);
        }
	}
}
