using Cysharp.Threading.Tasks;
using Infrastructure.ServiceLocator;
using UnityEngine.AddressableAssets;

namespace Game
{
	public interface IAssetProvider : IService
	{
		UniTask<T> Load<T>(AssetReference assetReference) where T : class;
	}
}