using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInput : MonoBehaviour
{
    private PlayerInputActions playerInputActions;

    public event Action OnTryJump;
    
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    private void Start()
    {
        playerInputActions.Player.Jump.performed += OnJumpPerformed;
    }

    private void OnDestroy()
    {
        playerInputActions.Player.Jump.performed -= OnJumpPerformed;
    }

    private void OnJumpPerformed(InputAction.CallbackContext obj)
    {
        OnTryJump?.Invoke();
    }
    
}
