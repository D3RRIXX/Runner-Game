using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;

namespace Game
{
	[CreateAssetMenu(fileName = "New Level Config", menuName = SOPaths.CONFIGS + "Level Config", order = 0)]
	public class LevelConfig : ScriptableObject
	{
		[SerializeField] private List<BlockType> _layout;
		[SerializeField] private BlockPalette _blockPalette;

		public IReadOnlyList<BlockType> Layout => _layout;
		public BlockPalette BlockPalette => _blockPalette;
	}
}