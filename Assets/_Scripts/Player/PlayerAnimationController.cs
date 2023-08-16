
using System;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private PlayerNetwork _playerNetwork;
    [SerializeField] private Transform _animationTransform;
    
    

    private void Start()
    {
        _playerNetwork.OnJump += OnPlayerJump;
    }

    private void OnDestroy()
    {
        _playerNetwork.OnJump -= OnPlayerJump;
    }

    private void OnPlayerJump()
    {
        PlayJumpAnimation();
    }

    private void PlayJumpAnimation()
    {
        
    }
}