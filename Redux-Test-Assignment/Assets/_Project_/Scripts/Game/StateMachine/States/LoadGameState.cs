using System.Linq;
using Cysharp.Threading.Tasks;
using Game.Factory;
using Game.Levels;
using Game.Player;
using Infrastructure.SceneLoadSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace Game.StateMachine.States
{
	public class LoadGameState : IState
	{
		private const string GAME_SCENE_PATH = "Game";

		private readonly IGameStateMachine _gameStateMachine;
		private readonly ISceneLoader _sceneLoader;
		private readonly ILevelService _levelService;
		private readonly LevelState _levelState;
		private readonly IGameFactory _gameFactory;

		public LoadGameState(IGameStateMachine gameStateMachine, ISceneLoader sceneLoader, ILevelService levelService, LevelState levelState, IGameFactory gameFactory)
		{
			_gameStateMachine = gameStateMachine;
			_sceneLoader = sceneLoader;
			_levelService = levelService;
			_levelState = levelState;
			_gameFactory = gameFactory;
		}

		public void OnEnter()
		{
			_levelState.CleanUp();
			_gameFactory.CleanUp();
			_sceneLoader.LoadScene(GAME_SCENE_PATH, OnGameSceneLoaded);
		}

		public void OnExit() { }

		private async void OnGameSceneLoaded()
		{
			LevelConfig levelConfig = _levelService.GetNextLevel();

			await _gameFactory.WarmUp(levelConfig);
			await CreateGameLevel();

			PlayerHealth player = await CreatePlayer();
			_levelState.Initialize(levelConfig, player);

			await CreateUIRoot(player);
			
			_gameStateMachine.Enter<GameplayState>();
		}

		private async UniTask CreateGameLevel()
		{
			for (int i = 0; i < Constants.BLOCKS_AHEAD; i++)
				await _gameFactory.TrySpawnNextBlock();
		}

		private async UniTask CreateUIRoot(PlayerHealth player)
		{
			await _gameFactory.CreateUIRoot(player);
		}

		private async UniTask<PlayerHealth> CreatePlayer()
		{
			Scene gameScene = SceneManager.GetActiveScene();

			Transform spawnPoint = gameScene.GetRootGameObjects()
			                                .First(x => x.CompareTag(Constants.SPAWN_POINT_TAG))
			                                .transform;

			GameObject player = await _gameFactory.CreatePlayer(spawnPoint.position);
			return player.GetComponent<PlayerHealth>();
		}
	}
}