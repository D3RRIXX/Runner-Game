using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game
{
	public class AssetProvider : IAssetProvider
	{
		private readonly Dictionary<string, AsyncOperationHandle> _completedHandles = new Dictionary<string, AsyncOperationHandle>();

		public async UniTask<T> Load<T>(AssetReference assetReference) where T : class
		{
			if (_completedHandles.TryGetValue(assetReference.AssetGUID, out AsyncOperationHandle completedHandle))
				return completedHandle.Result as T;

			AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(assetReference);
			await handle;

			_completedHandles[assetReference.AssetGUID] = handle;
			return handle.Result;
		}
	}
}