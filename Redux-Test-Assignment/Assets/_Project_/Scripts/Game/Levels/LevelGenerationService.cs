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

			var prevBlockType = BlockType.Default;
			for (int i = 1; i < levelLength; i++)
			{
				BlockType blockType;
				do
				{
					blockType = (BlockType)Random.Range(0, _blockTypes.Length);
				} while (!BlockTypeIsValid(blockType, prevBlockType));
				
				blocks.Add(blockType);
				prevBlockType = blockType;
			}

			return new LevelConfig(blocks, _levelGenerationConfig.BlockPalette);
		}

		private bool BlockTypeIsValid(BlockType blockType, BlockType prevBlockType)
		{
			if (prevBlockType == BlockType.TurnL || prevBlockType == BlockType.TurnR)
				return blockType != BlockType.TurnL && blockType != BlockType.TurnR;
			
			return true;
		}
	}
}