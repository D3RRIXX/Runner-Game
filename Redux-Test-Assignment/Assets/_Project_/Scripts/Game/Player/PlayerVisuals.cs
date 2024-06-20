using System.Collections;
using UniRx;
using UnityEngine;

namespace Game.Player
{
	[RequireComponent(typeof(PlayerHealth))]
	public class PlayerVisuals : MonoBehaviour
	{
		[SerializeField] private MeshRenderer _shield;
		[SerializeField] private PlayerHealth _playerHealth;
		[SerializeField] private float _blinkInterval = 0.1f;

		private Coroutine _blinkRoutine;

		private void Reset()
		{
			_playerHealth = GetComponent<PlayerHealth>();
		}

		private void Awake()
		{
			_playerHealth.IsInvincible
			             .SkipLatestValueOnSubscribe()
			             .Subscribe(_shield.gameObject.SetActive)
			             .AddTo(this);
		}
	}
}