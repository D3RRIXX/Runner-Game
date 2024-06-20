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

		public void Construct(Transform player)
		{
			_virtualCamera.Follow = player;
			_virtualCamera.LookAt = player;
		}

		private void Awake()
		{
			_gameStateMachine = AllServices.Container.GetSingle<IGameStateMachine>();

			_gameStateMachine.CurrentState.SkipLatestValueOnSubscribe()
			                 .First(x => !(x is GameplayState))
			                 .Subscribe(_ =>
			                 {
				                 _virtualCamera.Follow = null;
				                 _virtualCamera.LookAt = null;
			                 })
			                 .AddTo(this);
		}
	}
}