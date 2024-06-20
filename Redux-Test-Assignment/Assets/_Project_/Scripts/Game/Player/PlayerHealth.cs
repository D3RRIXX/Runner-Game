using System;
using Cysharp.Threading.Tasks;
using Game.Events;
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
		private int _maxLives;
		private readonly ReactiveProperty<bool> _isInvincible = new ReactiveProperty<bool>();

		public IReadOnlyReactiveProperty<int> Lives => _lives;
		public IReadOnlyReactiveProperty<bool> IsInvincible => _isInvincible;
		
		private void Awake()
		{
			_maxLives = _lives.Value;
			_eventService = AllServices.Container.GetSingle<IEventService>();
		}

		public void TakeDamage()
		{
			_lives.Value--;
			_eventService.Fire(new PlayerDiedEvent(this));
		}

		public async UniTaskVoid SetInvincible(float duration)
		{
			_isInvincible.Value = true;
			_eventService.Fire(new PlayerInvincibilityEvent(isInvincible: true, duration));
			
			await UniTask.Delay(TimeSpan.FromSeconds(duration));
			
			_eventService.Fire(new PlayerInvincibilityEvent(isInvincible: false, 0f));
			_isInvincible.Value = false;
		}

		public void RestoreLives(int livesToRestore)
		{
			_lives.Value = Mathf.Min(_lives.Value + livesToRestore, _maxLives);
		}
	}
}