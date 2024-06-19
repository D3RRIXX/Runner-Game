namespace Game.StateMachine.States
{
	public class GameplayState : IState
	{
		private readonly IGameFactory _gameFactory;

		public GameplayState(IGameFactory gameFactory)
		{
			_gameFactory = gameFactory;
		}

		public void OnExit() { }

		public async void OnEnter()
		{
			await _gameFactory.WarmUp();
			await _gameFactory.CreateGameLevel();
		}
	}
}