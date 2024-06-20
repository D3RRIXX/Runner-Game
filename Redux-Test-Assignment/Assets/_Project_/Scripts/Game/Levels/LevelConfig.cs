using System.Collections.Generic;
using Game.Blocks;
using UnityEngine;
using Utilities;

namespace Game.Configs
{
	[CreateAssetMenu(fileName = "New Level Config", menuName = SOPaths.CONFIGS + "Level Config", order = 0)]
	public class LevelConfig : ScriptableObject
	{
		[SerializeField] private BlockType[] _layout;
		[SerializeField] private BlockPalette _blockPalette;

		public IReadOnlyList<BlockType> Blocks => _layout;
		public BlockPalette BlockPalette => _blockPalette;
	}
}