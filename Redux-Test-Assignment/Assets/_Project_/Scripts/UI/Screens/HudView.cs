using System;
using CustomExtensions.UIExtensions;
using Game.Events;
using Game.Levels;
using Game.Player;
using Infrastructure.EventBus;
using Infrastructure.ServiceLocator;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
	public class HudView : MonoBehaviour
	{
		[SerializeField] private FormattedLabel _livesLabel;
		[SerializeField] private GameObject _invincibleRoot;
		[SerializeField] private Slider _invincibleSlider;

		private PlayerHealth _player;
		private IEventService _eventService;
		private IDisposable _invincibilitySliderStream;

		private void Awake()
		{
			_player = AllServices.Container.GetSingle<LevelState>().Player;
			_eventService = AllServices.Container.GetSingle<IEventService>();

			_eventService.Subscribe<PlayerInvincibilityEvent>(OnPlayerInvincibility);
		}

		private void Start()
		{
			_player.Lives
			       .Subscribe(x => _livesLabel.SetValues(x.ToString()))
			       .AddTo(this);
		}

		private void OnDestroy()
		{
			_eventService.Unsubscribe<PlayerInvincibilityEvent>(OnPlayerInvincibility);
			_invincibilitySliderStream?.Dispose();
		}

		private void OnPlayerInvincibility(PlayerInvincibilityEvent evt)
		{
			if (!evt.IsInvincible)
			{
				_invincibleRoot.SetActive(false);
				_invincibilitySliderStream?.Dispose();
				_invincibilitySliderStream = null;
			}
			else
			{
				_invincibleRoot.SetActive(true);
				_invincibleSlider.maxValue = _invincibleSlider.value = evt.InvincibilityDuration;

				_invincibilitySliderStream = Observable.EveryUpdate()
				                                       .Scan(evt.InvincibilityDuration, (x, _) => x - Time.deltaTime)
				                                       .TakeWhile(x => x >= 0f)
				                                       .Subscribe(x => _invincibleSlider.value = x);
			}
		}
	}
}