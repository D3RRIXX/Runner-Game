using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utilities;

namespace Game
{
	[RequireComponent(typeof(Collider))]
	public class Obstacle : MonoBehaviour
	{
		private void Awake()
		{
			this.OnTriggerEnterAsObservable()
			    .Where(x => x.CompareTag(Constants.PLAYER_TAG))
			    .Subscribe(DealDamageToPlayer)
			    .AddTo(this);
		}

		private void DealDamageToPlayer(Collider player)
		{
			
		}
	}
}