using Cysharp.Threading.Tasks;
using Game.Configs;
using Infrastructure.ServiceLocator;

namespace Game
{
	public interface IGameFactory : IService
	{
		UniTask WarmUp(LevelConfig levelConfig);
		UniTask CreateGameLevel();
		void SpawnNextBlock();
	}
}