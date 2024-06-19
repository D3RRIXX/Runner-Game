using UnityEngine.AddressableAssets;

namespace Game.StateMachine
{
	public class SceneLoader : ISceneLoader
	{
		public void LoadScene(string scenePath)
		{
			Addressables.LoadSceneAsync(scenePath);
		}
	}
}