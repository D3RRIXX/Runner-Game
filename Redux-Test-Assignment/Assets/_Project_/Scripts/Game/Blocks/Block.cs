using System;
using Infrastructure.EventBus;
using Infrastructure.PoolingSystem;
using UnityEngine;

namespace Game.Blocks
{
	public partial class Block : MonoBehaviour, IDisposable
	{
		[SerializeField] private Transform _respawnPoint;
		[SerializeField] private Transform _endPoint;
		
		private IPoolingManager _poolingManager;
		private IEventService _eventService;

		public BlockType BlockType { get; private set; }
		
		public Vector3 RespawnPoint => _respawnPoint.position;
		public (Vector3 position, Quaternion rotation) NextBlockSpawnTransform => (_endPoint.position, _endPoint.rotation);

		public void OnSpawned(IPoolingManager poolingManager, IEventService eventService, BlockType blockType)
		{
			_eventService = eventService;
			_poolingManager = poolingManager;
			
			BlockType = blockType;
			
			// TODO: Fire event on trigger pass
		}
		
		public void Dispose()
		{
			_poolingManager.Return(this);
		}
	}
}