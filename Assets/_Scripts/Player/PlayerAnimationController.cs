
using System;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerNetwork _playerNetwork;
    [SerializeField] private PlayerGroundChecker _playerGroundChecker;
    

    private void Start()
    {
        _playerNetwork.OnJump += OnPlayerJump;
    }

    private void OnDestroy()
    {
        _playerNetwork.OnJump -= OnPlayerJump;
    }

    private void Update()
    {
        ControlMovingAnimation();
    }

    private void OnPlayerJump()
    {
        PlayJumpAnimation();
    }
    
    private void ControlMovingAnimation()
    {
        if (_playerNetwork.GetIsMoving() && _playerGroundChecker.GetIsGrounded())
        {
            SetIsMoving(true);
            return;
        }

        SetIsMoving(false);
    }
    
    private void PlayJumpAnimation()
    {
        _animator.SetTrigger("Jump");
    }
    
    private void SetIsMoving(bool isMoving)
    {
        _animator.SetBool("isMoving", isMoving);
    }
}