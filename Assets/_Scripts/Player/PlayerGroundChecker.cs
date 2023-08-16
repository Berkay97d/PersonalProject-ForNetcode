using System;
using UnityEngine;


public class PlayerGroundChecker : MonoBehaviour
{
    private bool m_İsGrounded;


    private void Update()
    {
        CheckIsGround();
    }

    private void CheckIsGround()
    {
        
    }

    public bool GetIsGrounded()
    {
        return m_İsGrounded;
    }


}
