namespace Game.StateMachine
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
			_sceneLoader.LoadScene(scenePath);
		}
		public void OnExit() { }
	}
}