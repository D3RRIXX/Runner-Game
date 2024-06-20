using System.Collections;
using UniRx;
using UnityEngine;

namespace Game.Player
{
	[RequireComponent(typeof(PlayerHealth))]
	public class PlayerVisuals : MonoBehaviour
	{
		[SerializeField] private Renderer[] _renderers;
		[SerializeField] private PlayerHealth _playerHealth;
		[SerializeField] private float _blinkInterval = 0.1f;

		private Material _material;
		private Coroutine _blinkRoutine;

		private void Reset()
		{
			_renderers = GetComponentsInChildren<Renderer>();
			_playerHealth = GetComponent<PlayerHealth>();
		}

		private void Awake()
		{
			_material = _renderers[0].sharedMaterial;
			_playerHealth.IsInvincible
			             .SkipLatestValueOnSubscribe()
			             .Subscribe(b =>
			             {
				             if (b)
				             {
					             _blinkRoutine = StartCoroutine(BlinkMaterialRoutine());
				             }
				             else
				             {
					             StopCoroutine(_blinkRoutine);
					             SetMaterialsColor(Color.white);
				             }
			             })
			             .AddTo(this);
		}

		private IEnumerator BlinkMaterialRoutine()
		{
			bool isWhite = false;
			while (true)
			{
				Color color = isWhite ? Color.clear : Color.white;
				
				SetMaterialsColor(color);

				isWhite = !isWhite;

				yield return new WaitForSeconds(_blinkInterval);
			}
		}

		private void SetMaterialsColor(Color color)
		{
			foreach (Renderer renderer in _renderers)
			{
				renderer.material.color = color;
			}
		}
	}
}