using System.Collections.Generic;
using UnityEngine;
using Original = UnityEngine.Component;
using Borrowed = UnityEngine.Component;

namespace Infrastructure.PoolingSystem
{
	public partial class PoolingManager : IPoolingManager
	{
		private readonly Transform _poolParent;

		private readonly Dictionary<Original, Pool> _pools = new Dictionary<Original, Pool>();
		private readonly Dictionary<Borrowed, Original> _borrowedMap = new Dictionary<Borrowed, Original>();

		public PoolingManager(Transform poolParent)
		{
			_poolParent = poolParent;
		}

		public T Get<T>(T prefab) where T : Original
		{
			if (!_pools.TryGetValue(prefab, out Pool pool))
			{
				pool = CreatePool(prefab);
			}

			var copy = (T)pool.Get();
			_borrowedMap.Add(copy, prefab);

			return copy;
		}
		
		public T Get<T>(T prefab, Transform parent) where T : Original
		{
			var copy = Get(prefab);
			copy.transform.SetParent(parent);

			return copy;
		}
		
		public T Get<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : Original
		{
			var copy = Get(prefab);
			Transform transform = copy.transform;
			transform.SetParent(parent);
			transform.SetPositionAndRotation(position, rotation);

			return copy;
		}

		// ReSharper disable Unity.PerformanceAnalysis
		public void Return<T>(T clone) where T : Original
		{
			if (!_borrowedMap.TryGetValue(clone, out Original prefab))
			{
				Debug.LogException(new KeyNotFoundException($"Instance {clone.name} doesn't belong to any pools!"), clone);
				return;
			}

			_borrowedMap.Remove(clone);
			_pools[prefab].Add(clone);
		}

		private Pool CreatePool(Component prefab)
		{
			Transform poolParent = new GameObject(prefab.name).transform;
			poolParent.SetParent(_poolParent);
			
			var pool = new Pool(prefab, poolParent);
			_pools[prefab] = pool;
			
			return pool;
		}
	}
}