using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Blocks;
using Game.Levels;
using Game.Player;
using UnityEngine;
using Utilities;

namespace Game.Factory
{
	public class GameFactory : IGameFactory
	{
		private readonly Block.Factory _blockFactory;
		private readonly PlayerFactory _playerFactory;
		private readonly List<Block> _activeBlocks = new List<Block>();

		private LevelConfig _level;
		private int _nextBlockIdx;

		public GameFactory(Block.Factory blockFactory, PlayerFactory playerFactory)
		{
			_blockFactory = blockFactory;
			_playerFactory = playerFactory;
		}

		public async UniTask WarmUp(LevelConfig levelConfig)
		{
			_level = levelConfig;

			await UniTask.WhenAll(_blockFactory.WarmUp(_level), _playerFactory.WarmUp());
		}

		public UniTask<GameObject> CreatePlayer(Vector3 at) => _playerFactory.Create(at);

		public async UniTask<Block> TrySpawnNextBlock()
		{
			if (_nextBlockIdx > _level.Blocks.Count)
				return null;

			if (_nextBlockIdx == _level.Blocks.Count)
			{
				SpawnFinishBlock();
				return null;
			}

			TryDespawnFirstActiveBlock();

			(Vector3 position, Quaternion rotation) = GetNextBlockSpawnTransform();

			BlockType blockType = _level.Blocks[_nextBlockIdx];
			Block block = await _blockFactory.InstantiateBlock(blockType, position, rotation);
			_activeBlocks.Add(block);

			_nextBlockIdx++;
			return block;
		}

		private async void SpawnFinishBlock()
		{
			(Vector3 position, Quaternion rotation) = GetNextBlockSpawnTransform();
			await _blockFactory.InstantiateFinishBlock(position, rotation);
		}

		private (Vector3 position, Quaternion rotation) GetNextBlockSpawnTransform()
		{
			if (_activeBlocks.Count == 0)
				return (Vector3.zero, Quaternion.identity);
			
			Block lastBlock = _activeBlocks[_activeBlocks.Count - 1];
			Transform nextBlockSpawnTransform = lastBlock.NextBlockSpawnTransform;
			
			return (nextBlockSpawnTransform.position, nextBlockSpawnTransform.rotation);
		}

		private void TryDespawnFirstActiveBlock()
		{
			if (_activeBlocks.Count < Constants.BLOCKS_AHEAD + 1)
				return;

			Block firstBlock = _activeBlocks[0];
			firstBlock.Dispose();

			_activeBlocks.RemoveAt(0);
		}
	}
}