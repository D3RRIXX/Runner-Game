namespace Game.StateMachine.States
{
	public class LoadSceneState : IState<string>
	{
		private readonly IGameStateMachine _gameStateMachine;
		private readonly ISceneLoader _sceneLoader;

		public LoadSceneState(IGameStateMachine gameStateMachine, ISceneLoader sceneLoader)
		{
			_gameStateMachine = gameStateMachine;
			_sceneLoader = sceneLoader;
		}

		public void OnEnter(string scenePath)
		{
			_sceneLoader.LoadScene(scenePath, OnGameSceneLoaded);
		}

		private void OnGameSceneLoaded()
		{
			_gameStateMachine.Enter<GameplayState>();
		}
		
		public void OnExit() { }
	}
}