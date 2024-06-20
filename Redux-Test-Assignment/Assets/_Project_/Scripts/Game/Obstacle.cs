using Game.Events;
using Game.Player;
using Infrastructure.EventBus;
using Infrastructure.ServiceLocator;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utilities;

namespace Game
{
	[RequireComponent(typeof(Collider))]
	public class Obstacle : MonoBehaviour
	{
		private Collider _collider;
		private IEventService _eventService;

		private void Awake()
		{
			_collider = GetComponent<Collider>();
			_eventService = AllServices.Container.GetSingle<IEventService>();
			
			this.OnTriggerEnterAsObservable()
			    .Where(x => x.CompareTag(Constants.PLAYER_TAG))
			    .Select(x => x.GetComponent<PlayerHealth>())
			    .Where(x => !x.IsInvincible.Value)
			    .Subscribe(DealDamageToPlayer)
			    .AddTo(this);

			Observable.FromEvent<PlayerInvincibilityEvent>(h => _eventService.Subscribe(h), h => _eventService.Unsubscribe(h))
			          .Subscribe(evt => _collider.enabled = !evt.IsInvincible)
			          .AddTo(this);
		}

		private void DealDamageToPlayer(PlayerHealth player)
		{
			Debug.Log("Hit player");
			player.TakeDamage();
		}
	}
}