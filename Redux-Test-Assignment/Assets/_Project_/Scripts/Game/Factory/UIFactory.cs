using Cysharp.Threading.Tasks;
using Game.Player;
using Infrastructure.AssetManagement;
using UI;
using UnityEngine;

namespace Game.Factory
{
	public class UIFactory
	{
		private readonly IAssetProvider _assetProvider;

		public UIFactory(IAssetProvider assetProvider)
		{
			_assetProvider = assetProvider;
		}

		public UniTask WarmUp() => _assetProvider.Load<GameObject>(AssetPaths.UI);
		
		public async UniTask<UIManager> CreateUIRoot(PlayerHealth player)
		{
			var prefab = await _assetProvider.Load<GameObject>(AssetPaths.UI);
			var uiManager = Object.Instantiate(prefab).GetComponent<UIManager>();
			
			return uiManager;
		}
	}
}