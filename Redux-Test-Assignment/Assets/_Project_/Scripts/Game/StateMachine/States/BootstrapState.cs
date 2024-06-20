using Game.Blocks;
using Game.Factory;
using Game.Levels;
using Game.Player;
using Infrastructure.AssetManagement;
using Infrastructure.EventBus;
using Infrastructure.SceneLoadSystem;
using Infrastructure.ServiceLocator;

namespace Game.StateMachine.States
{
	public class BootstrapState : IState
	{
		private readonly IGameStateMachine _stateMachine;

		public BootstrapState(IGameStateMachine stateMachine, AllServices services, LevelGenerationConfig levelGenerationConfig)
		{
			_stateMachine = stateMachine;
			RegisterServices(services, levelGenerationConfig);
		}

		public void OnEnter()
		{
			_stateMachine.Enter<LoadGameState>();
		}

		public void OnExit() { }

		private void RegisterServices(AllServices services, LevelGenerationConfig levelGenerationConfig)
		{
			services.RegisterSingle<ISceneLoader>(new SceneLoader());
			services.RegisterSingle<ILevelService>(new LevelGenerationService(levelGenerationConfig));
			services.RegisterSingle<IAssetProvider>(new AssetProvider());
			services.RegisterSingle<IEventService>(new EventService());

			services.RegisterSingle<IGameFactory>(new GameFactory(CreateBlockFactory(services), CreatePlayerFactory(services), new UIFactory(services.GetSingle<IAssetProvider>())));
			services.RegisterSingle<IGameStateMachine>(_stateMachine);
			services.RegisterSingle(new LevelState());
			services.RegisterSingle<IPlayerRespawnManager>(new PlayerRespawnManager(services.GetSingle<IEventService>(), services.GetSingle<IGameStateMachine>(), services.GetSingle<LevelState>()));
		}

		private static PlayerFactory CreatePlayerFactory(AllServices services) => new PlayerFactory(services.GetSingle<IAssetProvider>());
		private static Block.Factory CreateBlockFactory(AllServices services) => new Block.Factory(services.GetSingle<IAssetProvider>());
	}
}