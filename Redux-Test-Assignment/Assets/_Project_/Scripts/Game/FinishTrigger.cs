using Game.Events;
using Infrastructure.EventBus;
using Infrastructure.ServiceLocator;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Game
{
	public class FinishTrigger : MonoBehaviour
	{
		private const string PLAYER_TAG = "Player";
		private IEventService _eventService;

		private void Awake()
		{
			_eventService = AllServices.Container.GetSingle<IEventService>();

			this.OnTriggerEnterAsObservable()
			    .Where(x => x.CompareTag(PLAYER_TAG))
			    .Subscribe(_ => _eventService.Fire<LevelCompletedEvent>())
			    .AddTo(this);
		}
	}
}