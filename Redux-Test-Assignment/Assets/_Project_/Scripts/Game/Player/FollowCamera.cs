using Cinemachine;
using Game.StateMachine;
using Game.StateMachine.States;
using Infrastructure.ServiceLocator;
using UniRx;
using UnityEngine;

namespace Game.Player
{
	public class FollowCamera : MonoBehaviour
	{
		[SerializeField] private CinemachineVirtualCamera _virtualCamera;
		
		private IGameStateMachine _gameStateMachine;
		private Transform _player;

		public void Construct(Transform player)
		{
			_player = player;
		}

		private void Awake()
		{
			_gameStateMachine = AllServices.Container.GetSingle<IGameStateMachine>();
			
			_gameStateMachine.CurrentState
			                 .Select(x => x is GameplayState ? _player : null)
			                 .Subscribe(SetTarget)
			                 .AddTo(this);
		}

		private void SetTarget(Transform target) => _virtualCamera.Follow = _virtualCamera.LookAt = target;
	}
}