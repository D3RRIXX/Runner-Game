namespace Game.StateMachine
{
	public class BootstrapState : IState
	{
		private readonly IGameStateMachine _stateMachine;

		public BootstrapState(IGameStateMachine stateMachine)
		{
			_stateMachine = stateMachine;
		}
		
		public void OnEnter()
		{
			_stateMachine.Enter<LoadSceneState, string>("Game");
		}
		public void OnExit() { }
	}
}