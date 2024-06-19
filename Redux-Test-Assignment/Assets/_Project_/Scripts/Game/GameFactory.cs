using Cysharp.Threading.Tasks;
using Game.Blocks;
using Game.Configs;
using Game.Levels;
using UnityEngine;

namespace Game
{
	public class GameFactory
	{
		private readonly Block.Factory _blockFactory;
		private readonly LevelConfig _level;

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

			foreach (BlockType blockType in _level.Layout)
			{
				Block block = await _blockFactory.InstantiateBlock(blockType, position, rotation);
				(position, rotation) = block.NextBlockSpawnTransform;
			}
		}
	}
}