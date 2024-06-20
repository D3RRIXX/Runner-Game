using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Configs;
using Game.Levels;
using Infrastructure.AssetManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Blocks
{
	public partial class Block
	{
		public class Factory
		{
			private readonly IAssetProvider _assetProvider;

			private Transform _parent;
			private BlockPalette _blockPalette;

			public Factory(IAssetProvider assetProvider)
			{
				_assetProvider = assetProvider;
			}

			public async UniTask WarmUp(LevelConfig level)
			{
				_blockPalette = level.BlockPalette;
				_parent = new GameObject("Blocks").transform;
				
				IEnumerable<AssetReferenceGameObject> prefabRefs = level.Blocks.Distinct().Select(GetBlockPrefabRef);
				await UniTask.WhenAll(prefabRefs.Select(_assetProvider.Load<GameObject>));
			}

			public async UniTask<Block> InstantiateBlock(BlockType blockType, Vector3 position, Quaternion rotation)
			{
				var prefab = await _assetProvider.Load<GameObject>(GetBlockPrefabRef(blockType));
				Block block = Instantiate(prefab, position, rotation, _parent).GetComponent<Block>();
				block.gameObject.SetActive(true);
				
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