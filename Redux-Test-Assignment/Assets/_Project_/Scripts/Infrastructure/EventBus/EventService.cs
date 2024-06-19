using System;
using System.Collections.Generic;

namespace Infrastructure.EventBus
{
	public class EventService : IEventService
	{
		private readonly Dictionary<Type, Delegate> _parametrizedCallbacks = new Dictionary<Type, Delegate>();
		private readonly Dictionary<Type, Action> _nonParametrizedCallbacks = new Dictionary<Type, Action>();

		public void Subscribe<TEvent>(Action<TEvent> callback)
		{
			Type type = typeof(TEvent);
			if (_parametrizedCallbacks.TryGetValue(type, out Delegate del))
			{
				_parametrizedCallbacks[type] = Delegate.Combine(del, callback);
			}
			else
			{
				_parametrizedCallbacks.Add(type, callback);
			}
		}

		public void Subscribe<TEvent>(Action callback)
		{
			Type type = typeof(TEvent);
			if (_nonParametrizedCallbacks.ContainsKey(type))
			{
				_nonParametrizedCallbacks[type] += callback;
			}
			else
			{
				_nonParametrizedCallbacks.Add(type, callback);
			}
		}

		public void Unsubscribe<TEvent>(Action<TEvent> callback)
		{
			Type type = typeof(TEvent);

			if (!_parametrizedCallbacks.TryGetValue(type, out Delegate del))
				return;

			del = Delegate.Remove(del, callback);
			if (del == null)
				_parametrizedCallbacks.Remove(type);
			else
				_parametrizedCallbacks[type] = del;
		}

		public void Unsubscribe<TEvent>(Action callback)
		{
			Type type = typeof(TEvent);

			if (!_nonParametrizedCallbacks.TryGetValue(type, out Action action))
				return;

			action -= callback;
			if (action == null)
				_nonParametrizedCallbacks.Remove(type);
			else
				_nonParametrizedCallbacks[type] = action;
		}

		public void Fire<TEvent>(in TEvent evt) => Fire_Internal(evt);
		public void Fire<TEvent>() where TEvent : new() => Fire_Internal(new TEvent());

		private void Fire_Internal<TEvent>(TEvent evt)
		{
			Type type = typeof(TEvent);
			if (_parametrizedCallbacks.TryGetValue(type, out Delegate del))
			{
				var parametrized = (Action<TEvent>)del;
				parametrized.Invoke(evt);
			}

			if (_nonParametrizedCallbacks.TryGetValue(type, out Action action))
			{
				action.Invoke();
			}
		}
	}
}