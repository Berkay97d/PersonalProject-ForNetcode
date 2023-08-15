using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerGroundChecker playerGroundChecker;


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        playerInput.OnTryJump += OnTryJump;
    }

    public override void OnNetworkDespawn()
    {
        playerInput.OnTryJump -= OnTryJump;
    }
    
    private void OnTryJump()
    {
        if (!IsOwner) return;
        if (!playerGroundChecker.GetIsGrounded()) return;
        
        Jump();
    }

    private void Jump()
    {
        
    }
    
    
}
