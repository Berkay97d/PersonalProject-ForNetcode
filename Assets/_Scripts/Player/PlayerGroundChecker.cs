using System;
using UnityEngine;


public class PlayerGroundChecker : MonoBehaviour
{
    [SerializeField] private Transform _feet;
    [SerializeField] private LayerMask _groundLayer;

    private static readonly Collider[] HİTS;
    private bool m_IsGrounded = true;


    private void Update()
    {
        CheckIsGrounded();
    }
    
    private void CheckIsGrounded()
    {
        m_IsGrounded = Physics.CheckBox(_feet.position, Vector3.one * 0.1f,  Quaternion.identity, _groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_feet.position, Vector3.one * 0.1f);
    }

    public bool GetIsGrounded()
    {
        return m_IsGrounded;
    }

   

}
