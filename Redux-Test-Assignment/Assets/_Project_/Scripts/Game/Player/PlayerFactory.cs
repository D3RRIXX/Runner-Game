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

		public UniTask WarmUp()
		{
			return UniTask.WhenAll(LoadPlayerPrefab(), LoadCameraPrefab());
		}

		private UniTask<GameObject> LoadPlayerPrefab() => _assetProvider.Load<GameObject>(AssetPaths.PLAYER);
		private UniTask<GameObject> LoadCameraPrefab() => _assetProvider.Load<GameObject>(AssetPaths.FOLLOW_CAMERA);

		public async UniTask<GameObject> Create(Vector3 at)
		{
			GameObject playerPrefab = await LoadPlayerPrefab();
			GameObject player = Object.Instantiate(playerPrefab, at, Quaternion.identity);

			GameObject cameraPrefab = await LoadCameraPrefab();
			var camera = Object.Instantiate(cameraPrefab).GetComponent<FollowCamera>();
			camera.Construct(player.transform);

			return player;
		}
	}
}