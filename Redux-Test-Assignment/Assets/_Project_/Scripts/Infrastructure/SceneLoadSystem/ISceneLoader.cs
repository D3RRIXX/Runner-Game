using System;
using Infrastructure.ServiceLocator;

namespace Game.StateMachine
{
	public interface ISceneLoader : IService
	{
		void LoadScene(string scenePath, Action onComplete = null);
	}
}