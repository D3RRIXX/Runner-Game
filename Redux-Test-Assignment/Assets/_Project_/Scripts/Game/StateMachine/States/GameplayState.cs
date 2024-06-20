using Cysharp.Threading.Tasks;
using Game.Events;
using Game.Levels;
using Game.Player;
using Infrastructure.EventBus;

namespace Game.StateMachine.States
{
	public class GameplayState : IState
	{
		private readonly IGameStateMachine _gameStateMachine;
		private readonly IEventService _eventService;
		private readonly LevelState _levelState;
		private readonly IPlayerRespawnManager _respawnManager;

		public GameplayState(IGameStateMachine gameStateMachine, IEventService eventService, LevelState levelState, IPlayerRespawnManager respawnManager)
		{
			_gameStateMachine = gameStateMachine;
			_eventService = eventService;
			_levelState = levelState;
			_respawnManager = respawnManager;
		}

		public void OnEnter()
		{
			_respawnManager.Initialize();
			_eventService.Subscribe<BlockPassedEvent>(OnBlockPassed);
			_eventService.Subscribe<LevelCompletedEvent>(OnLevelCompleted);
		}

		public void OnExit()
		{
			_respawnManager.Dispose();
			_eventService.Unsubscribe<BlockPassedEvent>(OnBlockPassed);
			_eventService.Unsubscribe<LevelCompletedEvent>(OnLevelCompleted);
		}

		private void OnBlockPassed(BlockPassedEvent evt)
		{
			_levelState.BlocksPassed++;
		}

		private void OnLevelCompleted()
		{
			_gameStateMachine.Enter<LevelCompleteState>();
		}
	}
}