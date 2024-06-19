using Infrastructure.ServiceLocator;
using UnityEngine;

namespace Game
{
	public class LevelTest : MonoBehaviour
	{
		private IGameFactory _gameFactory;

		private void Awake()
		{
			_gameFactory = AllServices.Container.GetSingle<IGameFactory>();
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
				_gameFactory.SpawnNextBlock();
		}
	}
}