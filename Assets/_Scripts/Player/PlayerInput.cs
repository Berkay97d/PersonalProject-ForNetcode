using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInput : MonoBehaviour
{
    private PlayerInputActions m_PlayerInputActions;

    public event Action OnTryJump;
    
    private void Awake()
    {
        m_PlayerInputActions = new PlayerInputActions();
        m_PlayerInputActions.Player.Enable();
    }

    private void Start()
    {
        m_PlayerInputActions.Player.Jump.performed += OnJumpPerformed;
    }

    private void OnDestroy()
    {
        m_PlayerInputActions.Player.Jump.performed -= OnJumpPerformed;
    }

    private void OnJumpPerformed(InputAction.CallbackContext obj)
    {
        OnTryJump?.Invoke();
    }
    
}
