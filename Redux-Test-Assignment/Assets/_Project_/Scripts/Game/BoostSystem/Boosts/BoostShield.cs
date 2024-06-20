using Game.Player;
using UnityEngine;

namespace Game.BoostSystem.Boosts
{
	public class BoostShield : BoostBase
	{
		[SerializeField] private float _duration;

		protected override void ApplyBoost(GameObject player)
		{
			player.GetComponent<PlayerHealth>().SetInvincible(_duration).Forget();
		}
	}
}