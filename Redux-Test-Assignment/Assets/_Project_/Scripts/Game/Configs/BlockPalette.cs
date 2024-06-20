using Game.Blocks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utilities;

namespace Game.Configs
{
	[CreateAssetMenu(fileName = "Block Palette", menuName = SOPaths.CONFIGS + "Block Palette", order = 0)]
	public class BlockPalette : ScriptableObject
	{
		[SerializeField] private GenericDictionary<BlockType, AssetReferenceGameObject> _blocks;
		[SerializeField] private AssetReferenceGameObject _finishBlock;

		public AssetReferenceGameObject this[BlockType blockType] => _blocks[blockType];
		public AssetReferenceGameObject FinishBlock => _finishBlock;
	}
}