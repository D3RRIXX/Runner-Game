using Game.Blocks;
using Game.Configs;
using Game.Levels;
using Game.StateMachine;
using Infrastructure.EventBus;
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
			AllServices services = AllServices.Container;
			
			services.RegisterSingle<ISceneLoader>(new SceneLoader());
			services.RegisterSingle<ILevelService>(new LevelService(_levelList));
			services.RegisterSingle<IAssetProvider>(new AssetProvider());
			services.RegisterSingle<IEventService>(new EventService());
			RegisterPoolingManager();

			services.RegisterSingle<IGameFactory>(new GameFactory(services.GetSingle<ILevelService>(), GetBlockFactory()));

			services.RegisterSingle<IGameStateMachine>(new GameStateMachine(services));
			
			DontDestroyOnLoad(this);
		}

		private void RegisterPoolingManager()
		{
			var poolParent = new GameObject("Pools").transform;
			poolParent.SetParent(transform);
			AllServices.Container.RegisterSingle<IPoolingManager>(new PoolingManager(poolParent));
		}

		private void Start()
		{
			AllServices.Container.GetSingle<IGameStateMachine>().Enter<BootstrapState>();
		}

		private static Block.Factory GetBlockFactory()
		{
			var assetProvider = AllServices.Container.GetSingle<IAssetProvider>();
			var poolingManager = AllServices.Container.GetSingle<IPoolingManager>();
			var eventService = AllServices.Container.GetSingle<IEventService>();
			Transform parent = new GameObject("Blocks").transform;

			return new Block.Factory(assetProvider, poolingManager, eventService, parent);
		}
	}
}