using Game.Configs;
using Infrastructure.ServiceLocator;

namespace Game.Levels
{
	public interface ILevelService : IService
	{
		int CurrentLevelIdx { get; }
		LevelConfig GetNextLevel();
	}
}