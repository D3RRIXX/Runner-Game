using System;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Game.StateMachine
{
	public class SceneLoader : ISceneLoader
	{
		public async void LoadScene(string scenePath, Action onComplete = null)
		{
			await Addressables.LoadSceneAsync(scenePath);
			onComplete?.Invoke();
		}
	}
}