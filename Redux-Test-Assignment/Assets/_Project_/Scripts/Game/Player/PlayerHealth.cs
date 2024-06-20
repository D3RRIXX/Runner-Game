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
		[SerializeField] private float _invincibilityDuration = 3f;
		
		private IEventService _eventService;
		private readonly ReactiveProperty<bool> _isInvincible = new ReactiveProperty<bool>();

		public float InvincibilityDuration => _invincibilityDuration;
		public IReadOnlyReactiveProperty<int> Lives => _lives;
		public IReadOnlyReactiveProperty<bool> IsInvincible => _isInvincible;
		
		private void Awake()
		{
			_eventService = AllServices.Container.GetSingle<IEventService>();
			
			_isInvincible.SkipLatestValueOnSubscribe()
			             .Select(b => new PlayerInvincibilityEvent(this))
			             .Subscribe(x => _eventService.Fire(x))
			             .AddTo(this);
		}

		public void TakeDamage()
		{
			_lives.Value--;
			_eventService.Fire(new PlayerDiedEvent(this));
		}

		public async UniTaskVoid SetInvincible()
		{
			_isInvincible.Value = true;
			await UniTask.Delay(TimeSpan.FromSeconds(_invincibilityDuration));
			_isInvincible.Value = false;
		}
	}
}