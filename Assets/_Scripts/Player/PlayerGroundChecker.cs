using System;
using UnityEngine;


public class PlayerGroundChecker : MonoBehaviour
{
    private bool m_IsGrounded = true;


    private void Update()
    {
        CheckIsGround();
    }

    private void CheckIsGround()
    {
        
    }

    public bool GetIsGrounded()
    {
        return m_IsGrounded;
    }


}
