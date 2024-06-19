using Infrastructure.ServiceLocator;
using UniRx;

namespace Game.StateMachine
{
	public interface IGameStateMachine : IService
	{
		IReadOnlyReactiveProperty<IExitableState> CurrentState { get; }
		void Enter<TState>() where TState : IState;
		void Enter<TState, TData>(TData data) where TState : IState<TData>;
	}
}