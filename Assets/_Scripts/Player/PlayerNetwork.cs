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
        
        if (!IsOwner) return;
        if (!m_CanMove) return;

        var movementVector = new Vector3(_playerInput.GetMovementInput().x, 0, _playerInput.GetMovementInput().y);
        transform.position += movementVector *  _speed * Time.deltaTime;
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
        
        return _playerInput.GetMovementInput() != Vector2.zero;
    }
    
}
