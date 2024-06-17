using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MiniJam160.PostJam
{
	public class PersonMovement : MonoBehaviour
	{
		[SerializeField] JumpInfo _jump;
		[SerializeField] GroundInfo _ground;
		[SerializeField] MoveInfo _move;

		[Space(8)] 
		[SerializeField] Collider2D _collider;
		// [SerializeField] PlayerAnimations _anim;

		Rigidbody2D _rb;
		bool _isLocked;

		private void Awake()
		{
			_rb = GetComponent<Rigidbody2D>();
		}
		
		public void SetDesiredMove(float move)
		{
			_move.desiredVelocity = new Vector2(move * _move.maxSpeed, 0);

		}

		private void FixedUpdate()
		{
			_move.velocity = _rb.velocity;

			float acceleration = _ground.IsGrounded() ? _move.maxAcceleration : _move.maxAirAcceleration;
			float maxSpeedChange = acceleration * Time.deltaTime;

			if (_jump.isRequested)
			{
				_jump.isRequested = false;
				Jump();
			}
			
			_move.velocity.x = Mathf.MoveTowards(_move.velocity.x, _move.desiredVelocity.x, maxSpeedChange);
			_rb.velocity = _move.velocity;
			_ground.groundContactCount = 0;
		}

		public void SetJumpRequested(bool jumpPressed)
		{
			_jump.isRequested |= jumpPressed;
		}
		
		private void Jump()
		{
			if (_ground.IsGrounded())
			{
				_move.velocity.y += _jump.Speed;
			}
		}
		
		private void OnCollisionEnter2D(Collision2D collision) 
		{
			EvaluateCollision(collision);
		}
		
		private void OnCollisionStay2D(Collision2D collision) 
		{
			EvaluateCollision(collision);
		}
		
		private void EvaluateCollision (Collision2D collision)
		{
			_ground.groundContactCount = 0;
			
			for (int i = 0; i < collision.contactCount; i++) 
			{
				Vector2 normal = collision.GetContact(i).normal;
				
				if (normal.y >= 0.9f)
				{
					_ground.groundContactCount ++;
				}
					
			}
		}
	}
}