using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utilities;

namespace Game.Blocks
{
	[RequireComponent(typeof(Collider))]
	public class TurnTrigger : MonoBehaviour
	{
		[SerializeField] private Vector3 _relativeEuler = Vector3.up * 90f;

		private void Awake()
		{
			this.OnTriggerEnterAsObservable()
			    .Where(x => x.CompareTag(Constants.PLAYER_TAG))
			    .Subscribe(x => x.transform.localEulerAngles += _relativeEuler)
			    .AddTo(this);
		}
	}
}