using System;
using Game.Configs;

namespace Game.Levels
{
	public class LevelService : ILevelService
	{
		private readonly LevelList _levelList;

		public LevelService(LevelList levelList)
		{
			_levelList = levelList;
		}

		public int CurrentLevelIdx { get; private set; }
		
		public LevelConfig GetNextLevel()
		{
			if (CurrentLevelIdx < _levelList.Levels.Count)
				return _levelList.Levels[CurrentLevelIdx];

			var random = new Random(CurrentLevelIdx);
			return _levelList.Levels[random.Next(0, _levelList.Levels.Count)];
		}

		public void SetCurrentLevelCompleted() => CurrentLevelIdx++;
	}
}