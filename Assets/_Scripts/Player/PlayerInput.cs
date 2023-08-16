using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInput : MonoBehaviour
{ 
    public event Action OnTryJump;
    
    private PlayerInputActions m_PlayerInputActions;
    private InputAction m_Jump;
    private InputAction m_Move;
    private float m_MovementInput;
    
    
    private void OnEnable()
    {
        m_PlayerInputActions = new PlayerInputActions();
        m_PlayerInputActions.Player.Enable();
        
        m_Move = m_PlayerInputActions.Player.Move;
        m_Jump = m_PlayerInputActions.Player.Jump;
        
        m_Move.Enable();
        m_Jump.Enable();
    }

    private void OnDisable()
    {
        m_Move.Disable();
        m_Jump.Disable();
        m_PlayerInputActions.Player.Disable();
    }

    private void Start()
    {
        m_Jump.performed += OnJumpPerformed;
        m_Move.started += OnMoveStarted;
        m_Move.canceled += OnMoveCanceled;
    }
    
    private void OnDestroy()
    {
        m_Jump.performed -= OnJumpPerformed;
        m_Move.started -= OnMoveStarted;
        m_Move.canceled -= OnMoveCanceled;
    }

    private void OnMoveCanceled(InputAction.CallbackContext obj)
    {
        m_MovementInput = 0;
    }

    private void OnMoveStarted(InputAction.CallbackContext obj)
    {
        var movementVector = obj.ReadValue<Vector2>();

        m_MovementInput = movementVector.x;
    }
    
    private void OnJumpPerformed(InputAction.CallbackContext obj)
    {
        OnTryJump?.Invoke();
    }
    
    public float GetMovementInput()
    {
        return m_MovementInput;
    }
    
}
