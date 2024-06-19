﻿using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Configs;
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

			public async UniTask WarmUp(LevelConfig level)
			{
				_blockPalette = level.BlockPalette;
				_parent = new GameObject("Blocks").transform;
				
				IEnumerable<AssetReferenceGameObject> prefabRefs = level.Layout.Distinct().Select(GetBlockPrefabRef);
				await UniTask.WhenAll(prefabRefs.Select(_assetProvider.Load<GameObject>));
			}

			public async UniTask<Block> InstantiateBlock(BlockType blockType, Vector3 position, Quaternion rotation)
			{
				var prefab = await _assetProvider.Load<GameObject>(GetBlockPrefabRef(blockType));
				Block block = _poolingManager.Get(prefab.GetComponent<Block>(), position, rotation, _parent);
				block.gameObject.SetActive(true);
				block.OnSpawned(_poolingManager, _eventService, blockType);
				
				return block;
			}

			private AssetReferenceGameObject GetBlockPrefabRef(BlockType blockType) => _blockPalette[blockType];
		}
	}
}