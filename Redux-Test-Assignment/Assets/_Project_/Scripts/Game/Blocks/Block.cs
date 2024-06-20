using System;
using Game.Events;
using Infrastructure.EventBus;
using Infrastructure.ServiceLocator;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utilities;

namespace Game.Blocks
{
	public partial class Block : MonoBehaviour
	{
		[SerializeField] private Transform _respawnPoint;
		[SerializeField] private Transform _endPoint;
		[SerializeField] private GameObject _endTrigger;

		private IEventService _eventService;
		private IDisposable _disposable;

		public Transform RespawnPoint => _respawnPoint;
		public Transform NextBlockSpawnTransform => _endPoint;

		private void Awake()
		{
			_eventService = AllServices.Container.GetSingle<IEventService>();
			_disposable = _endTrigger.OnTriggerEnterAsObservable()
			                         .First(x => x.CompareTag(Constants.PLAYER_TAG))
			                         .Subscribe(_ => _eventService.Fire(new BlockPassedEvent(this)));
		}

		private void OnDestroy()
		{
			_disposable?.Dispose();
		}
	}
}