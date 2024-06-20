using System.Collections.Generic;
using System.Linq;
using Game.StateMachine;
using Game.StateMachine.States;
using Infrastructure.ServiceLocator;
using UniRx;
using UnityEngine;

namespace UI
{
	public class UIManager : MonoBehaviour
	{
		[SerializeField] private UIScreen[] _screens;

		private Dictionary<UIScreenType, UIScreen> _screensMap;
		private IGameStateMachine _gameStateMachine;

		private void Reset()
		{
			_screens = GetComponentsInChildren<UIScreen>();
		}

		private void Awake()
		{
			_screensMap = _screens.ToDictionary(x => x.ScreenType, x => x);
			_gameStateMachine = AllServices.Container.GetSingle<IGameStateMachine>();

			_gameStateMachine.CurrentState
			                 .Select(GameStateToUIScreen)
			                 .Where(x => x != UIScreenType.None)
			                 .Subscribe(SwitchActiveScreen)
			                 .AddTo(this);
		}

		private void SwitchActiveScreen(UIScreenType screenType)
		{
			foreach (KeyValuePair<UIScreenType, UIScreen> pair in _screensMap)
			{
				pair.Value.gameObject.SetActive(pair.Key == screenType);
			}
		}

		private UIScreenType GameStateToUIScreen(IExitableState exitableState)
		{
			return exitableState switch
			{
				GameplayState _ => UIScreenType.HUD,
				LevelCompleteState _ => UIScreenType.LevelCompleted,
				LevelFailedState _ => UIScreenType.LevelFailed,
				_ => UIScreenType.None
			};
		}
	}
}