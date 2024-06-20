using UniRx;
using UnityEngine;

namespace Game.Player
{
	[RequireComponent(typeof(Animator))]
	public class PlayerAnimator : MonoBehaviour
	{
		private PlayerMovement _movement;
		private Animator _animator;
		
		private static readonly int JUMP = Animator.StringToHash("Jump");
		private static readonly int JUMP_NUMBER = Animator.StringToHash("JumpNumber");
		private static readonly int RUN = Animator.StringToHash("Run");
		private static readonly int LAND = Animator.StringToHash("Land");

		private void Awake()
		{
			_animator = GetComponent<Animator>();
			_movement = GetComponentInParent<PlayerMovement>();
			
			_movement.JumpStream
			         .Subscribe(OnJump)
			         .AddTo(this);
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