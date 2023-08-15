using System;
using UnityEngine;


public class PlayerGroundChecker : MonoBehaviour
{
    private bool isGrounded;


    private void Update()
    {
        CheckIsGround();
    }

    private void CheckIsGround()
    {
        
    }

    public bool GetIsGrounded()
    {
        return isGrounded;
    }


}
