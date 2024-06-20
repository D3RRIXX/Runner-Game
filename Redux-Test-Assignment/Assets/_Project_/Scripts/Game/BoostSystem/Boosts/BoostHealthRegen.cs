using Game.Player;
using UnityEngine;

namespace Game.BoostSystem.Boosts
{
	public class BoostHealthRegen : BoostBase
	{
		[SerializeField] private int _livesToRestore = 2;

		protected override void ApplyBoost(GameObject player)
		{
			player.GetComponent<PlayerHealth>().RestoreLives(_livesToRestore);
		}
	}
}