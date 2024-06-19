using System;
using System.Collections.Generic;
using Infrastructure.ServiceLocator;
using UniRx;

namespace Game.StateMachine
{
	public class GameStateMachine : IGameStateMachine
	{
		private readonly Dictionary<Type, IExitableState> _states;
		private readonly ReactiveProperty<IExitableState> _currentState = new ReactiveProperty<IExitableState>();

		public GameStateMachine(AllServices services)
		{
			_states = new Dictionary<Type, IExitableState>
			{
				[typeof(BootstrapState)] = new BootstrapState(),
				[typeof(LoadSceneState)] = new LoadSceneState(services.GetSingle<ISceneLoader>()),
				[typeof(GameplayState)] = new GameplayState()
			};
		}

		public IReadOnlyReactiveProperty<IExitableState> CurrentState => _currentState;

		public void Enter<TState>() where TState : IState => SwitchStateTo<TState>().OnEnter();

		public void Enter<TState, TData>(TData data) where TState : IState<TData> => SwitchStateTo<TState>().OnEnter(data);

		private TState SwitchStateTo<TState>() where TState : IExitableState
		{
			_currentState.Value?.OnExit();
			IExitableState state = _states[typeof(TState)];
			_currentState.Value = state;

			return (TState)state;
		}
	}
}