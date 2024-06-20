using Game.Events;
using Game.Factory;
using Game.Levels;
using Infrastructure.EventBus;
using UnityEngine;

namespace Game.StateMachine.States
{
	public class GameplayState : IState<LevelConfig>
	{
		private readonly IGameStateMachine _gameStateMachine;
		private readonly IGameFactory _gameFactory;
		private readonly IEventService _eventService;

		public GameplayState(IGameStateMachine gameStateMachine, IGameFactory gameFactory, IEventService eventService)
		{
			_gameStateMachine = gameStateMachine;
			_gameFactory = gameFactory;
			_eventService = eventService;
		}

		public async void OnEnter(LevelConfig levelConfig)
		{
			await _gameFactory.WarmUp(levelConfig);
			await _gameFactory.CreateGameLevel();
			
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
			_gameFactory.SpawnNextBlock();
		}

		private void OnLevelCompleted()
		{
			_gameStateMachine.Enter<LevelCompleteState>();
		}
	}
}