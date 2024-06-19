using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Blocks;
using Game.Configs;
using Game.Levels;
using UnityEngine;

namespace Game
{
	public class GameFactory : IGameFactory
	{
		private const int BLOCKS_AHEAD = 3;

		private readonly Block.Factory _blockFactory;
		private readonly LevelConfig _level;

		private int _nextBlockIdx;
		private List<Block> _activeBlocks = new List<Block>();

		public GameFactory(ILevelService levelService, Block.Factory blockFactory)
		{
			_blockFactory = blockFactory;
			_level = levelService.GetNextLevel();
		}

		public async UniTask WarmUp()
		{
			await _blockFactory.WarmUp(_level);
		}

		public async UniTask CreateGameLevel()
		{
			Vector3 position = Vector3.zero;
			Quaternion rotation = Quaternion.identity;

			for (int i = 0; i < BLOCKS_AHEAD; i++)
			{
				BlockType blockType = _level.Layout[i];
				Block block = await _blockFactory.InstantiateBlock(blockType, position, rotation);
				(position, rotation) = block.NextBlockSpawnTransform;

				_activeBlocks.Add(block);
			}

			_nextBlockIdx = BLOCKS_AHEAD;
		}

		public async void SpawnNextBlock()
		{
			if (_nextBlockIdx >= _level.Layout.Count)
				return;
			
			TryDespawnFirstActiveBlock();

			Block lastBlock = _activeBlocks[_activeBlocks.Count - 1];
			(Vector3 position, Quaternion rotation) = lastBlock.NextBlockSpawnTransform;

			BlockType blockType = _level.Layout[_nextBlockIdx];
			Block block = await _blockFactory.InstantiateBlock(blockType, position, rotation);
			_activeBlocks.Add(block);

			_nextBlockIdx++;
		}

		private void TryDespawnFirstActiveBlock()
		{
			if (_activeBlocks.Count < BLOCKS_AHEAD + 1)
				return;

			Block firstBlock = _activeBlocks[0];
			firstBlock.Dispose();
			
			_activeBlocks.RemoveAt(0);
		}
	}
}