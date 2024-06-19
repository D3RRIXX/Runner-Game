using Infrastructure.ServiceLocator;
using UnityEngine;

namespace Infrastructure.PoolingSystem
{
	public interface IPoolingManager : IService
	{
		T Get<T>(T prefab) where T : Component;
		T Get<T>(T prefab, Transform parent) where T : Component;
		T Get<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : Component;
		void Return<T>(T clone) where T : Component;
	}
}