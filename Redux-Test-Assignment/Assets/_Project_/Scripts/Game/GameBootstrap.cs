using Game.Configs;
using Game.StateMachine;
using UnityEngine;

namespace Game
{
	public class GameBootstrap : MonoBehaviour
	{
		[SerializeField] private LevelList _levelList;

		private Application _application;

		private void Awake()
		{
			_application = new Application(_levelList);
			_application.StateMachine.Enter<BootstrapState>();

			DontDestroyOnLoad(this);
		}
	}
}