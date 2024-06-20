namespace Game.Player
{
	public class PlayerDiedEvent
	{
		public PlayerDiedEvent(PlayerHealth player)
		{
			Player = player;
		}

		public PlayerHealth Player { get; }
	}
}