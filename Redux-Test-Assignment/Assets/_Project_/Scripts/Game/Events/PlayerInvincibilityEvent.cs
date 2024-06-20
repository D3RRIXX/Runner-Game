namespace Game.Events
{
	public class PlayerInvincibilityEvent
	{
		public PlayerInvincibilityEvent(bool isInvincible)
		{
			IsInvincible = isInvincible;
		}
		
		public bool IsInvincible { get; }
	}
}