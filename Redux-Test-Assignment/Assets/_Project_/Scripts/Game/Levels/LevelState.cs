using System;
using Game.Player;
using Infrastructure.ServiceLocator;

namespace Game.Levels
{
	public class LevelState : IService
	{
		private bool _initialized;
		
		public LevelConfig Level { get; private set; }
		public PlayerHealth Player { get; private set; }
		public int BlocksPassed { get; set; }

		public void Initialize(LevelConfig level, PlayerHealth player)
		{
			if (_initialized)
				throw new InvalidOperationException();
            
			Level = level;
			Player = player;
			_initialized = true;
		}

		public void CleanUp()
		{
			Level = null;
			BlocksPassed = 0;
			_initialized = false;
		}
	}
}