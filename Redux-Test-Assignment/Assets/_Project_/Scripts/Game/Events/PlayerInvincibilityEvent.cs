using Game.Player;

namespace Game.Events
{
	public class PlayerInvincibilityEvent
	{
		public PlayerInvincibilityEvent(PlayerHealth health)
		{
			Health = health;
		}

		public PlayerHealth Health { get; }
		public float InvincibilityDuration => Health.InvincibilityDuration;
		public bool IsInvincible => Health.IsInvincible.Value;
	}
}