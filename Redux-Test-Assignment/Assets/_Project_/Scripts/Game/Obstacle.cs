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
			
			this.OnTriggerEnterAsObservable()
			    .Where(x => x.CompareTag(Constants.PLAYER_TAG))
			    .Subscribe(DealDamageToPlayer)
			    .AddTo(this);

			_eventService = AllServices.Container.GetSingle<IEventService>();

			Observable.FromEvent<PlayerInvincibilityEvent>(h => _eventService.Subscribe(h), h => _eventService.Unsubscribe(h))
			          .Subscribe(evt => _collider.enabled = !evt.IsInvincible)
			          .AddTo(this);
		}

		private void DealDamageToPlayer(Collider player)
		{
			Debug.Log("Hit player");
			player.GetComponent<PlayerHealth>().TakeDamage();
		}
	}
}