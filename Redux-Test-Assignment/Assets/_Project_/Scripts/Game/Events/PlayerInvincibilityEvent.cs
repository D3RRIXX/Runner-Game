namespace Game.Events
{
	public class PlayerInvincibilityEvent
	{
		public PlayerInvincibilityEvent(bool isInvincible, float duration)
		{
			IsInvincible = isInvincible;
			InvincibilityDuration = duration;
		}

		public float InvincibilityDuration { get; }
		public bool IsInvincible { get; }
	}
}