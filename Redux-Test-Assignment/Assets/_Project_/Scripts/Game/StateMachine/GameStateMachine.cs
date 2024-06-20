using System;
using System.Collections.Generic;
using Game.Factory;
using Game.Levels;
using Game.StateMachine.States;
using Infrastructure.EventBus;
using Infrastructure.SceneLoadSystem;
using Infrastructure.ServiceLocator;
using UniRx;

namespace Game.StateMachine
{
	public class GameStateMachine : IGameStateMachine
	{
		private readonly Dictionary<Type, IExitableState> _states;
		private readonly ReactiveProperty<IExitableState> _currentState = new ReactiveProperty<IExitableState>();

		public GameStateMachine(AllServices services, LevelList levelList)
		{
			_states = new Dictionary<Type, IExitableState>
			{
				[typeof(BootstrapState)] = new BootstrapState(this, services, levelList),
				[typeof(LoadGameState)] = new LoadGameState(this, services.GetSingle<ISceneLoader>(), services.GetSingle<ILevelService>(), services.GetSingle<LevelState>()),
				[typeof(GameplayState)] = new GameplayState(this, services.GetSingle<IGameFactory>(), services.GetSingle<IEventService>(), services.GetSingle<LevelState>()),
				[typeof(LevelCompleteState)] = new LevelCompleteState(services.GetSingle<ILevelService>()),
				[typeof(LevelFailedState)] = new LevelFailedState()
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