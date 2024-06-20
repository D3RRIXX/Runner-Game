using Game.Factory;
using Infrastructure.ServiceLocator;
using UnityEngine;

namespace Game.Player
{
	public class PlayerSpawner : MonoBehaviour
	{
		[SerializeField] private Transform _spawnPoint;

		private async void Awake()
		{
			var gameFactory = AllServices.Container.GetSingle<IGameFactory>();
			await gameFactory.CreatePlayer(_spawnPoint.position);
		}
	}
}