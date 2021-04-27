using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed = 40f;
    private CharacterController2D _controller;
    private Animator _animator;
    private float _horizontalMove = 0f;
    private bool _jump = false;
    
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");

    private void Start()
    {
        _controller = GetComponent<CharacterController2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        _animator.SetFloat(Speed, Mathf.Abs(_horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            _jump = true;
            _animator.SetBool(IsJumping, true);
        }
    }

    private void FixedUpdate()
    {
        _controller.Move(_horizontalMove * Time.fixedDeltaTime, _jump);
        _jump = false;
    }

    public void OnLanding()
    {
        _animator.SetBool(IsJumping, false);
    }
}
