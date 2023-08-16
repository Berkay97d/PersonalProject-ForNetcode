using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private PlayerGroundChecker _playerGroundChecker;


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        _playerInput.OnTryJump += OnTryJump;
    }

    public override void OnNetworkDespawn()
    {
        _playerInput.OnTryJump -= OnTryJump;
    }

    private void Update()
    {
        Move();
    }

    private void OnTryJump()
    {
        if (!IsOwner) return;
        if (!_playerGroundChecker.GetIsGrounded()) return;
        
        Jump();
    }

    private void Jump()
    {
        Debug.Log("JUMPED");
    }

    private void Move()
    {
        Debug.Log(_playerInput.GetMovementInput());
    }
    
    
}
