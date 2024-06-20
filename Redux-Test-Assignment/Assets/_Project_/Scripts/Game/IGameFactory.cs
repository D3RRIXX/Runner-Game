using Cysharp.Threading.Tasks;
using Game.Configs;
using Infrastructure.ServiceLocator;
using UnityEngine;

namespace Game
{
	public interface IGameFactory : IService
	{
		UniTask WarmUp(LevelConfig levelConfig);
		UniTask CreateGameLevel();
		UniTask<GameObject> CreatePlayer(Vector3 at);
		void SpawnNextBlock();
	}
}