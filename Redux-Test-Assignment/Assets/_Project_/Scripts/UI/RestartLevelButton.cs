using Game.Levels;
using Game.Player;
using Game.StateMachine;
using Game.StateMachine.States;
using Infrastructure.ServiceLocator;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	[RequireComponent(typeof(Button))]
	public class RestartLevelButton : MonoBehaviour
	{
		private IGameStateMachine _gameStateMachine;

		private void Awake()
		{
			_gameStateMachine = AllServices.Container.GetSingle<IGameStateMachine>();
			
			GetComponent<Button>().onClick.AddListener(OnClick);
		}

		private void OnClick()
		{
			_gameStateMachine.Enter<LoadGameState>();
		}
	}
}