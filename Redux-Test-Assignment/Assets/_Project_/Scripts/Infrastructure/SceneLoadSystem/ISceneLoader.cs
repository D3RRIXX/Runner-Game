using System;
using Infrastructure.ServiceLocator;

namespace Infrastructure.SceneLoadSystem
{
	public interface ISceneLoader : IService
	{
		void LoadScene(string scenePath, Action onComplete = null);
	}
}