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
			AllServices services = AllServices.Container;
			
			services.RegisterSingle<ILevelService>(new LevelService(_levelList));
			services.RegisterSingle<IAssetProvider>(new AssetProvider());

			RegisterPoolingManager();

			services.RegisterSingle<IGameFactory>(new GameFactory(services.GetSingle<ILevelService>(), GetBlockFactory()));
			
			DontDestroyOnLoad(this);
		}

		private void RegisterPoolingManager()
		{
			var poolParent = new GameObject("Pools").transform;
			poolParent.SetParent(transform);
			AllServices.Container.RegisterSingle<IPoolingManager>(new PoolingManager(poolParent));
		}

		private async void Start()
		{
			var gameFactory = AllServices.Container.GetSingle<IGameFactory>();

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