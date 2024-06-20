using System;
using Infrastructure.ServiceLocator;

namespace Game.Levels
{
	public class LevelProgress : IService
	{
		private bool _initialized;
		
		public LevelConfig Level { get; private set; }
		public int BlocksPassed { get; set; }

		public void Initialize(LevelConfig level)
		{
			if (_initialized)
				throw new InvalidOperationException();
            
			Level = level;
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