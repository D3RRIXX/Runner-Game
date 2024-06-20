using System;
using Game.Player;
using Infrastructure.ServiceLocator;

namespace Game.Levels
{
	public class LevelState : IService
	{
		private bool _initialized;
		
		public LevelConfig Config { get; private set; }
		public PlayerHealth Player { get; private set; }
		public int BlocksPassed { get; set; }

		public void Initialize(LevelConfig config, PlayerHealth player)
		{
			if (_initialized)
				throw new InvalidOperationException();
            
			Config = config;
			Player = player;
			_initialized = true;
		}

		public void CleanUp()
		{
			Config = null;
			BlocksPassed = 0;
			_initialized = false;
		}
	}
}