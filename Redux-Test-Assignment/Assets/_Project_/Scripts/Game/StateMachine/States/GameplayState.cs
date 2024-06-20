using Game.Configs;

namespace Game.StateMachine.States
{
	public class GameplayState : IState<LevelConfig>
	{
		private readonly IGameFactory _gameFactory;

		public GameplayState(IGameFactory gameFactory)
		{
			_gameFactory = gameFactory;
		}

		public void OnExit() { }

		public async void OnEnter(LevelConfig levelConfig)
		{
			await _gameFactory.WarmUp(levelConfig);
			await _gameFactory.CreateGameLevel();
		}
	}
}