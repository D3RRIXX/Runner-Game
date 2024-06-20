using System;
using Game.StateMachine;
using Game.StateMachine.States;
using Infrastructure.ServiceLocator;
using UniRx;
using UniRx.Triggers;
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
		private readonly CompositeDisposable _disposables = new CompositeDisposable();
		
		private Rigidbody _rb;
		private IGameStateMachine _gameStateMachine;
		
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
			_gameStateMachine = AllServices.Container.GetSingle<IGameStateMachine>();

			_gameStateMachine.ObserveStateChangedTo<GameplayState>()
			                 .First()
			                 .Subscribe(_ => OnGameStarted())
			                 .AddTo(_disposables);
		}

		private void OnDestroy()
		{
			_disposables.Dispose();
		}

		private void OnGameStarted()
		{
			var stateChangedStream = _gameStateMachine.CurrentState.SkipLatestValueOnSubscribe().First(x => !(x is GameplayState));

			this.UpdateAsObservable()
			    .TakeUntil(stateChangedStream)
			    .Subscribe(_ => OnUpdate())
			    .AddTo(_disposables);

			this.FixedUpdateAsObservable()
			    .TakeUntil(stateChangedStream)
			    .Subscribe(_ => OnFixedUpdate())
			    .AddTo(_disposables);

			stateChangedStream.Subscribe(_ => _rb.velocity = Vector3.zero).AddTo(_disposables);
		}

		private void OnUpdate()
		{
			if (Input.GetMouseButtonDown(0) && CanJump)
				_queueJump = true;
		}

		private void OnFixedUpdate()
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