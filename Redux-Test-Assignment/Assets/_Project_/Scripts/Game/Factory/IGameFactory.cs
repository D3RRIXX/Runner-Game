using Cysharp.Threading.Tasks;
using Game.Levels;
using Infrastructure.ServiceLocator;
using UnityEngine;

namespace Game.Factory
{
	public interface IGameFactory : IService
	{
		UniTask WarmUp(LevelConfig levelConfig);
		UniTask CreateGameLevel();
		UniTask<GameObject> CreatePlayer(Vector3 at);
		void SpawnNextBlock();
	}
}