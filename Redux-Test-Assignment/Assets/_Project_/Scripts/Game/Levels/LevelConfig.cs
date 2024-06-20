using System.Collections.Generic;
using Game.Blocks;
using Game.Configs;

namespace Game.Levels
{
	public class LevelConfig
	{
		private readonly List<BlockType> _blocks;

		public LevelConfig(List<BlockType> blocks, BlockPalette blockPalette)
		{
			_blocks = blocks;
			BlockPalette = blockPalette;
		}

		public IReadOnlyList<BlockType> Blocks => _blocks;
		public BlockPalette BlockPalette { get; }
	}
}