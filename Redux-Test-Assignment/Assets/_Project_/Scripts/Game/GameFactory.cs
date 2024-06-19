using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Blocks;
using Game.Configs;
using Game.Levels;
using Infrastructure.PoolingSystem;
using Infrastructure.ServiceLocator;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game
{
	public class GameFactory : MonoBehaviour
	{
		private IAssetProvider _assetProvider;
		private IPoolingManager _poolingManager;

		private LevelConfig _level;

		private void Awake()
		{
			_assetProvider = AllServices.Container.GetSingle<IAssetProvider>();
			_poolingManager = AllServices.Container.GetSingle<IPoolingManager>();

			var levelService = AllServices.Container.GetSingle<ILevelService>();
			_level = levelService.GetNextLevel();
		}

		public async UniTask WarmUp()
		{
			IEnumerable<AssetReferenceGameObject> prefabRefs = _level.Layout.Distinct().Select(GetBlockPrefabRef);
			await UniTask.WhenAll(prefabRefs.Select(_assetProvider.Load<GameObject>));
		}

		private async void Start()
		{
			await WarmUp();

			Vector3 position = Vector3.zero;
			Quaternion rotation = Quaternion.identity;

			foreach (BlockType blockType in _level.Layout)
			{
				Block block = await InstantiateBlock(blockType, position, rotation);
				
				(position, rotation) = block.NextBlockSpawnTransform;
			}
		}

		private async UniTask<Block> InstantiateBlock(BlockType blockType, Vector3 position, Quaternion rotation)
		{
			var prefab = await _assetProvider.Load<GameObject>(GetBlockPrefabRef(blockType));
			return _poolingManager.Get(prefab.GetComponent<Block>(), position, rotation, transform);
		}

		private AssetReferenceGameObject GetBlockPrefabRef(BlockType blockType) => _level.BlockPalette[blockType];
	}
}