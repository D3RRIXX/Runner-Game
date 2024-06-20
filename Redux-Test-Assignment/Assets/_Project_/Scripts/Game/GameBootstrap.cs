using Game.Levels;
using Game.StateMachine.States;
using UnityEngine;

namespace Game
{
	public class GameBootstrap : MonoBehaviour
	{
		[SerializeField] private LevelGenerationConfig _levelGenerationConfig;

		private Application _application;

		private void Awake()
		{
			_application = new Application(_levelGenerationConfig);
			_application.StateMachine.Enter<BootstrapState>();

			DontDestroyOnLoad(this);
		}
	}
}