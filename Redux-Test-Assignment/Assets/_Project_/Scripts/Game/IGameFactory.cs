using Cysharp.Threading.Tasks;
using Infrastructure.ServiceLocator;

namespace Game
{
	public interface IGameFactory : IService
	{
		UniTask WarmUp();
		UniTask CreateGameLevel();
		void SpawnNextBlock();
	}
}