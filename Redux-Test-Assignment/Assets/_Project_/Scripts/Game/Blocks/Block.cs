using System;
using Game.Events;
using Infrastructure.EventBus;
using Infrastructure.PoolingSystem;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Game.Blocks
{
	public partial class Block : MonoBehaviour, IDisposable
	{
		private const string PLAYER_TAG = "Player";
		
		[SerializeField] private Transform _respawnPoint;
		[SerializeField] private Transform _endPoint;
		[SerializeField] private GameObject _endTrigger;
		
		private IPoolingManager _poolingManager;
		private IEventService _eventService;
		private IDisposable _disposable;

		public BlockType BlockType { get; private set; }
		
		public Vector3 RespawnPoint => _respawnPoint.position;
		public (Vector3 position, Quaternion rotation) NextBlockSpawnTransform => (_endPoint.position, _endPoint.rotation);

		public void Construct(IPoolingManager poolingManager, IEventService eventService, BlockType blockType)
		{
			_eventService = eventService;
			_poolingManager = poolingManager;
			
			BlockType = blockType;
			
			// TODO: Fire event on trigger pass
			_disposable = _endTrigger.OnTriggerEnterAsObservable()
			                         .First(x => x.CompareTag(PLAYER_TAG))
			                         .Subscribe(_ => _eventService.Fire(new BlockPassedEvent(this)));
		}
		
		public void Dispose()
		{
			_poolingManager.Return(this);
			_disposable?.Dispose();
			_disposable = null;
		}

		private void OnDestroy()
		{
			_disposable?.Dispose();
		}
	}
}