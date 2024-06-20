using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Game.Blocks;
using Game.Events;
using Game.Factory;
using Game.Levels;
using Infrastructure.EventBus;
using UnityEngine;
using Utilities;

namespace Game.StateMachine.States
{
	public class GameplayState : IState<LevelConfig>
	{
		private readonly IGameStateMachine _gameStateMachine;
		private readonly IGameFactory _gameFactory;
		private readonly IEventService _eventService;

		private Block _currentBlock;

		public GameplayState(IGameStateMachine gameStateMachine, IGameFactory gameFactory, IEventService eventService)
		{
			_gameStateMachine = gameStateMachine;
			_gameFactory = gameFactory;
			_eventService = eventService;
		}

		public async void OnEnter(LevelConfig levelConfig)
		{
			await _gameFactory.WarmUp(levelConfig);
			await CreateGameLevel();

			_eventService.Subscribe<BlockPassedEvent>(OnBlockPassed);
			_eventService.Subscribe<LevelCompletedEvent>(OnLevelCompleted);
		}

		public void OnExit()
		{
			_eventService.Unsubscribe<BlockPassedEvent>(OnBlockPassed);
			_eventService.Unsubscribe<LevelCompletedEvent>(OnLevelCompleted);
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

		private async UniTask SpawnNextBlock()
		{
			_currentBlock = await _gameFactory.TrySpawnNextBlock();
		}

		private void OnLevelCompleted()
		{
			_gameStateMachine.Enter<LevelCompleteState>();
		}
	}
}