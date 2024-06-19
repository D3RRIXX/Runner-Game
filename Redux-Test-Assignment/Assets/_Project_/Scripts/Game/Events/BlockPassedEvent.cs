using Game.Blocks;

namespace Game.Events
{
	public class BlockPassedEvent
	{
		public BlockPassedEvent(Block block)
		{
			Block = block;
		}
		
		public Block Block { get; }
	}
}