using System;
using Infrastructure.ServiceLocator;

namespace Infrastructure.EventBus
{
	public interface IEventService : IService
	{
		void Subscribe<TEvent>(Action<TEvent> callback);
		void Subscribe<TEvent>(Action callback);
		void Unsubscribe<TEvent>(Action<TEvent> callback);
		void Unsubscribe<TEvent>(Action callback);
		void Fire<TEvent>(in TEvent evt);
		void Fire<TEvent>() where TEvent : new();
	}
}