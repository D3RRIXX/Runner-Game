using Game.Blocks;
using Game.Factory;
using Game.Levels;
using Game.Player;
using Infrastructure.AssetManagement;
using Infrastructure.EventBus;
using Infrastructure.PoolingSystem;
using Infrastructure.SceneLoadSystem;
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
			_stateMachine.Enter<LoadGameState>();
		}

		public void OnExit() { }

		private void RegisterServices(AllServices services, LevelList levelList)
		{
			services.RegisterSingle<ISceneLoader>(new SceneLoader());
			services.RegisterSingle<ILevelService>(new LevelService(levelList));
			services.RegisterSingle<IAssetProvider>(new AssetProvider());
			services.RegisterSingle<IEventService>(new EventService());
			services.RegisterSingle<IPoolingManager>(CreatePoolingManager());

			services.RegisterSingle<IGameFactory>(new GameFactory(CreateBlockFactory(services), CreatePlayerFactory(services)));
			services.RegisterSingle<IGameStateMachine>(_stateMachine);
		}

		private static PlayerFactory CreatePlayerFactory(AllServices services)
		{
			return new PlayerFactory(services.GetSingle<IAssetProvider>());
		}

		private static PoolingManager CreatePoolingManager()
		{
			var poolParent = new GameObject("Pools").transform;
			Object.DontDestroyOnLoad(poolParent);

			return new PoolingManager(poolParent);
		}

		private static Block.Factory CreateBlockFactory(AllServices services)
		{
			var assetProvider = services.GetSingle<IAssetProvider>();
			var poolingManager = services.GetSingle<IPoolingManager>();
			var eventService = services.GetSingle<IEventService>();

			return new Block.Factory(assetProvider, poolingManager, eventService);
		}
	}
}