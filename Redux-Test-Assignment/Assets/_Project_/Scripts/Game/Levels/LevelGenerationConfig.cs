using System.Collections.Generic;
using Game.Blocks;
using Game.Configs;
using UnityEngine;
using Utilities;

namespace Game.Levels
{
	[CreateAssetMenu(fileName = "New Level Config", menuName = SOPaths.CONFIGS + "Level Config", order = 0)]
	public class LevelGenerationConfig : ScriptableObject
	{
		[SerializeField] private Vector2Int _levelLengthRange;
		[SerializeField] private BlockPalette _blockPalette;

		public Vector2Int LevelLengthRange => _levelLengthRange;
		public BlockPalette BlockPalette => _blockPalette;
	}
}