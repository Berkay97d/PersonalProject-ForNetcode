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

    private bool m_CanMove;
    

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
        SetCanMove(_playerGroundChecker.GetIsGrounded());
        Debug.Log(GetIsMoving());
        
        if (!IsOwner) return;
        if (!m_CanMove) return;
            
        transform.position += _playerInput.GetMovementInput() * Vector3.right * _speed * Time.deltaTime;
    }

    private void RaiseOnJump()
    {
        OnJump?.Invoke();
    }

    private void SetCanMove(bool canMove)
    {
        m_CanMove = canMove;
    }

    public bool GetIsMoving()
    {
        if (!m_CanMove)
        {
            return false;
        }
        
        return _playerInput.GetMovementInput() != 0;
    }
    
}
