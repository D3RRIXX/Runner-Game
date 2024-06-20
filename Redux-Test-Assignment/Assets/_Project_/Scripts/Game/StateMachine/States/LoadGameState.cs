using Game.Levels;
using Infrastructure.SceneLoadSystem;

namespace Game.StateMachine.States
{
	public class LoadGameState : IState
	{
		private const string GAME_SCENE_PATH = "Game";
		
		private readonly IGameStateMachine _gameStateMachine;
		private readonly ISceneLoader _sceneLoader;
		private readonly ILevelService _levelService;
		private readonly LevelProgress _levelProgress;

		public LoadGameState(IGameStateMachine gameStateMachine, ISceneLoader sceneLoader, ILevelService levelService, LevelProgress levelProgress)
		{
			_gameStateMachine = gameStateMachine;
			_sceneLoader = sceneLoader;
			_levelService = levelService;
			_levelProgress = levelProgress;
		}

		public void OnEnter()
		{
			_levelProgress.CleanUp();
			_sceneLoader.LoadScene(GAME_SCENE_PATH, OnGameSceneLoaded);
		}

		private void OnGameSceneLoaded()
		{
			_gameStateMachine.Enter<GameplayState, LevelConfig>(_levelService.GetNextLevel());
		}
		
		public void OnExit() { }
	}
}