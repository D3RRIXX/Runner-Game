using System;
using Infrastructure.PoolingSystem;
using UnityEngine;

namespace Game.Blocks
{
	public partial class Block : MonoBehaviour, IDisposable
	{
		[SerializeField] private Transform _respawnPoint;
		[SerializeField] private Transform _endPoint;
		
		private IPoolingManager _poolingManager;

		public Vector3 RespawnPoint => _respawnPoint.position;
		public (Vector3 position, Quaternion rotation) NextBlockSpawnTransform => (_endPoint.position, _endPoint.rotation);

		public void OnSpawned(IPoolingManager poolingManager)
		{
			_poolingManager = poolingManager;
		}
		
		public void Dispose()
		{
			_poolingManager.Return(this);
		}
	}
}