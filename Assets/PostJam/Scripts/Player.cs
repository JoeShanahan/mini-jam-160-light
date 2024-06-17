using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

namespace MiniJam160.PostJam
{
    [RequireComponent(typeof(PersonMovement))]
    public class Player : MonoBehaviour
    {
        private InputSystem_Actions _input;
        private Vector2 _moveInput;
        private bool _jumpPressed;
        PersonMovement _movement;

        void Start()
        {
            _movement = GetComponent<PersonMovement>();
            _input = new();
            _input.Enable();
            
            _input.Player.Jump.performed += JumpPressed;
            _input.Player.Interact.performed += InteractPressed;
        }

        private void Update()
        {
            HandlePlayerInput();
        }

        
        // Input Event
        private void JumpPressed(InputAction.CallbackContext ctx)
        {
            _jumpPressed = true;
        }

        // Input Event
        private void InteractPressed(InputAction.CallbackContext ctx)
        {

        }

        private void HandlePlayerInput()
        {
            _moveInput = _input.Player.Move.ReadValue<Vector2>();

            _movement.SetDesiredMove(_moveInput.x);
            _movement.SetJumpRequested(_jumpPressed);

            _jumpPressed = false;
        }
    }
}