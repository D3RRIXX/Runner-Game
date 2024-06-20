using Infrastructure.EventBus;
using Infrastructure.ServiceLocator;
using UniRx;
using UnityEngine;

namespace Game.Player
{
	public class PlayerHealth : MonoBehaviour
	{
		[SerializeField] private IntReactiveProperty _lives = new IntReactiveProperty(3);
		private IEventService _eventService;

		public IReadOnlyReactiveProperty<int> Lives => _lives;
		
		private void Awake()
		{
			_eventService = AllServices.Container.GetSingle<IEventService>();
		}

		public void TakeDamage()
		{
			_lives.Value--;
			_eventService.Fire(new PlayerDiedEvent(this));
		}
	}
}