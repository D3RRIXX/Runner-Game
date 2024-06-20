using Game.Configs;
using Game.Events;
using Infrastructure.EventBus;
using UniRx;
using UnityEngine;

namespace Game.StateMachine.States
{
	public class GameplayState : IState<LevelConfig>
	{
		private readonly IGameFactory _gameFactory;
		private readonly IEventService _eventService;

		public GameplayState(IGameFactory gameFactory, IEventService eventService)
		{
			_gameFactory = gameFactory;
			_eventService = eventService;
		}

		public async void OnEnter(LevelConfig levelConfig)
		{
			await _gameFactory.WarmUp(levelConfig);
			await _gameFactory.CreateGameLevel();
			
			_eventService.Subscribe<BlockPassedEvent>(OnBlockPassed);
		}

		public void OnExit()
		{
			_eventService.Unsubscribe<BlockPassedEvent>(OnBlockPassed);
		}

		private void OnBlockPassed(BlockPassedEvent evt)
		{
			Debug.Log($"Player passed {evt.Block.BlockType} block", evt.Block);
			_gameFactory.SpawnNextBlock();
		}
	}
}