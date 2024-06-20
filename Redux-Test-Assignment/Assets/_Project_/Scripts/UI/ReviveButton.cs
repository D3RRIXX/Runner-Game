using Game.Player;
using Game.StateMachine;
using Game.StateMachine.States;
using Infrastructure.ServiceLocator;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	[RequireComponent(typeof(Button))]
	public class ReviveButton : MonoBehaviour
	{
		private IPlayerRespawnManager _playerRespawnManager;
		private IGameStateMachine _gameStateMachine;

		private void Awake()
		{
			_playerRespawnManager = AllServices.Container.GetSingle<IPlayerRespawnManager>();
			_gameStateMachine = AllServices.Container.GetSingle<IGameStateMachine>();
			
			GetComponent<Button>().onClick.AddListener(OnClick);
		}

		private void OnClick()
		{
			_playerRespawnManager.RespawnPlayer(withFullHealth: true);
			_gameStateMachine.Enter<GameplayState>();
		}
	}
}