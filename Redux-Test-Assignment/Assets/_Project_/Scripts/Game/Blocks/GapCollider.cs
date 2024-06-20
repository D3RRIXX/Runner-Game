using Game.Events;
using Infrastructure.EventBus;
using Infrastructure.ServiceLocator;
using UniRx;
using UnityEngine;

namespace Game.Blocks
{
	[RequireComponent(typeof(BoxCollider))]
	public class GapCollider : MonoBehaviour
	{
		private Collider _collider;
		private IEventService _eventService;

		private void Awake()
		{
			_collider = GetComponent<Collider>();
			_eventService = AllServices.Container.GetSingle<IEventService>();

			Observable.FromEvent<PlayerInvincibilityEvent>(h => _eventService.Subscribe(h), h => _eventService.Unsubscribe(h))
			          .Subscribe(x => _collider.enabled = x.IsInvincible)
			          .AddTo(this);
		}
	}
}