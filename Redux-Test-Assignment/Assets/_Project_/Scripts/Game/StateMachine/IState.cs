namespace Game.StateMachine
{
	public interface IState<TData> : IExitableState
	{
		void OnEnter(TData data);
	}

	public interface IState : IExitableState
	{
		void OnEnter();
	}
}