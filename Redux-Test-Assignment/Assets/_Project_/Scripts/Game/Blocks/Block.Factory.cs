using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Configs;
using Game.Levels;
using Infrastructure.AssetManagement;
using Infrastructure.EventBus;
using Infrastructure.PoolingSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Blocks
{
	public partial class Block
	{
		public class Factory
		{
			private readonly IAssetProvider _assetProvider;
			private readonly IEventService _eventService;
			private readonly IPoolingManager _poolingManager;
			
			private Transform _parent;
			private BlockPalette _blockPalette;

			public Factory(IAssetProvider assetProvider, IPoolingManager poolingManager, IEventService eventService)
			{
				_assetProvider = assetProvider;
				_poolingManager = poolingManager;
				_eventService = eventService;
			}

			public async UniTask WarmUp(LevelConfig levelGeneration)
			{
				_blockPalette = levelGeneration.BlockPalette;
				_parent = new GameObject("Blocks").transform;
				
				IEnumerable<AssetReferenceGameObject> prefabRefs = levelGeneration.Blocks.Distinct().Select(GetBlockPrefabRef);
				await UniTask.WhenAll(prefabRefs.Select(_assetProvider.Load<GameObject>));
			}

			public async UniTask<Block> InstantiateBlock(BlockType blockType, Vector3 position, Quaternion rotation)
			{
				var prefab = await _assetProvider.Load<GameObject>(GetBlockPrefabRef(blockType));
				Block block = _poolingManager.Get(prefab.GetComponent<Block>(), position, rotation, _parent);
				block.gameObject.SetActive(true);
				block.Construct(_poolingManager, _eventService, blockType);
				
				return block;
			}

			public async UniTask<GameObject> InstantiateFinishBlock(Vector3 position, Quaternion rotation)
			{
				var prefab = await _assetProvider.Load<GameObject>(_blockPalette.FinishBlock);
				return Instantiate(prefab, position, rotation);
			}

			private AssetReferenceGameObject GetBlockPrefabRef(BlockType blockType) => _blockPalette[blockType];
		}
	}
}