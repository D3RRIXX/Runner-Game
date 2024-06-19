using Game.Configs;
using Game.Levels;
using Infrastructure.PoolingSystem;
using Infrastructure.ServiceLocator;
using UnityEngine;

namespace Game
{
	public class GameBootstrap : MonoBehaviour
	{
		[SerializeField] private LevelList _levelList;

		private void Awake()
		{
			AllServices.Container.RegisterSingle<ILevelService>(new LevelService(_levelList));
			AllServices.Container.RegisterSingle<IAssetProvider>(new AssetProvider());

			var poolParent = new GameObject("Pools").transform;
			poolParent.SetParent(transform);
			AllServices.Container.RegisterSingle<IPoolingManager>(new PoolingManager(poolParent));
			
			DontDestroyOnLoad(this);
		}
	}
}