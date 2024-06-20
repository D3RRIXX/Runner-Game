using Cysharp.Threading.Tasks;
using Game.Blocks;
using Game.Levels;
using Game.Player;
using Infrastructure.ServiceLocator;
using UI;
using UnityEngine;

namespace Game.Factory
{
	public interface IGameFactory : IService
	{
		UniTask WarmUp(LevelConfig levelConfig);
		UniTask<GameObject> CreatePlayer(Vector3 at);
		UniTask<Block> TrySpawnNextBlock();
		UniTask<UIManager> CreateUIRoot(PlayerHealth player);
		void CleanUp();
	}
}