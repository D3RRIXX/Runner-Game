using System;
using UniRx;

namespace Game.StateMachine
{
	public static class GameStateMachineExtensions
	{
		public static IObservable<TState> ObserveStateChangedTo<TState>(this IGameStateMachine stateMachine) where TState : IExitableState
		{
			return stateMachine.CurrentState
			                   .OfType<IExitableState, TState>();
		}
	}
}