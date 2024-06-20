using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Blocks;
using Game.Levels;
using Game.Player;
using UI;
using UnityEngine;

namespace Game.Factory
{
	public class GameFactory : IGameFactory
	{
		private readonly Block.Factory _blockFactory;
		private readonly PlayerFactory _playerFactory;
		private readonly UIFactory _uiFactory;
		private readonly List<Block> _activeBlocks = new List<Block>();

		private LevelConfig _level;
		private int _nextBlockIdx;

		public GameFactory(Block.Factory blockFactory, PlayerFactory playerFactory, UIFactory uiFactory)
		{
			_blockFactory = blockFactory;
			_playerFactory = playerFactory;
			_uiFactory = uiFactory;
		}

		public async UniTask WarmUp(LevelConfig levelConfig)
		{
			_level = levelConfig;

			await UniTask.WhenAll(_blockFactory.WarmUp(_level), _playerFactory.WarmUp(), _uiFactory.WarmUp());
		}

		public void CleanUp()
		{
			foreach (Block block in _activeBlocks)
			{
				block.Dispose();
				Object.Destroy(block.gameObject);
			}

			_activeBlocks.Clear();
		}

		public UniTask<GameObject> CreatePlayer(Vector3 at) => _playerFactory.Create(at);

		public async UniTask BuildLevel(LevelConfig level)
		{
			(Vector3 position, Quaternion rotation) = GetNextBlockSpawnTransform();
			foreach (BlockType blockType in level.Blocks)
			{
				Block block = await _blockFactory.InstantiateBlock(blockType, position, rotation);

				_activeBlocks.Add(block);
				_nextBlockIdx++;
				
				(position, rotation) = GetNextBlockSpawnTransform();
			}

			await _blockFactory.InstantiateFinishBlock(position, rotation);
		}

		public UniTask<UIManager> CreateUIRoot(PlayerHealth player) => _uiFactory.CreateUIRoot(player);

		private (Vector3 position, Quaternion rotation) GetNextBlockSpawnTransform()
		{
			if (_activeBlocks.Count == 0)
				return (Vector3.zero, Quaternion.identity);

			Block lastBlock = _activeBlocks[_activeBlocks.Count - 1];
			Transform nextBlockSpawnTransform = lastBlock.NextBlockSpawnTransform;

			return (nextBlockSpawnTransform.position, nextBlockSpawnTransform.rotation);
		}
	}
}