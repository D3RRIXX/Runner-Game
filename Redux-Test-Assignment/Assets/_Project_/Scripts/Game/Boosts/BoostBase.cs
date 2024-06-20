using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utilities;

namespace Game.Boosts
{
	public abstract class BoostBase : MonoBehaviour
	{
		private void Awake()
		{
			this.OnTriggerEnterAsObservable()
			    .Where(x => x.CompareTag(Constants.PLAYER_TAG))
			    .Subscribe(x => ApplyBoost(x.gameObject))
			    .AddTo(this);
		}

		protected abstract void ApplyBoost(GameObject player);
	}
}