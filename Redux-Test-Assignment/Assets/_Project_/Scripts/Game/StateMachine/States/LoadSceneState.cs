namespace Game.StateMachine
{
	public class LoadSceneState : IState<string>
	{
		private readonly ISceneLoader _sceneLoader;

		public LoadSceneState(ISceneLoader sceneLoader)
		{
			_sceneLoader = sceneLoader;
		}

		public void OnEnter(string scenePath)
		{
			_sceneLoader.LoadScene(scenePath);
		}
		public void OnExit() { }
	}
}