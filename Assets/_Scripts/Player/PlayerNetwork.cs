using System;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private PlayerGroundChecker _playerGroundChecker;
    [SerializeField] private int _speed;
    
    public event Action OnJump;
    

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
        RaiseOnJump();
    }

    private void Move()
    {
        if (!IsOwner) return;
        
        transform.position += _playerInput.GetMovementInput() * Vector3.right * _speed * Time.deltaTime;
    }

    private void RaiseOnJump()
    {
        OnJump?.Invoke();
    }
    
}
