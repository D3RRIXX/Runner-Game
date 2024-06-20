using Cysharp.Threading.Tasks;
using Infrastructure.ServiceLocator;
using UnityEngine.AddressableAssets;

namespace Infrastructure.AssetManagement
{
	public interface IAssetProvider : IService
	{
		UniTask<T> Load<T>(AssetReference assetReference) where T : class;
		UniTask<T> Load<T>(string key) where T : class;
	}
}