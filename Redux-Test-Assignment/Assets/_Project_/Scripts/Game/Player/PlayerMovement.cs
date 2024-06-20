using System;
using UniRx;
using UnityEngine;

namespace Game.Player
{
	[RequireComponent(typeof(Rigidbody))]
	public class PlayerMovement : MonoBehaviour
	{
		[SerializeField] private float _moveSpeed;
		[SerializeField] private float[] _jumpHeights = new float[2];
		[Header("Ground Check")]
		[SerializeField] private float _groundCheckDistance;
		[SerializeField] private LayerMask _groundMask;

		private readonly ReactiveProperty<int> _currentJumpNumber = new ReactiveProperty<int>();
		private Rigidbody _rb;
		private bool _queueJump;
		private bool _isGrounded;
		private float _speedModifier = 1f;

		/// <summary>
		/// Observable that emits on player jump, containing current jump number
		/// </summary>
		public IObservable<int> JumpStream => _currentJumpNumber.SkipLatestValueOnSubscribe();

		private bool CanJump => _currentJumpNumber.Value == 0 ? _isGrounded : _currentJumpNumber.Value < AllowedJumps;
		private int AllowedJumps => _jumpHeights.Length;
		private static float Gravity => Physics.gravity.y;

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawRay(transform.position, Vector3.down * _groundCheckDistance);
		}

		private void Awake()
		{
			_rb = GetComponent<Rigidbody>();
			JumpStream.Subscribe(x => Debug.Log($"Current jump num is {x}")).AddTo(this);
		}

		private void Update()
		{
			if (Input.GetMouseButtonDown(0) && CanJump)
				_queueJump = true;
		}

		private void FixedUpdate()
		{
			_isGrounded = HasGroundUnderneath();
			if (_queueJump)
			{
				Jump();
				_queueJump = false;
			}
			else if (_currentJumpNumber.Value > 0 && _rb.velocity.y <= 0 && _isGrounded)
			{
				_currentJumpNumber.Value = 0;
			}
			
			MoveForward();
		}

		private bool HasGroundUnderneath() 
			=> Physics.CheckSphere(transform.position + Vector3.down * _groundCheckDistance, 0.15f, _groundMask);

		private void MoveForward()
		{
			Vector3 velocity = transform.forward * (_speedModifier * _moveSpeed);
			velocity.y = _rb.velocity.y;

			_rb.velocity = velocity;
		}

		private void Jump()
		{
			_currentJumpNumber.Value++;

			float jumpHeight = _jumpHeights[_currentJumpNumber.Value - 1];
			float jumpForce = Mathf.Sqrt(2 * -Gravity * jumpHeight) * _rb.mass;

			Vector3 velocity = _rb.velocity;
			_rb.velocity = new Vector3(velocity.x, 0f, velocity.y);
			_rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
		}

		public void SetSpeedModifier(float speedMultiplier) => _speedModifier = speedMultiplier;
	}
}