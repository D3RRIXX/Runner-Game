using Game.Player;
using UnityEngine;

namespace Game.BoostSystem.Boosts
{
	public class BoostSpeed : BoostBase
	{
		[SerializeField] private float _speedModifier = 1.5f;

		protected override void ApplyBoost(GameObject player)
		{
			player.GetComponent<PlayerMovement>().SetSpeedModifier(_speedModifier);
		}
	}
}