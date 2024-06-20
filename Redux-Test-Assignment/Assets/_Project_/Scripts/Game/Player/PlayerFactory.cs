using Cysharp.Threading.Tasks;
using Infrastructure.AssetManagement;
using UnityEngine;

namespace Game.Player
{
	public class PlayerFactory
	{
		private readonly IAssetProvider _assetProvider;

		public PlayerFactory(IAssetProvider assetProvider)
		{
			_assetProvider = assetProvider;
		}

		public UniTask WarmUp() => _assetProvider.Load<GameObject>(AssetPaths.PLAYER);

		public async UniTask<GameObject> Create(Vector3 at)
		{
			var prefab = await _assetProvider.Load<GameObject>(AssetPaths.PLAYER);
			return Object.Instantiate(prefab, at, Quaternion.identity);
		}
	}
}