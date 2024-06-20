using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Infrastructure.AssetManagement
{
	public class AssetProvider : IAssetProvider
	{
		private readonly Dictionary<string, AsyncOperationHandle> _completedHandles = new Dictionary<string, AsyncOperationHandle>();

		public UniTask<T> Load<T>(AssetReference assetReference) where T : class 
			=> Load<T>(assetReference.AssetGUID);

		public async UniTask<T> Load<T>(string key) where T : class
		{
			if (_completedHandles.TryGetValue(key, out AsyncOperationHandle completedHandle))
				return completedHandle.Result as T;

			AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);
			await handle;

			_completedHandles[key] = handle;
			return handle.Result;
		}
	}
}