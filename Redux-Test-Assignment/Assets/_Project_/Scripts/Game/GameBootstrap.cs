using Game.Blocks;
using Game.Configs;
using Game.Levels;
using Infrastructure.PoolingSystem;
using Infrastructure.ServiceLocator;
using UnityEngine;

namespace Game
{
	public class GameBootstrap : MonoBehaviour
	{
		[SerializeField] private LevelList _levelList;

		private void Awake()
		{
			AllServices.Container.RegisterSingle<ILevelService>(new LevelService(_levelList));
			AllServices.Container.RegisterSingle<IAssetProvider>(new AssetProvider());

			var poolParent = new GameObject("Pools").transform;
			poolParent.SetParent(transform);
			AllServices.Container.RegisterSingle<IPoolingManager>(new PoolingManager(poolParent));

			DontDestroyOnLoad(this);
		}

		private async void Start()
		{
			var gameFactory = new GameFactory(AllServices.Container.GetSingle<ILevelService>(), GetBlockFactory());

			await gameFactory.WarmUp();
			await gameFactory.CreateGameLevel();
		}

		private static Block.Factory GetBlockFactory()
		{
			var assetProvider = AllServices.Container.GetSingle<IAssetProvider>();
			var poolingManager = AllServices.Container.GetSingle<IPoolingManager>();
			Transform parent = new GameObject("Blocks").transform;
			
			return new Block.Factory(assetProvider, poolingManager, parent);
		}
	}
}