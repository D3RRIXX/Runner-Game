using Game.Blocks;
using Game.Configs;
using Game.Levels;
using Infrastructure.EventBus;
using Infrastructure.PoolingSystem;
using Infrastructure.ServiceLocator;
using UnityEngine;

namespace Game.StateMachine.States
{
	public class BootstrapState : IState
	{
		private readonly IGameStateMachine _stateMachine;

		public BootstrapState(IGameStateMachine stateMachine, AllServices services, LevelList levelList)
		{
			_stateMachine = stateMachine;
			RegisterServices(services, levelList);
		}
		
		public void OnEnter()
		{
			_stateMachine.Enter<LoadSceneState, string>("Game");
		}
		public void OnExit() { }
		
		private void RegisterServices(AllServices services, LevelList levelList)
		{
			services.RegisterSingle<ISceneLoader>(new SceneLoader());
			services.RegisterSingle<ILevelService>(new LevelService(levelList));
			services.RegisterSingle<IAssetProvider>(new AssetProvider());
			services.RegisterSingle<IEventService>(new EventService());
			RegisterPoolingManager();

			services.RegisterSingle<IGameFactory>(new GameFactory(services.GetSingle<ILevelService>(), CreateBlockFactory()));
			services.RegisterSingle<IGameStateMachine>(_stateMachine);
		}

		private void RegisterPoolingManager()
		{
			var poolParent = new GameObject("Pools").transform;
			Object.DontDestroyOnLoad(poolParent);
			
			AllServices.Container.RegisterSingle<IPoolingManager>(new PoolingManager(poolParent));
		}

		private static Block.Factory CreateBlockFactory()
		{
			var assetProvider = AllServices.Container.GetSingle<IAssetProvider>();
			var poolingManager = AllServices.Container.GetSingle<IPoolingManager>();
			var eventService = AllServices.Container.GetSingle<IEventService>();

			return new Block.Factory(assetProvider, poolingManager, eventService);
		}
	}
}