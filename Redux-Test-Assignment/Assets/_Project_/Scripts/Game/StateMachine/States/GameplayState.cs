using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Game.Events;
using Game.Factory;
using Game.Levels;
using Game.Player;
using Infrastructure.EventBus;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace Game.StateMachine.States
{
	public class GameplayState : IState<LevelConfig>
	{
		private readonly IGameStateMachine _gameStateMachine;
		private readonly IGameFactory _gameFactory;
		private readonly IEventService _eventService;
		private readonly LevelState _levelState;

		public GameplayState(IGameStateMachine gameStateMachine, IGameFactory gameFactory, IEventService eventService, LevelState levelState)
		{
			_gameStateMachine = gameStateMachine;
			_gameFactory = gameFactory;
			_eventService = eventService;
			_levelState = levelState;
		}

		public async void OnEnter(LevelConfig levelConfig)
		{
			await _gameFactory.WarmUp(levelConfig);
			await CreateGameLevel();
			
			PlayerHealth player = await CreatePlayer();
			_levelState.Initialize(levelConfig, player);

			await CreateUIRoot(player);
			
			_eventService.Subscribe<BlockPassedEvent>(OnBlockPassed);
			_eventService.Subscribe<LevelCompletedEvent>(OnLevelCompleted);
		}

		private async UniTask CreateUIRoot(PlayerHealth player)
		{
			await _gameFactory.CreateUIRoot(player);
		}

		public void OnExit()
		{
			_eventService.Unsubscribe<BlockPassedEvent>(OnBlockPassed);
			_eventService.Unsubscribe<LevelCompletedEvent>(OnLevelCompleted);
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

		private void OnBlockPassed(BlockPassedEvent evt)
		{
			Debug.Log($"Player passed {evt.Block.BlockType} block", evt.Block);
			SpawnNextBlock().Forget();
		}

		private async UniTask CreateGameLevel()
		{
			for (int i = 0; i < Constants.BLOCKS_AHEAD; i++)
				await SpawnNextBlock();
		}

		private UniTask SpawnNextBlock() => _gameFactory.TrySpawnNextBlock();

		private void OnLevelCompleted()
		{
			_gameStateMachine.Enter<LevelCompleteState>();
		}
	}
}