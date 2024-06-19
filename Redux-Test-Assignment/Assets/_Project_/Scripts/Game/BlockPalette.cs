using UnityEngine;
using UnityEngine.AddressableAssets;
using Utilities;

namespace Game
{
	[CreateAssetMenu(fileName = "Block Palette", menuName = SOPaths.CONFIGS + "Block Palette", order = 0)]
	public class BlockPalette : ScriptableObject
	{
		[SerializeField] private GenericDictionary<BlockType, AssetReferenceGameObject> _blocks;

		public AssetReferenceGameObject this[BlockType blockType] => _blocks[blockType];
	}
}