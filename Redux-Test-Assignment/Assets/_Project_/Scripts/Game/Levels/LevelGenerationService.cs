using System;
using System.Collections.Generic;
using Game.Blocks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Levels
{
	public class LevelGenerationService : ILevelService
	{
		private readonly LevelGenerationConfig _levelGenerationConfig;
		private readonly BlockType[] _blockTypes;

		public LevelGenerationService(LevelGenerationConfig levelGenerationConfig)
		{
			_levelGenerationConfig = levelGenerationConfig;
			_blockTypes = (BlockType[])Enum.GetValues(typeof(BlockType));
		}

		public LevelConfig GetNextLevel()
		{
			Vector2Int lengthRange = _levelGenerationConfig.LevelLengthRange;
			int levelLength = Random.Range(lengthRange.x, lengthRange.y + 1);

			var blocks = new List<BlockType>(levelLength)
			{
				BlockType.Default
			};
			
			for (int i = 1; i < levelLength; i++)
			{
				blocks.Add((BlockType)Random.Range(0, _blockTypes.Length));
			}

			return new LevelConfig(blocks, _levelGenerationConfig.BlockPalette);
		}
	}
}