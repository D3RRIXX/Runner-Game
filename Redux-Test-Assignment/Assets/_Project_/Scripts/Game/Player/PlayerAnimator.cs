using System;
using Game.StateMachine;
using Game.StateMachine.States;
using Infrastructure.ServiceLocator;
using UniRx;
using UnityEngine;

namespace Game.Player
{
	[RequireComponent(typeof(Animator))]
	public class PlayerAnimator : MonoBehaviour
	{
		private PlayerMovement _movement;
		private Animator _animator;
		private IGameStateMachine _gameStateMachine;
		
		private static readonly int JUMP = Animator.StringToHash("Jump");
		private static readonly int JUMP_NUMBER = Animator.StringToHash("JumpNumber");
		private static readonly int RUN = Animator.StringToHash("Run");
		private static readonly int LAND = Animator.StringToHash("Land");
		private static readonly int LEVEL_COMPLETE = Animator.StringToHash("LevelComplete");
		private static readonly int LEVEL_FAILED = Animator.StringToHash("LevelFailed");

		private void Awake()
		{
			_animator = GetComponent<Animator>();
			_movement = GetComponentInParent<PlayerMovement>();
			_gameStateMachine = AllServices.Container.GetSingle<IGameStateMachine>();

			_gameStateMachine.CurrentState
			                 .Subscribe(OnGameStateChanged)
			                 .AddTo(this);

			_movement.JumpStream
			         .Subscribe(OnJump)
			         .AddTo(this);
		}

		private void OnGameStateChanged(IExitableState state)
		{
			switch (state)
			{
				case GameplayState _:
					_animator.SetTrigger(RUN);
					break;
				case LevelCompleteState _:
					_animator.SetTrigger(LEVEL_COMPLETE);
					break;
				case LevelFailedState _:
					_animator.SetTrigger(LEVEL_FAILED);
					break;
				default:
					return;
			}
		}

		private void Start()
		{
			_animator.SetTrigger(RUN);
		}

		private void OnJump(int jumpNum)
		{
			if (jumpNum == 0)
			{
				_animator.SetTrigger(LAND);
				return;
			}
			
			_animator.SetTrigger(JUMP);
			_animator.SetInteger(JUMP_NUMBER, jumpNum);
		}
	}
}