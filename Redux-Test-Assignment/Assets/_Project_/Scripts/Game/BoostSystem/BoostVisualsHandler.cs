using DG.Tweening;
using UnityEngine;

namespace Game.BoostSystem
{
	public class BoostVisualsHandler : MonoBehaviour
	{
		[SerializeField] private Transform _visualsRoot;
		[SerializeField] private float _rotationSpeed = 5f;

		private void OnEnable()
		{
			_visualsRoot.DORotate(Vector3.up, _rotationSpeed, RotateMode.FastBeyond360)
			            .SetSpeedBased()
			            .SetLoops(-1, LoopType.Incremental)
			            .SetLink(gameObject, LinkBehaviour.KillOnDisable);
		}
	}
}