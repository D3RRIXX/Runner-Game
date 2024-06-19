using System;

namespace Infrastructure.ServiceLocator
{
	public class AllServices
	{
		private static AllServices container;
		public static AllServices Container => container ??= new AllServices();

		public void RegisterSingle<TService>(TService implementation) where TService : IService
		{
			Implementation<TService>.Instance = implementation;
		}

		public TService GetSingle<TService>() where TService : IService
		{
			TService instance = Implementation<TService>.Instance;
			if (instance == null)
				throw new NullReferenceException($"No service of type {typeof(TService).Name} found!");

			return instance;
		}

		private static class Implementation<TService> where TService : IService
		{
			public static TService Instance;
		}
	}
}